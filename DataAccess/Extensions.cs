using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Linq.Expressions;

namespace DataAccess;

public enum Direction
{
    Ascending = 0,
    Descending
}

public static class EnumExtensions
{
    public static bool IsAscending(this Direction direction)
        => direction == Direction.Ascending;
}

public class Sort
{
    public string Property { get; set; }
    public Direction Direction { get; set; }
}

public static class QueryExtensions
{/*
    public static IQueryable<TSource> CreateOrderExpression<TSource>(this IQueryable<TSource> query, List<Sort>? orderFields) 
        => orderFields.IsNullOrEmpty() 
                ? query 
                : CreateOrderExpression(query, orderFields!, 0);*/

    public static async Task<List<T>> WhereNavigationPropertyInWithTrimAsync<T>(
        this IQueryable<T> source,
        string navigationPath,
        List<string> allowedValues,
        List<Sort>? orderFields)
    {
        // Build query expression to filter root entities
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression body = BuildNavigationExpression(parameter, navigationPath.Split('.'), allowedValues);

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        var filteredQuery = source.Where(lambda);
        if (!orderFields.IsNullOrEmpty())
            filteredQuery = filteredQuery.CreateOrderExpression(orderFields);

        var filtered = await filteredQuery.ToListAsync();

        // Trim matching collection elements from navigation property
        TrimNavigationCollection(filtered, navigationPath, allowedValues, orderFields);

        return filtered;
    }

    private static Expression BuildNavigationExpression(Expression parameter, string[] members, List<string> allowedValues)
    {
        Expression current = parameter;

        for (int i = 0; i < members.Length; i++)
        {
            var member = members[i];
            var property = current.Type.GetProperty(member);

            if (property == null)
                throw new InvalidOperationException($"Property '{member}' not found on type '{current.Type.Name}'.");

            if (IsEnumerableButNotString(property.PropertyType))
            {
                var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault()
                                 ?? property.PropertyType.GetElementType();

                var anyParameter = Expression.Parameter(elementType, "e");

                var innerExpression = BuildNavigationExpression(anyParameter, members[(i + 1)..], allowedValues);
                var anyLambda = Expression.Lambda(innerExpression, anyParameter);

                var anyMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(elementType);

                return Expression.Call(anyMethod, Expression.PropertyOrField(current, member), anyLambda);
            }
            else
            {
                current = Expression.PropertyOrField(current, member);
            }
        }

        // partial string matching

        Expression? predicate = null;

        foreach (var value in allowedValues)
        {
            var searchValue = Expression.Constant(value);
            var toStringCall = Expression.Call(current, "ToString", Type.EmptyTypes);

            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            var containsCall = Expression.Call(toStringCall, containsMethod, searchValue);

            predicate = predicate == null
                ? containsCall
                : Expression.OrElse(predicate, containsCall);
        }

        return predicate!;

        /*
        var valuesExpression = Expression.Constant(allowedValues);
        var containsMethod = typeof(List<string>).GetMethod(nameof(List<string>.Contains), new[] { typeof(string) });

        return Expression.Call(valuesExpression, containsMethod, current);*/
    }

    private static bool IsEnumerableButNotString(Type type) =>
        typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string);

    private static void TrimNavigationCollection<T>(IEnumerable<T> rootEntities, string navigationPath, List<string> allowedValues, List<Sort> orderFields)
    {
        var members = navigationPath.Split('.');
        if (members.Length < 2)
            return;

        var collectionProp = typeof(T).GetProperty(members[0]);
        if (collectionProp == null)
            throw new InvalidOperationException($"Property '{members[0]}' not found on {typeof(T).Name}");

        foreach (var root in rootEntities)
        {
            var collection = collectionProp.GetValue(root) as System.Collections.IEnumerable;
            if (collection == null) continue;

            var elementType = collectionProp.PropertyType.GetGenericArguments().FirstOrDefault()
                              ?? collectionProp.PropertyType.GetElementType();

            var filteredList = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var item in collection)
            {
                var current = item;
                for (int i = 1; i < members.Length; i++)
                {
                    var prop = current.GetType().GetProperty(members[i]);
                    if (prop == null)
                        throw new InvalidOperationException($"Property '{members[i]}' not found on type '{current.GetType().Name}'");

                    current = prop.GetValue(current);
                    if (current == null) break;
                }

                foreach (var value in allowedValues)
                {
                    if (current?.ToString().Contains(value) is true)
                    {
                        filteredList.Add(item);
                        break;
                    }
                }
                /*
                if (allowedValues.Contains(current?.ToString()))
                    filteredList.Add(item);*/
            }

            var filteredOrderFields = orderFields
                ?.Where(sort => IsValidPropertyPath(elementType, sort.Property))
                .ToList();

            var orderedList = ApplyOrdering(filteredList, filteredOrderFields);

            var targetCollectionType = collectionProp.PropertyType;

            // Create an instance of the target collection type (usually ICollection<Brand>, HashSet<Brand>, or List<Brand>)
            object newCollection;

            if (targetCollectionType.IsInterface)
            {
                // If the property is declared as interface (like ICollection<T>), create a concrete type instance (e.g., HashSet<T>)
                var concreteType = typeof(HashSet<>).MakeGenericType(elementType);
                newCollection = Activator.CreateInstance(concreteType)!;
            }
            else
            {
                // If it's a concrete type, instantiate it directly
                newCollection = Activator.CreateInstance(targetCollectionType)!;
            }

            // Use reflection to call Add method for each item in filteredList
            var addMethod = targetCollectionType.GetMethod("Add")
                            ?? newCollection.GetType().GetMethod("Add");

            foreach (var item in filteredList)
            {
                addMethod!.Invoke(newCollection, new object[] { item });
            }

            // Finally assign the new collection to the property
            collectionProp.SetValue(root, newCollection);
        }
    }

    private static bool IsValidPropertyPath(Type type, string path)
    {
        var current = type;
        foreach (var part in path.Split('.'))
        {
            var prop = current.GetProperty(part);
            if (prop == null) return false;
            current = prop.PropertyType;
        }
        return true;
    }

    private static object ApplyOrdering(IList sourceList, List<Sort>? orderFields)
    {
        if (sourceList.Count == 0 || orderFields.IsNullOrEmpty())
            return sourceList;

        var elementType = sourceList[0].GetType();
        var param = Expression.Parameter(elementType, "x");

        // Step 1: Cast IList to IEnumerable<elementType>
        var castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast))!
            .MakeGenericMethod(elementType);
        var typedEnumerable = castMethod.Invoke(null, new object[] { sourceList })!;

        // Step 2: Get IQueryable<elementType> via AsQueryable
        var asQueryableMethod = typeof(Queryable).GetMethod(nameof(Queryable.AsQueryable), new Type[] { typeof(IEnumerable<>).MakeGenericType(elementType) })!;
        var queryable = (IQueryable)asQueryableMethod.Invoke(null, new object[] { typedEnumerable })!;

        // Step 3: Apply ordering for each Sort
        for (int i = 0; i < orderFields.Count; i++)
        {
            var sort = orderFields[i];

            Expression propertyAccess = param;
            Type currentType = elementType;
            foreach (var part in sort.Property.Split('.'))
            {
                var propertyInfo = currentType.GetProperty(part);
                if (propertyInfo == null)
                    throw new InvalidOperationException($"Property '{part}' not found on type '{currentType.Name}'");

                propertyAccess = Expression.Property(propertyAccess, propertyInfo);
                currentType = propertyInfo.PropertyType;
            }

            var lambda = Expression.Lambda(propertyAccess, param);

            string methodName;
            if (i == 0)
                methodName = sort.Direction.IsAscending() ? "OrderBy" : "OrderByDescending";
            else
                methodName = sort.Direction.IsAscending() ? "ThenBy" : "ThenByDescending";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(elementType, currentType);

            queryable = (IQueryable)method.Invoke(null, new object[] { queryable, lambda })!;
        }

        // Step 4: Convert back to List<elementType>
        var toListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!
            .MakeGenericMethod(elementType);

        return toListMethod.Invoke(null, new object[] { queryable })!;
    }

    public static IQueryable<TSource> CreateOrderExpression<TSource>(
        this IQueryable<TSource> query,
        List<Sort> orderFields,
        int index = 0)
    {
        if (index >= orderFields.Count)
            return query;

        var sort = orderFields[index];
        var parameter = Expression.Parameter(typeof(TSource), "x");
        Expression body = parameter;
        Type currentType = typeof(TSource);

        foreach (var part in sort.Property.Split('.'))
        {
            var prop = currentType.GetProperty(part);
            if (prop == null)
                throw new InvalidOperationException($"Property '{part}' not found on type '{currentType.Name}'.");

            body = Expression.Property(body, prop);
            currentType = prop.PropertyType;
        }

        var lambda = Expression.Lambda(body, parameter);
        var methodName = index == 0
            ? (sort.Direction.IsAscending() ? "OrderBy" : "OrderByDescending")
            : (sort.Direction.IsAscending() ? "ThenBy" : "ThenByDescending");

        var method = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(typeof(TSource), currentType);

        var orderedQuery = (IQueryable<TSource>)method.Invoke(null, new object[] { query, lambda })!;
        return orderedQuery.CreateOrderExpression(orderFields, index + 1);
    }
    /*
    private static IQueryable<TSource> CreateOrderExpression<TSource>(IQueryable<TSource> query, List<Sort> orderFields, int index)
    {
        if (orderFields.Count == index)
            return query;

        const string orderBy = nameof(Queryable.OrderBy);
        const string orderByDescending = nameof(Queryable.OrderByDescending);

        const string orderThenBy = nameof(Queryable.ThenBy);
        const string orderThenByDescending = nameof(Queryable.ThenByDescending);

        string methodName;
        var order = orderFields[index];

        var param = Expression.Parameter(typeof(TSource), "o");
        var prop = Expression.Property(param, order.Property);
        var getProp = Expression.Lambda(prop, param);

        var isAscending = order.Direction.IsAscending();

        if (index == 0)
            methodName = isAscending ? orderBy : orderByDescending;
        else
            methodName = isAscending ? orderThenBy : orderThenByDescending;

        var expression = Expression.Call(
            typeof(Queryable),
            methodName,
            [query.ElementType, prop.Type],
            [query.Expression, Expression.Quote(getProp)]
            );

        return CreateOrderExpression(query.Provider.CreateQuery<TSource>(expression), orderFields, ++index);
    }*/
}


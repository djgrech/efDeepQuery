using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess;

public static class QueryExtensions
{
    public static async Task<List<T>> WhereNavigationPropertyInWithTrimAsync<T>(
        this IQueryable<T> source,
        string navigationPath,
        List<string> allowedValues)
    {
        // Build query expression to filter root entities
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression body = BuildNavigationExpression(parameter, navigationPath.Split('.'), allowedValues);

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        var filtered = await source.Where(lambda).ToListAsync();

        // Trim matching collection elements from navigation property
        TrimNavigationCollection(filtered, navigationPath, allowedValues);

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

        var valuesExpression = Expression.Constant(allowedValues);
        var containsMethod = typeof(List<string>).GetMethod(nameof(List<string>.Contains), new[] { typeof(string) });

        return Expression.Call(valuesExpression, containsMethod, current);
    }

    private static bool IsEnumerableButNotString(Type type) =>
        typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string);

    private static void TrimNavigationCollection<T>(IEnumerable<T> rootEntities, string navigationPath, List<string> allowedValues)
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

                if (allowedValues.Contains(current?.ToString()))
                    filteredList.Add(item);
            }

            collectionProp.SetValue(root, filteredList);
        }
    }
}


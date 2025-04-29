using System.Linq.Expressions;

namespace EfDeepQuery;

public static class QueryExtensions
{
    public static IQueryable<T> WhereNavigationPropertyIn<T>(
        this IQueryable<T> source,
        string navigationPath,
        List<string> allowedValues)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression body = BuildNavigationExpression(parameter, navigationPath.Split('.'), allowedValues);

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
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
                // If the property is a collection (e.g., Posts), insert an Any() and continue deeper
                var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault()
                                 ?? property.PropertyType.GetElementType();

                var anyParameter = Expression.Parameter(elementType, "e");

                // Recursively build for the rest of the path
                var innerExpression = BuildNavigationExpression(anyParameter, members[(i + 1)..], allowedValues);

                var anyLambda = Expression.Lambda(innerExpression, anyParameter);

                var anyMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(elementType);

                return Expression.Call(anyMethod, Expression.PropertyOrField(current, member), anyLambda);
            }
            else
            {
                // Normal property (non-collection), just access it
                current = Expression.PropertyOrField(current, member);
            }
        }

        // Once we arrive at a final scalar property (like Name), do allowedValues.Contains(property)
        var valuesExpression = Expression.Constant(allowedValues);
        var containsMethod = typeof(List<string>).GetMethod(nameof(List<string>.Contains), new[] { typeof(string) });

        return Expression.Call(valuesExpression, containsMethod, current);
    }

    private static bool IsEnumerableButNotString(Type type)
    {
        return typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }
}

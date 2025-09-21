using System.Linq.Expressions;

namespace Common;

public static class ExpressionsExtensions
{

    public static string GetPropertyName<T, TResult>(this Expression<Func<T, TResult>> expression)
    {
        var memberNames = new Stack<string>();

        var current = expression.Body;

        if (current.NodeType == ExpressionType.Convert && current is UnaryExpression unaryExp)
            current = unaryExp.Operand;

        while (current is MemberExpression memberExp)
        {
            memberNames.Push(memberExp.Member.Name);
            current = memberExp.Expression;
        }

        return string.Join(".", memberNames);
    }
}

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
        => s == null || s.Length == 0;
}
public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        => collection == null || !collection.Any();
}

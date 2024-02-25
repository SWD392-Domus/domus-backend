using System.Linq.Expressions;

namespace Domus.Common.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), typeof(T).Name);

        var combinedExpr = Expression.Or(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));

        return Expression.Lambda<Func<T, bool>>(combinedExpr, param);
    } 
}
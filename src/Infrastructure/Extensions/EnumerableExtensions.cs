using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Extensions;
public static class EnumerableeExtensions
{
    public static IEnumerable<T> ApplyFilters<T>(this IEnumerable<T> list, IDictionary<string, string?> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null || string.IsNullOrEmpty(filter.Value)) continue;

            var left = Expression.Property(parameter, property);
            Expression predicate;

            if (property.PropertyType == typeof(string))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(filter.Value, typeof(string));
                predicate = Expression.Call(left, method!, right);
            }
            else
            {
                object? convertedValue;
                try
                {
                    convertedValue = QueryableExtensions.ConvertFilterValue(filter.Value, property.PropertyType);
                }
                catch
                {
                    continue;
                }

                var right = Expression.Constant(convertedValue, property.PropertyType);
                predicate = Expression.Equal(left, right);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            list = list.AsQueryable().Where(lambda);
        }

        return list;
    }
}

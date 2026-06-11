using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Extensions;
public static class QueryableExtensions
{
    public static (string? SortBy, string? SortDirection, IDictionary<string, string?> Filters) ExtractSorting(
        IDictionary<string, string?>? filters)
    {
        if (filters == null || filters.Count == 0)
        {
            return (null, null, new Dictionary<string, string?>());
        }

        var remaining = new Dictionary<string, string?>(filters, StringComparer.OrdinalIgnoreCase);
        remaining.TryGetValue("sortBy", out var sortBy);
        remaining.TryGetValue("sortDirection", out var sortDirection);
        remaining.Remove("sortBy");
        remaining.Remove("sortDirection");
        remaining.Remove("organizationId");
        remaining.Remove("scrollCount");

        return (sortBy, sortDirection, remaining);
    }

    internal static object? ConvertFilterValue(string? value, Type propertyType)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType == typeof(Guid))
        {
            return Guid.Parse(value);
        }

        if (underlyingType == typeof(string))
        {
            return value;
        }

        if (underlyingType.IsEnum)
        {
            return Enum.Parse(underlyingType, value, ignoreCase: true);
        }

        if (underlyingType == typeof(bool))
        {
            return bool.Parse(value);
        }

        return Convert.ChangeType(value, underlyingType);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, string? sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query;
        }

        var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var lambda = Expression.Lambda(propertyAccess, parameter);
        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        var methodName = descending ? "OrderByDescending" : "OrderBy";

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), property.PropertyType],
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExpression);
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string?> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null || string.IsNullOrEmpty(filter.Value))
            {
                continue;
            }

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
                    convertedValue = ConvertFilterValue(filter.Value, property.PropertyType);
                }
                catch
                {
                    continue;
                }

                var right = Expression.Constant(convertedValue, property.PropertyType);
                predicate = Expression.Equal(left, right);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}

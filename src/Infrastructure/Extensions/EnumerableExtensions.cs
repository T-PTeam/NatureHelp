using Domain.Models.Organization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

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

            if (property.Name == "Location")
            {
                var locationFilters = JsonSerializer.Deserialize<Dictionary<string, string?>>(filter.Value);

                if (locationFilters != null)
                {
                    foreach (var locationFilter in locationFilters)
                    {
                        var locationPropertyName = locationFilter.Key;
                        var locationPropertyValue = locationFilter.Value;

                        var locationProperty = typeof(Location).GetProperty(locationPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (locationProperty != null && !string.IsNullOrEmpty(locationPropertyValue))
                        {
                            // Apply filter on Location properties
                            var locationExpression = Expression.Property(Expression.Property(parameter, "Location"), locationProperty);
                            var right = Expression.Constant(Convert.ChangeType(locationPropertyValue, locationProperty.PropertyType));
                            var equals = Expression.Equal(locationExpression, right);
                            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                            list = list.AsQueryable().Where(lambda);
                        }
                    }
                }
            }
            else
            {
                var left = Expression.Property(parameter, property);
                Expression predicate;

                if (property.PropertyType == typeof(string))
                {
                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var right = Expression.Constant(filter.Value.ToString(), typeof(string));
                    predicate = Expression.Call(left, method, right);
                }
                else if (property.PropertyType.IsEnum)
                {
                    var value = Enum.Parse(property.PropertyType, filter.Value.ToString(), ignoreCase: true);
                    var right = Expression.Constant(value);
                    predicate = Expression.Equal(left, right);
                }
                else
                {
                    var right = Expression.Constant(Convert.ChangeType(filter.Value, property.PropertyType));
                    predicate = Expression.Equal(left, right);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                list = list.AsQueryable().Where(lambda);
            }
        }

        return list;
    }

    private static IEnumerable<T> ApplyLocationFilters<T>(IEnumerable<T> list, IDictionary<string, string?> locationFilters, ParameterExpression parameter)
    {
        foreach (var locFilter in locationFilters)
        {
            var locationProperty = typeof(Location).GetProperty(locFilter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (locationProperty != null && !string.IsNullOrEmpty(locFilter.Value))
            {
                var locationExpression = Expression.Property(Expression.Property(parameter, "Location"), locationProperty);
                var right = Expression.Constant(Convert.ChangeType(locFilter.Value, locationProperty.PropertyType));
                var equals = Expression.Equal(locationExpression, right);
                var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                list = list.AsQueryable().Where(lambda);
            }
        }

        return list;
    }
}

using Domain.Models.Organization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Extensions;
public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string?> filters)
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
                            query = query.Where(lambda);
                        }
                    }
                }
            }
            else
            {
                var left = Expression.Property(parameter, property);
                var right = Expression.Constant(Convert.ChangeType(filter.Value, property.PropertyType));
                var equals = Expression.Equal(left, right);
                var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                query = query.Where(lambda);
            }
        }

        return query;
    }

    private static IQueryable<T> ApplyLocationFilters<T>(IQueryable<T> query, IDictionary<string, string?> locationFilters, ParameterExpression parameter)
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
                query = query.Where(lambda);
            }
        }

        return query;
    }
}

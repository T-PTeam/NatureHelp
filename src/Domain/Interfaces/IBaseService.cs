using Shared.Dtos;

namespace Domain.Interfaces;
public interface IBaseService<T> where T : class
{
    Task<ListData<T>> GetList(int scrollCount, IDictionary<string, string?>? filters);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<ListData<T>> GetOrSetAsync(Func<Task<ListData<T>>> fetchFromDb);
}
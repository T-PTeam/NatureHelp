using Shared.Dtos;

namespace Domain.Interfaces;
public interface IBaseService<T> where T : class
{
    Task<ListData<T>> GetList(int scrollCount);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
}

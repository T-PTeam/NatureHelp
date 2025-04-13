namespace Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters = null);
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(IEnumerable<T> list);
        Task<int> GetTotalCount();
        Task SaveChangesAsync();
    }
}

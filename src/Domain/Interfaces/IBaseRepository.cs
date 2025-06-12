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
        Task<Guid> DeleteAsync(Guid id);
        Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> ids);
        Task<int> GetTotalCount();
        Task SaveChangesAsync();
    }
}

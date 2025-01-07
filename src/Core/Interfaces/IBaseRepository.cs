namespace Core.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(IEnumerable<T> list);
        Task SaveChangesAsync();
    }
}

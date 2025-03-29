using Domain.Interfaces;
using Shared.Dtos;

namespace Application.Interfaces.Services;
public class BaseService<T> : IBaseService<T> where T : class
{
    protected readonly IBaseRepository<T> _repository;

    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ListData<T>> GetList(int scrollCount)
    {
        var originalData = await _repository.GetAllAsync(scrollCount);

        var totalCount = await _repository.GetTotalCount();

        var result = new ListData<T>()
        {
            List = originalData,
            TotalCount = totalCount,
        };

        return result;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

    public virtual async Task<T> AddAsync(T entity) => await _repository.AddAsync(entity);

    public virtual async Task<T> UpdateAsync(T entity) => await _repository.UpdateAsync(entity);
}

using Application.Interfaces.Services.Cache;
using Domain.Interfaces;
using Shared.Dtos;
using System.Text.Json;

namespace Application.Services;
public class BaseService<T> : IBaseService<T> where T : class
{
    protected readonly IBaseRepository<T> _repository;
    private readonly IRedisCacheService _redisCacheService;

    public BaseService(IBaseRepository<T> repository, IRedisCacheService? redisCacheService = null)
    {
        _repository = repository;
        _redisCacheService = redisCacheService;
    }

    public virtual async Task<ListData<T>> GetList(int scrollCount, IDictionary<string, string?>? filters)
    {
        var originalData = await _repository.GetAllAsync(scrollCount, filters);

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

    public virtual async Task<Guid> DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    public virtual async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> ids) => await _repository.DeleteRangeAsync(ids);

    public async Task<ListData<T>> GetOrSetAsync(Func<Task<ListData<T>>> fetchFromDb, bool isFiltering)
    {
        string key = typeof(T).Name + "_listdata";

        var cached = await _redisCacheService.GetAsync(key);
        if (cached != null && !isFiltering)
        {
            var cachedData = JsonSerializer.Deserialize<List<T>>(cached);
            if (cachedData != null)
                return new ListData<T>()
                {
                    List = cachedData,
                    TotalCount = cachedData.Count
                };
        }

        var data = await fetchFromDb();

        if (!isFiltering) await _redisCacheService.SetAsync(key, JsonSerializer.Serialize(data.List), TimeSpan.FromHours(2));

        return data;
    }

    protected List<T> ApplyScroll(List<T> list, int scrollCount) =>
        list.Take(scrollCount).ToList();
}

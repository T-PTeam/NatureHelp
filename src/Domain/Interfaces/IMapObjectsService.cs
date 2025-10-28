using Shared.Dtos;

namespace Domain.Interfaces;
public interface IMapObjectsService<T, MAPT> : IBaseService<T> where T : class
{
    Task<ListData<MAPT>> GetMapObjectsAsync(IDictionary<string, string?>? filters);
}

using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IMapObjectsRepository<T, MAPT> : IBaseRepository<T> where T : class
    {
        Task<IEnumerable<MAPT>> GetMapObjects(IDictionary<string, string?>? filters = null);
    }
}

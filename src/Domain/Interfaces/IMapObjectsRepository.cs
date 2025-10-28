using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Interfaces
{
    public interface IMapObjectsRepository<T, MAPT> : IBaseRepository<T> where T : class
    {
        Task<IEnumerable<MAPT>> GetMapObjects(IDictionary<string, string?>? filters = null, User? currentUser = null);
    }
}

using Domain.Interfaces;
using Domain.Models.Analitycs;

namespace Infrastructure.Interfaces;
public interface IResearchRepository : IBaseRepository<Research>
{
    public Task<IEnumerable<Research>> GetByLabId(Guid labId);
}
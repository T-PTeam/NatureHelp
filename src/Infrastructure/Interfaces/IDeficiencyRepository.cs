using Domain.Interfaces;
using Domain.Models.Nature;

namespace Infrastructure.Interfaces;
public interface IDeficiencyRepository<D> : IBaseRepository<D> where D : Deficiency
{
}

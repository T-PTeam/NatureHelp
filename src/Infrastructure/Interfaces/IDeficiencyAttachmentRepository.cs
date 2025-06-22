using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Nature;

namespace Infrastructure.Interfaces;
public interface IDeficiencyAttachmentRepository : IBaseRepository<DeficiencyAttachment>
{
    public Task<IEnumerable<DeficiencyAttachment>> GetAttachmentsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType);
}

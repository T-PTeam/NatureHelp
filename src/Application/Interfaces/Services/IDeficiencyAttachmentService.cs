using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Nature;

namespace Application.Interfaces.Services;
public interface IDeficiencyAttachmentService : IBaseService<Attachment>
{
    public Task<IEnumerable<DeficiencyAttachment>> GetAttachmentsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType);
}

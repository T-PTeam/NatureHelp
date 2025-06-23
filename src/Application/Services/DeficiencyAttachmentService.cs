using Application.Interfaces.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Nature;
using Infrastructure.Interfaces;

namespace Application.Services;
public class DeficiencyAttachmentService : BaseService<Attachment>, IDeficiencyAttachmentService
{
    private readonly IDeficiencyAttachmentRepository _deficiencyAttachmentRepository;

    public DeficiencyAttachmentService(
        IBaseRepository<Attachment> attachmentRepository,
        IDeficiencyAttachmentRepository deficiencyAttachmentRepository)
        : base(attachmentRepository)
    {
        _deficiencyAttachmentRepository = deficiencyAttachmentRepository;
    }

    public async Task<IEnumerable<DeficiencyAttachment>> GetAttachmentsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType)
    {
        return await _deficiencyAttachmentRepository.GetAttachmentsByDeficiencyIdAsync(deficiencyId, deficiencyType);
    }
}

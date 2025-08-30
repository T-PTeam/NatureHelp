using Application.Interfaces.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Interfaces;

namespace Application.Services;
public class DeficiencyBindModelService<T> : BaseService<T>, IModelByDeficiencyService<T> where T : class
{
    private readonly IDeficiencyBindModelRepository<T> _deficiencyAttachmentRepository;

    public DeficiencyBindModelService(
        IBaseRepository<T> attachmentRepository,
        IDeficiencyBindModelRepository<T> deficiencyAttachmentRepository)
        : base(attachmentRepository)
    {
        _deficiencyAttachmentRepository = deficiencyAttachmentRepository;
    }

    public async Task<IEnumerable<T>> GetModelsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType)
    {
        return await _deficiencyAttachmentRepository.GetModelsByDeficiencyIdAsync(deficiencyId, deficiencyType);
    }
}

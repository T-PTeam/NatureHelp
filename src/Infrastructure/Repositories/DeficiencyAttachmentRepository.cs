using Domain.Enums;
using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DeficiencyAttachmentRepository : BaseRepository<DeficiencyAttachment>, IDeficiencyAttachmentRepository
{
    public DeficiencyAttachmentRepository(IDbContextFactory<ApplicationContext> contextFactory) : base(contextFactory)
    { }

    public async Task<IEnumerable<DeficiencyAttachment>> GetAttachmentsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType)
    {
        var context = _contextFactory.CreateDbContext();

        var query = context.Set<DeficiencyAttachment>()
            .Where(a => a.DeficiencyId == deficiencyId && a.DeficiencyType == deficiencyType);

        return await query.ToListAsync();
    }
}

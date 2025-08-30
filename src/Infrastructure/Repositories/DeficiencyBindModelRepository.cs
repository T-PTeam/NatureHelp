using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DeficiencyBindModelRepository<T> : BaseRepository<T>, IDeficiencyBindModelRepository<T> where T : BaseModel, IDeficiencyBindModel
{
    public DeficiencyBindModelRepository(IDbContextFactory<ApplicationContext> contextFactory) : base(contextFactory)
    { }

    public async Task<IEnumerable<T>> GetModelsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType)
    {
        var context = _contextFactory.CreateDbContext();

        var query = context.Set<T>()
            .Where(a => a.DeficiencyId == deficiencyId && a.DeficiencyType == deficiencyType);

        return await query.ToListAsync();
    }
}

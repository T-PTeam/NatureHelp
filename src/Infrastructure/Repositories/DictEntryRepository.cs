using Domain.Models.Profile;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DictEntryRepository : IDictEntryRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public DictEntryRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<string?> GetValueJsonByKeyAsync(string entryKey, CancellationToken ct = default)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync(ct);
        return await ctx.DictEntries.AsNoTracking()
            .Where(e => e.EntryKey == entryKey)
            .Select(e => e.ValueJson)
            .FirstOrDefaultAsync(ct);
    }
}

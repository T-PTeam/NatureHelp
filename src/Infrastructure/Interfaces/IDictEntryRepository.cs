namespace Infrastructure.Interfaces;

public interface IDictEntryRepository
{
    Task<string?> GetValueJsonByKeyAsync(string entryKey, CancellationToken ct = default);
}

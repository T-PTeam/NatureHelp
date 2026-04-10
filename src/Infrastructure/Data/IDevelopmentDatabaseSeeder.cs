namespace Infrastructure.Data;

public interface IDevelopmentDatabaseSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}

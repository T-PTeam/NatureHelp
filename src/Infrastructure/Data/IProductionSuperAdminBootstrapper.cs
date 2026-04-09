namespace Infrastructure.Data;

public interface IProductionSuperAdminBootstrapper
{
    Task EnsureSuperAdminAsync(CancellationToken cancellationToken = default);
}

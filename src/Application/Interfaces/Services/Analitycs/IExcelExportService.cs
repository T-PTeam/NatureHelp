namespace Application.Interfaces.Services.Analitycs;
public interface IExcelExportService
{
    public Task<byte[]> GenerateWaterDeficienciesTableAsync();
    public Task<byte[]> GenerateSoilDeficienciesTableAsync();
    public Task<byte[]> GenerateOrgUsersTableAsync();
}

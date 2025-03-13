namespace Application.Interfaces.Services.Analitycs;
public interface IExcelExportService
{
    public Task<byte[]> GenerateWaterDeficienciesTable();
    public Task<byte[]> GenerateSoilDeficienciesTable();
}

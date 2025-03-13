using Application.Interfaces.Services.Analitycs;
using ClosedXML.Excel;
using Domain.Models.Nature;
using Infrastructure.Interfaces;
using System.Data;

namespace Application.Services.Analitycs;
public class ExcelExportService : IExcelExportService
{
    private readonly IDeficiencyRepository<WaterDeficiency> _waterDeficiencyService;
    private readonly IDeficiencyRepository<SoilDeficiency> _soilDeficiencyService;

    public ExcelExportService(
        IDeficiencyRepository<WaterDeficiency> waterDeficiencyService,
        IDeficiencyRepository<SoilDeficiency> soilDeficiencyService
        )
    {
        _waterDeficiencyService = waterDeficiencyService;
        _soilDeficiencyService = soilDeficiencyService;
    }

    public async Task<byte[]> GenerateWaterDeficienciesTable()
    {
        var data = await _soilDeficiencyService.GetAllAsync();

        return GenerateWorkBook(data);
    }

    public async Task<byte[]> GenerateSoilDeficienciesTable()
    {
        var data = await _soilDeficiencyService.GetAllAsync();

        return GenerateWorkBook(data);
    }

    private byte[] GenerateWorkBook<T>(IEnumerable<T> data)
    {
        using var wb = new XLWorkbook();

        DataTable dt = new DataTable(nameof(T));

        Type type = typeof(T);
        var propertyNames = type.GetProperties().Select(p => new DataColumn(p.Name)).ToArray();

        foreach (var item in propertyNames)
        {
            dt.Columns.Add(item);
        }

        foreach (var item in data)
        {
            DataRow row = dt.NewRow();
            foreach (var property in type.GetProperties())
            {
                row[property.Name] = property.GetValue(item);
            }
            dt.Rows.Add(row);
        }

        var worksheet = wb.AddWorksheet(dt);

        worksheet.Columns().AdjustToContents();

        using (MemoryStream stream = new MemoryStream())
        {
            wb.SaveAs(stream);
            stream.Position = 0;

            return stream.ToArray();
        }
    }
}

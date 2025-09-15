using Application.Interfaces.Services.Analitycs;
using ClosedXML.Excel;
using Domain.Interfaces;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using System.Data;

namespace Application.Services.Analitycs;
public class ExcelExportService : IExcelExportService
{
    private readonly IBaseRepository<WaterDeficiency> _waterDeficiencyRepository;
    private readonly IBaseRepository<SoilDeficiency> _soilDeficiencyRepository;
    private readonly IUserRepository _userRepository;

    public ExcelExportService(
        IBaseRepository<WaterDeficiency> waterDeficiencyRepository,
        IBaseRepository<SoilDeficiency> soilDeficiencyRepository,
        IUserRepository userRepository
        )
    {
        _waterDeficiencyRepository = waterDeficiencyRepository;
        _soilDeficiencyRepository = soilDeficiencyRepository;
        _userRepository = userRepository;
    }

    public async Task<byte[]> GenerateWaterDeficienciesTableAsync()
    {
        var data = await _waterDeficiencyRepository.GetAllAsync(-1);

        string[] excludeColumns = {
            "ChangedModelLog",
            "DeficiencyMonitoring",
            "CreatedBy"
        };

        return GenerateWorkBook(data, excludeColumns);
    }

    public async Task<byte[]> GenerateSoilDeficienciesTableAsync()
    {
        var data = await _soilDeficiencyRepository.GetAllAsync(-1);

        string[] excludeColumns = {
            "ChangedModelLog",
            "DeficiencyMonitoring",
            "CreatedBy"
        };

        return GenerateWorkBook(data, excludeColumns);
    }

    public async Task<byte[]> GenerateOrgUsersTableAsync()
    {
        var data = await _userRepository.GetAllAsync(-1);

        string[] excludeColumns = {
            "PasswordHash",
            "AccessToken",
            "RefreshToken",
            "EmailConfirmationToken",
            "AccessTokenExpireTime",
            "Password",
            "PasswordResetToken",
            "PasswordResetTokenExpiry",
            "DeficiencyMonitoringScheme",
            "RefreshTokenExpireTime",
            "CreatedBy"
        };

        return GenerateWorkBook(data, excludeColumns);
    }

    private byte[] GenerateWorkBook<T>(IEnumerable<T> data, string[] excludeColumns)
    {
        using var wb = new XLWorkbook();

        DataTable dt = new DataTable(nameof(T));

        Type type = typeof(T);
        var propertyNames = type.GetProperties()
            .Where(p => !p.Name.Contains("Id")
                && !excludeColumns.Contains(p.Name))
            .Select(p => new DataColumn(p.Name))
            .ToArray();

        foreach (var item in propertyNames)
        {
            dt.Columns.Add(item);
        }

        foreach (var item in data)
        {
            DataRow row = dt.NewRow();
            foreach (var property in type.GetProperties())
            {
                if (property.GetValue(item) == null
                    || property.Name.Contains("Id")
                    || excludeColumns.Contains(property.Name))
                {
                    continue;
                }

                if (property.Name == "Creator")
                {
                    User creator = (property.GetValue(item) as User);
                    row[property.Name] = $"{creator.LastName} {creator.FirstName}";
                }
                else if (property.Name == "ResponsibleUser")
                {
                    User responsibleUser = (property.GetValue(item) as User);
                    row[property.Name] = $"{responsibleUser.LastName} {responsibleUser.FirstName}";
                }
                else if (property.Name == "Organization")
                {
                    Domain.Models.Organization.Organization organization = (property.GetValue(item) as Domain.Models.Organization.Organization);
                    row[property.Name] = organization.Title;
                }
                else if (property.Name == "Laboratory")
                {
                    Laboratory lab = (property.GetValue(item) as Laboratory);
                    row[property.Name] = lab.Title;
                }
                else row[property.Name] = property.GetValue(item);
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

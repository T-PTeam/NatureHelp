using Application.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class MonitoringService : BaseService<DeficiencyMonitoring>, IBaseService<DeficiencyMonitoring>
{
    private readonly IMonitoringRepository _monitoringRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBaseService<SoilDeficiency> _soilDeficiencyService;
    private readonly IBaseService<WaterDeficiency> _waterDeficiencyService;

    public MonitoringService(
        IMonitoringRepository monitoringRepo,
        IUserRepository userRepo,
        IBaseService<SoilDeficiency> soilDeficiencyService,
        IBaseService<WaterDeficiency> waterDeficiencyService)
        : base(monitoringRepo)
    {
        _monitoringRepo = monitoringRepo;
        _userRepo = userRepo;
        _soilDeficiencyService = soilDeficiencyService;
        _waterDeficiencyService = waterDeficiencyService;
    }

    /// <summary>
    /// Toggles monitoring state for a single deficiency or all deficiencies of a given type.
    /// </summary>
    public async Task<bool> ToggleMonitoringAsync(string refreshToken, Guid? deficiencyId, EDeficiencyType type)
    {
        var user = await _userRepo.GetUserByRefreshTokenAsync(refreshToken);

        if (user is null) throw new ArgumentNullException("You are unauthorized. Please, login.", nameof(user));

        if (deficiencyId.HasValue)
        {
            Deficiency? deficiency = type.Equals(EDeficiencyType.Soil) ? await _soilDeficiencyService.GetByIdAsync((Guid)deficiencyId) : await _waterDeficiencyService.GetByIdAsync((Guid)deficiencyId);

            if (deficiency is null) throw new ArgumentNullException("Can not find deficiency...");

            var monitoring = (await _monitoringRepo
                .GetUserDeficiencyMonitoringAsync(user.Id, (Guid)deficiencyId, type));

            if (monitoring == null)
            {
                monitoring = new DeficiencyMonitoring
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    DeficiencyId = deficiencyId.Value,
                    DeficiencyType = type,
                    IsMonitoring = true
                };
                deficiency.DeficiencyMonitoring = monitoring;
                deficiency.DeficiencyMonitoringId = monitoring.Id;

                await _monitoringRepo.AddAsync(monitoring);
                if (type.Equals(EDeficiencyType.Soil)) await _soilDeficiencyService.UpdateAsync((SoilDeficiency)deficiency);
                else await _waterDeficiencyService.UpdateAsync((WaterDeficiency)deficiency);
                await _monitoringRepo.SaveChangesAsync();
                return true;
            }
            else
            {
                monitoring.IsMonitoring = !monitoring.IsMonitoring;
                deficiency.DeficiencyMonitoringId = monitoring.Id;
                deficiency.DeficiencyMonitoring = monitoring;
                if (type.Equals(EDeficiencyType.Soil)) await _soilDeficiencyService.UpdateAsync(deficiency as SoilDeficiency);
                else await _waterDeficiencyService.UpdateAsync(deficiency as WaterDeficiency);
                await _monitoringRepo.UpdateAsync(monitoring);
                await _monitoringRepo.SaveChangesAsync();
                return true;
            }
        }

        if (user.DeficiencyMonitoringScheme == null)
            user.DeficiencyMonitoringScheme = new ComplexMonitoringScheme();

        if (type == EDeficiencyType.Water)
            user.DeficiencyMonitoringScheme.isMonitoringWaterDeficiencies =
                !user.DeficiencyMonitoringScheme.isMonitoringWaterDeficiencies;
        else if (type == EDeficiencyType.Soil)
            user.DeficiencyMonitoringScheme.isMonitoringSoilDeficiencies =
                !user.DeficiencyMonitoringScheme.isMonitoringSoilDeficiencies;

        await _userRepo.UpdateAsync(user);

        return type switch
        {
            EDeficiencyType.Water => user.DeficiencyMonitoringScheme.isMonitoringWaterDeficiencies,
            EDeficiencyType.Soil => user.DeficiencyMonitoringScheme.isMonitoringSoilDeficiencies,
            _ => false
        };
    }
}

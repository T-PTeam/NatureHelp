using Microsoft.EntityFrameworkCore;

namespace Domain.Models.Audit;

[Owned]
public class ComplexMonitoringScheme
{
    public bool isMonitoringWaterDeficiencies { get; set; }
    public bool isMonitoringSoilDeficiencies { get; set; }
}

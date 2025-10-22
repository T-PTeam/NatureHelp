using Domain.Enums;

namespace Shared.Dtos;
public class DeficiencyMapDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public EDeficiencyType Type { get; set; }
    public EDangerState EDangerState { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double RadiusAffected { get; set; }
    public string CreatorFullName { get; set; } = string.Empty;
    public string ResponsibleUserFullName { get; set; } = string.Empty;
}

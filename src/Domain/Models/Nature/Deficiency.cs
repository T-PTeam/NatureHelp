using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Audit;
using Domain.Models.Organization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Nature;
public class Deficiency : BaseModel, ICoordinates
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public EDeficiencyType Type { get; set; }

    [ForeignKey(nameof(ResponsibleUser))]
    public Guid? ResponsibleUserId { get; set; }
    public User? ResponsibleUser { get; set; }
    public EDangerState EDangerState { get; set; } = EDangerState.Moderate;

    [ForeignKey(nameof(ChangedModelLog))]
    public Guid? ChangedModelLogEntityId { get; set; }
    public ChangedModelLog? ChangedModelLog { get; set; }

    [ForeignKey(nameof(Creator))]
    public Guid CreatedBy { get; set; }
    public User? Creator { get; set; }

    [ForeignKey(nameof(DeficiencyMonitoring))]
    public Guid? DeficiencyMonitoringId { get; set; }
    public DeficiencyMonitoring? DeficiencyMonitoring { get; set; }

    #region ICoordinates Implementation
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double RadiusAffected { get; set; } = 10;
    public string? Address { get; set; }
    #endregion
}

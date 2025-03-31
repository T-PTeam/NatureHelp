using Domain.Enums;
using Domain.Models.Organization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Nature;
public class Deficiency : BaseModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public EDeficiencyType Type { get; set; }

    [ForeignKey(nameof(ResponsibleUser))]
    public Guid? ResponsibleUserId { get; set; }
    public User? ResponsibleUser { get; set; }

    [ForeignKey(nameof(Location))]
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    public EDangerState EDangerState { get; set; } = EDangerState.Moderate;


    [ForeignKey(nameof(Creator))]
    public Guid CreatedBy { get; set; }
    public User? Creator { get; set; }
}

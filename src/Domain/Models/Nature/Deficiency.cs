using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Nature;
public class Deficiency : BaseModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public EDeficiencyType Type { get; set; }
    public User Creator { get; set; } = null!;
    public User? ResponsibleUser { get; set; }
    public Location Location { get; set; } = null!;
    public EDangerState EDangerState { get; set; } = EDangerState.Moderate;
}

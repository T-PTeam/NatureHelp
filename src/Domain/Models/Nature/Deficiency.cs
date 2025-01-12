using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Nature;
public class Deficiency : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
    public EDeficiencyType Type { get; set; }
    public User Creator { get; set; } = null!;
    public User? ResponsibleUser { get; set; }
    public List<Metric>? Metrics { get; set; }
    public Location Location { get; set; } = null!;
}

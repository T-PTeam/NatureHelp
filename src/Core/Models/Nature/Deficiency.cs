using Core.Enums;
using Core.Models.Organization;

namespace Core.Models.Nature;
public class Deficiency
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
    public EDeficiencyType Type { get; set; }
    public User Creator { get; set; } = null!;
    public User? ResponsibleUser { get; set; }
    public List<Metric>? Metrics { get; set; }
    public Location Location { get; set; } = null!;
}

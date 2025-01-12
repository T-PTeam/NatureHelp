using Domain.Enums;

namespace Domain.Models.Nature;
public class Metric : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public double? ThresholdMin { get; set; }
    public double? ThresholdMax { get; set; }
    public Deficiency Deficiency { get; set; } = null!;
    public EDangerState DangerState { get; set; }
}

namespace Domain.Models.Nature;
public class WaterDeficiency : Deficiency
{
    public int PopulationAffected { get; set; }
    public decimal EconomicImpact { get; set; }
    public string? HealthImpact { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public DateTime? ExpectedResolutionDate { get; set; }
    public string Caused { get; set; } = "Not known";
    public decimal WaterQualityLevel { get; set; }
}

namespace Domain.Models.Organization;
public class LaboratoryMapDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int ResearchersCount { get; set; }
    public List<ResearcherDto> Researchers { get; set; } = new();
}

public class ResearcherDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}

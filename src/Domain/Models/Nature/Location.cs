namespace Domain.Models.Nature;
public class Location : BaseEntity
{
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

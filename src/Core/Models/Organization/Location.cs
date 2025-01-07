namespace Core.Models.Organization;

public class Location
{
    public string Country { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string District { get; set; } = null!;
    public string City { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

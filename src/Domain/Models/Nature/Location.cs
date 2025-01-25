using Domain.Models.Location;

namespace Domain.Models.Nature;
public class Location : ACoordinates
{
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public double RadiusAffected { get; set; } = 10;
}

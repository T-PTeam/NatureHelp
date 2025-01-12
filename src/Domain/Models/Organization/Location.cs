using Domain.Models.Location;

namespace Domain.Models.Organization;

public class Location : ACoordinates
{
    public string Country { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string District { get; set; } = null!;
    public string City { get; set; } = null!;

    public string GetFormattedAddress()
    {
        return $"{Country}, {Region} region, {District} district, {City}";
    }
}

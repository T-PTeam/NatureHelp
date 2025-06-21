namespace Domain.Interfaces;
public interface ICoordinates
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public bool IsNearby(ICoordinates otherLocation, double radiusInKm)
    {
        // Earth's radius in kilometers
        const double EarthRadiusKm = 6371.0;

        // Convert latitude and longitude from degrees to radians
        double lat1 = Latitude * Math.PI / 180.0;
        double lon1 = Longitude * Math.PI / 180.0;
        double lat2 = otherLocation.Latitude * Math.PI / 180.0;
        double lon2 = otherLocation.Longitude * Math.PI / 180.0;

        // Calculate the differences between the latitudes and longitudes
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        // Apply the Haversine formula
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate the distance
        double distance = EarthRadiusKm * c;

        // Check if the distance is within the specified radius
        return distance <= radiusInKm;
    }

    public void UpdateCoordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

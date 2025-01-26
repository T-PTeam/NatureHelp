namespace Domain.Models.Location;
public abstract class ACoordinates : BaseModel
{
    private double latitude;
    private double longitude;

    public bool IsNearby(ACoordinates otherLocation, double radiusInKm)
    {
        // Earth's radius in kilometers
        const double EarthRadiusKm = 6371.0;

        // Convert latitude and longitude from degrees to radians
        double lat1 = this.latitude * Math.PI / 180.0;
        double lon1 = this.longitude * Math.PI / 180.0;
        double lat2 = otherLocation.latitude * Math.PI / 180.0;
        double lon2 = otherLocation.longitude * Math.PI / 180.0;

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
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

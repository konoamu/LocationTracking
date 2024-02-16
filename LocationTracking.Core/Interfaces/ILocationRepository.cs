using LocationTracking.Data.Entities;

namespace LocationTracking.Core.Interfaces
{
    public interface ILocationRepository
    {
        Location GetLatestLocationForUser(string userId);
        IEnumerable<Location> GetLocationHistoryForUser(string userId);
        void UpdateLocation(Location location);
        IEnumerable<Location?> GetLocationArea(string userId, double latitude, double longitude, double radius);
        IEnumerable<Location?> GetLocationArea(double latitude, double longitude, double radius);
        IEnumerable<Location> GetLatestLocations();

    }
}

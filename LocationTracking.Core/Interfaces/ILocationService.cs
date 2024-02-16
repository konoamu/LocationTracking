using LocationTracking.Data.Dto;

namespace LocationTracking.Core.Interfaces
{
    public interface ILocationService
    {
        LocationDto GetLatestLocationForUser(string userId);
        IEnumerable<LocationDto> GetLocationHistoryForUser(string userId);
        void UpdateLocation(string userId, LocationDto location);
        IEnumerable<LocationDto> GetLocationArea(string userId, double latitude, double longitude, double radius);
        IEnumerable<LocationDto> GetLocationArea(double latitude, double longitude, double radius);

        IEnumerable<LocationDto> GetLatestLocations();

    }
}

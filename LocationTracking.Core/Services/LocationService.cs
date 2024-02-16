using LocationTracking.Data.Dto;
using LocationTracking.Data.Entities;
using LocationTracking.Core.Interfaces;
using MongoDB.Driver.GeoJsonObjectModel;
using LocationTracking.Events.Event;

namespace LocationTracking.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public event EventHandler<LocationAddedEventArgs> LocationAdded;


        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public LocationDto GetLatestLocationForUser(string userId)
        {
            var location = _locationRepository.GetLatestLocationForUser(userId);
            return new LocationDto
            {
                Latitude = location.Coordinates.Coordinates.Y,
                Longitude = location.Coordinates.Coordinates.X,
                Timestamp = location.Timestamp
            };
        }

        public IEnumerable<LocationDto> GetLocationHistoryForUser(string userId)
        {
            var locations = _locationRepository.GetLocationHistoryForUser(userId);
            return locations.Select(l => new LocationDto
            {
                Latitude = l.Coordinates.Coordinates.Y,
                Longitude = l.Coordinates.Coordinates.X,
                Timestamp = l.Timestamp
            });
        }

        public void UpdateLocation(string userId, LocationDto location)
        {
            var entity = new Location
            {
                UserId = userId,
                Coordinates = GeoJson.Point(GeoJson.Position(location.Longitude, location.Latitude)),
                Timestamp = location.Timestamp
            };

            _locationRepository.UpdateLocation(entity);

            LocationAdded?.Invoke(this, new LocationAddedEventArgs
            {
                UserId = userId,
                Location = location,
                Timestamp = DateTime.UtcNow
            });
        }

        public IEnumerable<LocationDto> GetLocationArea(string userId, double latitude, double longitude, double radius)
        {
            var locations = _locationRepository.GetLocationArea(userId, latitude, longitude, ToRadians(radius));
            return locations.Select(l => new LocationDto
            {
                Latitude = l.Coordinates.Coordinates.Y,
                Longitude = l.Coordinates.Coordinates.X,
                Timestamp = l.Timestamp
            });
        }

        public IEnumerable<LocationDto> GetLocationArea(double latitude, double longitude, double radius)
        {
            var locations = _locationRepository.GetLocationArea(latitude, longitude, ToRadians(radius));
            return locations.Select(l => new LocationDto
            {
                Latitude = l.Coordinates.Coordinates.Y,
                Longitude = l.Coordinates.Coordinates.X,
                Timestamp = l.Timestamp
            });
        }

        public IEnumerable<LocationDto> GetLatestLocations()
        {
            var locations = _locationRepository.GetLatestLocations();
            return locations.Select(l => new LocationDto
            {
                Latitude = l.Coordinates.Coordinates.Y,
                Longitude = l.Coordinates.Coordinates.X,
                Timestamp = l.Timestamp
            });

        }

        public double ToRadians(double radiusInMeters)
        {
            // Convert radius from meters to radians
            double earthRadiusInMeters = 6371000; // Earth's radius in meters
            return radiusInMeters / earthRadiusInMeters;
        }
    }
}

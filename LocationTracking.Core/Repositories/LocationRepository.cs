using LocationTracking.Data.Entities;
using LocationTracking.Core.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace LocationTracking.Core.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IMongoCollection<Location> _locationCollection;

        public LocationRepository(IMongoDatabase database)
        {
            _locationCollection = database.GetCollection<Location>("Locations");

            // Create a 2d sphere index on the Coordinates field
            var indexKeysDefinition = Builders<Location>.IndexKeys.Geo2DSphere(x => x.Coordinates);
            var createIndexModel = new CreateIndexModel<Location>(indexKeysDefinition);
            _locationCollection.Indexes.CreateOne(createIndexModel);

            // Create a unique index on the _id field
            var idIndexKeysDefinition = Builders<Location>.IndexKeys.Ascending(x => x.Id);
            var idIndexOptions = new CreateIndexOptions();
            var idCreateIndexModel = new CreateIndexModel<Location>(idIndexKeysDefinition, idIndexOptions);
            _locationCollection.Indexes.CreateOne(idCreateIndexModel);

        }

        public Location GetLatestLocationForUser(string userId)
        {
            return _locationCollection.Find(l => l.UserId == userId).SortByDescending(l => l.Timestamp).FirstOrDefault();
        }

        public IEnumerable<Location> GetLocationHistoryForUser(string userId)
        {
            return _locationCollection.Find(l => l.UserId == userId).SortByDescending(l => l.Timestamp).ToList();
        }

        public void UpdateLocation(Location location)
        {
            _locationCollection.InsertOne(location);
        }

        public IEnumerable<Location?> GetLocationArea(string userId, double latitude, double longitude, double maxDistance)
        {
            var builder = Builders<Location>.Filter;

            var userIdFilter = builder.Eq(x => x.UserId, userId);
            var point = GeoJson.Point(GeoJson.Position(longitude,latitude));
            var locationFilter = builder.GeoWithinCenterSphere(x => x.Coordinates, longitude, latitude, maxDistance);

            var combinedFilter = builder.And(userIdFilter, locationFilter);

            return _locationCollection.Find(combinedFilter).ToList();

        }

        public IEnumerable<Location?> GetLocationArea(double latitude, double longitude, double maxDistance)
        {
            var builder = Builders<Location>.Filter;
            var point = GeoJson.Point(GeoJson.Position(latitude, longitude));
            var locationFilter = builder.GeoWithinCenterSphere(x => x.Coordinates, longitude, latitude, maxDistance);

            return _locationCollection.Find(locationFilter).ToList();
        }

        public IEnumerable<Location> GetLatestLocations()
        {
            return _locationCollection.Aggregate()
                .SortByDescending(l => l.Timestamp)
                .Group(l => l.UserId, g => new Location
                {
                    UserId = g.Key,
                    Coordinates = g.First().Coordinates,
                    Timestamp = g.First().Timestamp
                })
                .ToList();
        }
    }
}

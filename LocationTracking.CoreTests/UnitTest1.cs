using LocationTracking.Core.Interfaces;
using LocationTracking.Core.Services;
using LocationTracking.Data.Dto;
using LocationTracking.Data.Entities;
using MongoDB.Driver.GeoJsonObjectModel;
using Moq;

namespace LocationTracking.CoreTests
{
    [TestFixture]
    public class LocationServiceTests
    {
        private LocationService _locationService;
        private Mock<ILocationRepository> _locationRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _locationRepositoryMock = new Mock<ILocationRepository>();
            _locationService = new LocationService(_locationRepositoryMock.Object);
        }

        /// <summary>
        /// Let's test if the GetLatestLocationForUser method returns a corret LocationDto with latitude and longitude in the correct place.
        /// </summary>
        [Test]
        public void GetLatestLocationForUser_Returns_LocationDto()
        {
            // Arrange
            string userId = "testUserId";
            var latitude = 1.0;
            var longitude = 2.0;

            var locationEntity = new Location
            {
                UserId = userId,
                Coordinates = GeoJson.Point(GeoJson.Position(longitude, latitude)),
                Timestamp = DateTime.UtcNow
            };
            _locationRepositoryMock.Setup(repo => repo.GetLatestLocationForUser(userId)).Returns(locationEntity);

            // Act
            var result = _locationService.GetLatestLocationForUser(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Latitude, Is.EqualTo(latitude));
            Assert.Multiple(() =>
            {
                Assert.That(result.Longitude, Is.EqualTo(longitude));
                Assert.That(result.Timestamp, Is.EqualTo(locationEntity.Timestamp));
            });
        }

        /// <summary>
        /// Let's test if the UpdateLocation method calls the repository.
        /// </summary>
        [Test]
        public void UpdateLocation_Calls_Repository()
        {
            // Arrange
            string userId = "testUserId";
            var locationDto = new LocationDto
            {
                Latitude = 1.0,
                Longitude = 2.0,
                Timestamp = DateTime.UtcNow
            };

            // Act
            _locationService.UpdateLocation(userId, locationDto);

            // Assert
            _locationRepositoryMock.Verify(repo => repo.UpdateLocation(It.IsAny<Location>()), Times.Once);
            _locationRepositoryMock.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Let's test if the UpdateLocation method raises the LocationAdded event.
        /// </summary>
        [Test]
        public void UpdateLocation_Raises_LocationAdded_Event()
        {
            // Arrange
            string userId = "testUserId";
            var locationDto = new LocationDto
            {
                Latitude = 1.0,
                Longitude = 2.0,
                Timestamp = DateTime.UtcNow
            };
            bool eventRaised = false;
            _locationService.LocationAdded += (sender, args) => eventRaised = true;

            // Act
            _locationService.UpdateLocation(userId, locationDto);

            // Assert
            Assert.That(eventRaised, Is.True);
        }
    }
}
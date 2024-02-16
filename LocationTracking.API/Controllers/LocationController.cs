using Microsoft.AspNetCore.Mvc;
using LocationTracking.Core.Interfaces;
using LocationTracking.Data.Dto;


namespace LocationTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationService _locationService;

        public LocationController(ILogger<LocationController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        }

        /// <summary>
        /// Adds a new location for a user.
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <param name="location">The location to add.</param>
        /// <returns>The added location.</returns>
        [HttpPost("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult AddLocation(string userId, [FromBody] LocationDto location)
        {
            // Validate the location
            if (location == null || location.Latitude < -90 || location.Latitude > 90 || location.Longitude < -180 || location.Longitude > 180)
            {
                return BadRequest("Invalid location");
            }

            try
            {
                _locationService.UpdateLocation(userId, location);
                _logger.LogInformation("Location added for user {UserId}. Latitude: {Latitude}, Longitude: {Longitude}", userId, location.Latitude, location.Longitude);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding location for user {UserId}. Latitude: {Latitude}, Longitude: {Longitude}", userId, location.Latitude, location.Longitude);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the latest location for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The latest location of the user.</returns>
        [HttpGet("{userId}/latest")]
        [ProducesResponseType(typeof(LocationDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLatestLocation(string userId)
        {
            try
            {
                var latestLocation = _locationService.GetLatestLocationForUser(userId);
                if (latestLocation != null)
                {
                    _logger.LogInformation("Latest location retrieved for user {UserId}. Latitude: {Latitude}, Longitude: {Longitude}", userId, latestLocation.Latitude, latestLocation.Longitude);
                    return Ok(latestLocation);
                }
                else
                {
                    _logger.LogWarning("Latest location not found for user {UserId}", userId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest location for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get the latest locations for all users.
        /// </summary>
        /// <returns> The latest user locations</returns>
        [HttpGet("latest")]
        [ProducesResponseType(typeof(IEnumerable<LocationDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLatestUserLocations()
        {
            return Ok(_locationService.GetLatestLocations());
        }

        /// <summary>
        /// Retrieves the location history for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The location history of the user.</returns>
        [HttpGet("{userId}/history")]
        [ProducesResponseType(typeof(IEnumerable<LocationDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLocationHistory(string userId)
        {
            try
            {
                var locationHistory = _locationService.GetLocationHistoryForUser(userId);
                if (locationHistory.Any())
                {
                    _logger.LogInformation("Location history retrieved for user {UserId}", userId);
                    return Ok(locationHistory);
                }
                else
                {
                    _logger.LogWarning("Location history not found for user {UserId}", userId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving location history for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the location area for a user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="latitude">The center latitude.</param>
        /// <param name="longitude">The center longitude.</param>
        /// <param name="radius">How far to check in meters.</param>
        /// <returns></returns>
        [HttpGet("area")]
        [ProducesResponseType(typeof(IEnumerable<LocationDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLocationArea(string userId, double latitude, double longitude, double radius)
        {
            try
            {
                var locationArea = _locationService.GetLocationArea(userId, latitude, longitude, radius);
                if (locationArea.Any())
                {
                    _logger.LogInformation("Location area retrieved for user {UserId}", userId);
                    return Ok(locationArea);
                }
                else
                {
                    _logger.LogWarning("Location area not found for user {UserId}", userId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving location area for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

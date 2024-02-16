using LocationTracking.Data.Dto;

namespace LocationTracking.Events.Event
{
    public class LocationAddedEventArgs : EventArgs
    {
        public string UserId { get; set; }
        public LocationDto Location { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

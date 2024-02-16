using LocationTracking.Events.Event;

namespace LocationTracking.Events.Interfaces
{
    public interface ILocationAddedEventHandler
    {
        void OnLocationAdded(object sender, LocationAddedEventArgs args);
    }
}

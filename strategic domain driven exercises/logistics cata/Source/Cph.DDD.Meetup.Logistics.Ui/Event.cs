using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Ui;

public class Event
{
    public string EventName { get; set; }
    public string EventData { get; set; }
    public DateTime Timestamp { get; set; }
}

public static class PubSubEvent
{
    private static readonly List<Action<IEvent>> _subscribers = new();

    public static void Publish(IEvent eventData)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber(eventData);
        }
    }

    public static void Subscribe(Action<IEvent> action)
    {
        _subscribers.Add(action);
    }
}

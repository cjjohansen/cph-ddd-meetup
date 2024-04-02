using Marten;
using Marten.Events;
using Marten.Events.Projections;

namespace Cph.DDD.Meetup.Logistics.Ui;

public class PubSubProjection : IProjection
{
    public void Apply(IDocumentOperations operations, IReadOnlyList<StreamAction> streams)
    {
        throw new NotImplementedException();
    }

    public async Task ApplyAsync(IDocumentOperations operations, IReadOnlyList<StreamAction> streams, CancellationToken cancellation)
    {
        foreach (var @event in streams.SelectMany(p => p.Events))
        {
            if (@event.Data is Cph.DDD.Meetup.Logistics.Domain.Common.IEvent @eventData)
                PubSubEvent.Publish(eventData);
        }

        await Task.CompletedTask;
    }
}

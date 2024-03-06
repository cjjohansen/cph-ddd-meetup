using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportTycoon;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;


// events

public record PlacedAt( Place Place, DateTime Date ) : IEvent;

// commands

public record PlaceAt( Place Place, DateTime Date ) : ICommand;

public record ContainerState( Place Destination, Place CurrentLocation )
{
    public override string ToString() => this.IsAtDestination() ? "At destination" : $"Destined for {this.Destination.Name}, currently at {this.CurrentLocation.Name}";
}

public static class ContainerDecider
{
    // handle state

    private static ContainerState Evolve( ContainerState state, IEvent @event ) =>
        @event switch
        {
            PlacedAt placedAt => state with { CurrentLocation = placedAt.Place },
            _ => state
        };

    public static ContainerState Fold( this IEnumerable<IEvent> history, ContainerState state ) =>
        history.Aggregate( state, Evolve );

    public static bool IsAtDestination( this ContainerState state ) => state.CurrentLocation == state.Destination;


    // handle commands
    public static Events Decide( this ContainerState state, ICommand command ) =>
        command switch
        {
            PlaceAt c => PlaceAt( c ),
            _ => throw new NotImplementedException()
        };

    private static Events PlaceAt( PlaceAt c ) =>
        new PlacedAt( c.Place , c.Date ).Singleton();
}
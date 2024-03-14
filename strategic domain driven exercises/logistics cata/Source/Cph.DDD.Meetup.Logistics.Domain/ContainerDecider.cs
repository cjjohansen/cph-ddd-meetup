using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportTycoon;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;

public record ContainerId( int Id );


// events

public record PlacedAt( PlaceState Place, DateTime Date ) : IEvent;

// commands

public record PlaceAt( PlaceState Place, DateTime Date ) : ICommand;

public record ContainerState(ContainerId Id, PlaceState Destination, PlaceState CurrentLocation )
{
    public override string ToString() => this.IsAtDestination() ? $"Id {Id}, At destination" : $"Id {Id}, Destined for {this.Destination.Name}, currently at {this.CurrentLocation.Name}";

    public static readonly ContainerState Initial = new( new ContainerId(0), PlaceState.Initial, PlaceState.Initial);
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
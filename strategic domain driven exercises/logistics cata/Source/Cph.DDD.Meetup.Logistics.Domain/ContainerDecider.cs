using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;

public record ContainerId( Guid Id );
public record DestinationCode( string Code);


// events

public record ContainerPlacedAt( ContainerStoreId ContainerStoreId, ContainerId ContainerId, DestinationCode Destination, DateTime timeStamp ) : IEvent;

// commands

public record PlaceContainerAt( ContainerId ContainerId, ContainerStoreId ContainerStoreId, DestinationCode Destination, IClock Clock ) : ICommand;

public record ContainerState(ContainerId Id, ContainerStoreId Destination, ContainerStoreId CurrentLocation )
{
    public override string ToString() => this.IsAtDestination() ? $"Id {Id}, At destination" : $"Id {Id}, Destined for {this.Destination.Id}, currently at {this.CurrentLocation.Id}";

    public static readonly ContainerState Initial = new( new ContainerId(Guid.Empty), new ContainerStoreId(Guid.Empty), new ContainerStoreId(Guid.Empty));
}

public static class ContainerDecider
{
    // handle state

    private static ContainerState Evolve( ContainerState state, IEvent @event ) =>
        @event switch
        {
            ContainerPlacedAt placedAt => state with { CurrentLocation = placedAt.ContainerStoreId },
            _ => state
        };

    public static ContainerState Fold( this IEnumerable<IEvent> history, ContainerState state ) =>
        history.Aggregate( state, Evolve );

    public static bool IsAtDestination( this ContainerState state ) => state.CurrentLocation == state.Destination;


    // handle commands
    public static Events Decide( this ContainerState state, ICommand command ) =>
        command switch
        {
            PlaceContainerAt c => PlaceAt( c ),
            _ => throw new NotImplementedException()
        };

    private static Events PlaceAt( PlaceContainerAt c ) =>
        new ContainerPlacedAt( c.ContainerStoreId, c.ContainerId, c.Destination, c.Clock.Time ).Singleton();
}
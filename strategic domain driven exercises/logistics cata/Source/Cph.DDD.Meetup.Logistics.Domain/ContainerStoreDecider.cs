using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;

public record ContainerStoreId(Guid Id);

// events

public record ContainerLoadedFrom( ContainerState Container, DateTime Date ) : IEvent;
public record ContainerUnloadedAt( ContainerStoreId CurrentLocationId, ContainerId ContainerId, ContainerStoreId DestinationId, DateTime Date ) : IEvent;
public record ContainerStoreInitialized( ContainerStoreId Id, FreightLocationId FreightLocationId, string Name, ISet<ContainerState> Containers ) : IEvent;


// commands
public record Initialize(ContainerStoreId Id, FreightLocationId FreightLocationId, string Name, ISet<ContainerState> Containers ) : ICommand;
public record LoadContainerFrom(DateTime Date ) : ICommand;
public record UnloadContainerAt( ContainerState Container, DateTime Date ) : ICommand;

public record ContainerStore(ContainerStoreId Id, FreightLocation Location, ISet<ContainerState> Containers)
{
    public static readonly ContainerStore Initial = new(new ContainerStoreId(Guid.Empty), FreightLocation.EmptyFreightLocation , new HashSet<ContainerState>() );
}

public static class ContainerStoreDecider
{

    // handle state

    private static ContainerStore Evolve( ContainerStore state, IEvent @event )
    {
        switch ( @event )
        {
            case ContainerStoreInitialized initialized:
                {
                    return state with {Id = initialized.Id, Location = new FreightLocation( initialized.FreightLocationId, initialized.Name),  Containers = initialized.Containers };
                }

            case ContainerLoadedFrom containerLoaded:
                {
                    var containers = state.Containers;
                    containers.Remove( containerLoaded.Container );
                    return state with { Containers = containers };
                }
            case ContainerUnloadedAt containerUnloaded:
                {
                    var containers = state.Containers;

                    containers.Add( new ContainerState( containerUnloaded.ContainerId,containerUnloaded.DestinationId, state.Id ) );

                    return state with { Containers = containers };
                }
            default:
                return state;
        };
    }

    public static ContainerStore Fold( this IEnumerable<IEvent> history, ContainerStore state ) =>
        history.Aggregate( state, Evolve );

    public static ContainerStore Fold( this IEnumerable<IEvent> history ) =>
        history.Fold( ContainerStore.Initial );


    // handle commands

    public static Events Decide( this ContainerStore state, ICommand command ) =>
        command switch
        {
            Initialize c => Initialize( c ),
            LoadContainerFrom c => LoadContainerFrom( state, c ),
            UnloadContainerAt c => UnloadContainerAt( state, c ),

            _ => throw new NotImplementedException()
        };


    private static Events Initialize(Initialize c )
    {
        var events = new List<IEvent>();

        var (id, locationId, name, containers) = c;

        if(id is null || name is null || containers is null)
            return events;

        return new ContainerStoreInitialized( id, locationId, name, containers ).Singleton();
    }

    private static Events LoadContainerFrom(ContainerStore state, LoadContainerFrom c )
    {
        if ( state.Containers.Count == 0 )
            return new ReadOnlyCollection<IEvent>( new List<IEvent>() );

        var container = state.Containers.GetEnumerator().Current;
       
        return new ContainerLoadedFrom(container, c.Date  ).Singleton();
    }

    private static Events UnloadContainerAt( ContainerStore state, UnloadContainerAt c )
    {
         return new ContainerUnloadedAt(state.Id, c.Container.Id, c.Container.Destination, c.Date ).Singleton();
    }
  
}
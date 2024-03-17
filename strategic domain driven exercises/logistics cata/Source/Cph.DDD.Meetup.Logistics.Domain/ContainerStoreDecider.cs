using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Domain;

using static System.Runtime.InteropServices.JavaScript.JSType;
using Events = IReadOnlyCollection<IEvent>;

public record ContainerStoreId(Guid Id);

// events
public record ContainerLoaded( ContainerState Container, DateTime Date ) : IEvent;
public record ContainerUnloaded( ContainerStore ContainerStore, ContainerState Container, DateTime Date ) : IEvent;
public record ContainerStoreInitialized( ContainerStoreId Id, FreightLocationId FreightLocationId, string Name, ISet<ContainerState> Containers ) : IEvent;


// commands
public record Initialize(ContainerStoreId Id, FreightLocationId FreightLocationId, string Name, ISet<ContainerState> Containers ) : ICommand;
public record LoadContainer(DateTime Date ) : ICommand;
public record UnloadContainer( ContainerState Container, DateTime Date ) : ICommand;

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

            case ContainerLoaded containerLoaded:
                {
                    var containers = state.Containers;
                    containers.Add( containerLoaded.Container );
                    return state with { Containers = containers };
                }
            case ContainerUnloaded containerUnloaded:
                {
                    var containers = state.Containers;
                    containers.Remove( containerUnloaded.Container );
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
            LoadContainer c => LoadContainer( state, c ),
            UnloadContainer c => UnloadContainer( state, c ),

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

    private static Events LoadContainer(ContainerStore state, LoadContainer c )
    {
        if ( state.Containers.Count == 0 )
            return new ReadOnlyCollection<IEvent>( new List<IEvent>() );

        var container = state.Containers.GetEnumerator().Current;
       
        return new ContainerLoaded(container, c.Date  ).Singleton();
    }

    private static Events UnloadContainer( ContainerStore state, UnloadContainer c )
    {
         return new ContainerUnloaded(state, c.Container, c.Date ).Singleton();
    }
  
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportTycoon;

namespace Cph.DDD.Meetup.Logistics.Domain;

using static System.Runtime.InteropServices.JavaScript.JSType;
using Events = IReadOnlyCollection<IEvent>;

public record PlaceId(uint Id);

// events
public record ContainerLoaded( ContainerState Container, DateTime Date ) : IEvent;
public record ContainerUnloaded( PlaceState Place, ContainerState Container, DateTime Date ) : IEvent;
public record PlaceInitialized(PlaceId Id, string Name, List<ContainerState> Containers, IRoutingSource RoutingSource ) : IEvent;


// commands
public record Initialize(PlaceId Id, string Name, List<ContainerState> Containers, IRoutingSource RoutingSource ) : ICommand;
public record LoadContainer(DateTime Date ) : ICommand;
public record UnloadContainer( ContainerState Container, DateTime Date ) : ICommand;

public record PlaceState(PlaceId Id,  string Name, List<ContainerState> Containers, IRoutingSource RoutingSource )
{
    public static readonly PlaceState Initial = new(new PlaceId(0), "Not Initialized", new List<ContainerState>(), new EmptyRoutingSource() );
}

public static class PlaceDecider
{

    // handle state

    private static PlaceState Evolve( PlaceState state, IEvent @event )
    {
        switch ( @event )
        {
            case PlaceInitialized placeInitialized:
                {
                    return state with {Id = placeInitialized.Id, Name = placeInitialized.Name,  Containers = placeInitialized.Containers, RoutingSource = placeInitialized.RoutingSource };
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

    public static PlaceState Fold( this IEnumerable<IEvent> history, PlaceState state ) =>
        history.Aggregate( state, Evolve );

    public static PlaceState Fold( this IEnumerable<IEvent> history ) =>
        history.Fold( PlaceState.Initial );


    // handle commands

    public static Events Decide( this PlaceState state, ICommand command ) =>
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

        var (id, name, containers, routingSource ) = c;

        if(id is null || name is null || containers is null || routingSource is null)
            return events;

        return new PlaceInitialized( id, name, containers, routingSource).Singleton();
    }

    private static Events LoadContainer(PlaceState state, LoadContainer c )
    {
        if ( state.Containers.Count == 0 )
            return new ReadOnlyCollection<IEvent>( new List<IEvent>() );

        var container = state.Containers[ 0 ];
       
        return new ContainerLoaded(container, c.Date  ).Singleton();
    }

    private static Events UnloadContainer( PlaceState state, UnloadContainer c )
    {
         return new ContainerUnloaded(state, c.Container, c.Date ).Singleton();
    }
  
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cph.DDD.Meetup.Logistics.Domain.Common;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;


public interface ITruckEvent: IEvent
{

}

public record TruckId(int Id);

// events
public record Loaded(Container Container, ContainerStoreId PlaceId, DateTime Date ) : ITruckEvent;
public record UnLoaded( ContainerId ContainerId, ContainerStoreId PlaceId, DateTime Date ) : ITruckEvent;
public record Assigned(BookingId BookingId, DateTime Date ) : ITruckEvent;
public record Vacant( DateTime Date ) : ITruckEvent;
public record Arrived(ContainerStore Location, DateTime Date ) : ITruckEvent;
public record Left( ContainerStore Location, DateTime Date ) : ITruckEvent;

// commands
public record Load( Container Container, ContainerStoreId PlaceId, DateTime Date ) : ICommand;
public record UnLoad( ContainerId ContainerId, ContainerStoreId PlaceId, DateTime Date ) : ICommand;
public record Assign(BookingId BookingId) : ICommand;
public record MarkAsVacant( DateTime Date ) : ICommand;
public record Arrive(ContainerStore Location, DateTime Date ) : ICommand;
public record Leave( ContainerStore Location, DateTime Date ) : ICommand;

public record TruckState( TruckId Id, BookingId? BookingId, Container? Container, ContainerStore Destination, ContainerStore? CurrentLocation, bool IsVacant )
{
    public static readonly TruckState Initial = new( new TruckId( 0 ), null, null, ContainerStore.Initial, ContainerStore.Initial, false );
}
    

public static class TruckDecider
{
    // handle state

    private static TruckState Evolve( TruckState state, IEvent @event ) =>
        @event switch
        {
            Assigned assigned => state with { BookingId = assigned.BookingId, IsVacant = false },
            Arrived arrived => state with {  CurrentLocation = arrived.Location },
            Left left => state with { CurrentLocation = null },
            Loaded loaded => state with { Container = loaded.Container },
            UnLoaded unLoaded => state with { Container = null },
            Vacant vacant => state with { IsVacant = true},
            _ => state
        };

    public static TruckState Fold( this IEnumerable<ITruckEvent> history, TruckState state ) =>
        history.Aggregate( state, Evolve );

    public static TruckState Fold( this IEnumerable<ITruckEvent> history ) =>
        history.Fold( TruckState.Initial );

    // handle commands

    public static Events Decide( this TruckState state, ICommand command, Clock clock ) =>
        command switch
        {
            Assign c => Assign( c, clock ),
            Arrive c => Arrive(c, clock),
            Leave c => Leave(c, clock),
            Load c => Load( c, clock ),
            UnLoad c => UnLoad( c, clock ),
            MarkAsVacant c => MarkAsVacant(c,clock),
            _ => throw new NotImplementedException()
        };

    private static Events Assign( Assign c, Clock clock ) =>
        new Assigned( c.BookingId, clock.Time ).Singleton();

    private static Events Arrive( Arrive c, Clock clock ) =>
        new Arrived(  c.Location, clock.Time ).Singleton();

    private static Events Leave( Leave c, Clock clock ) =>
        new Left( c.Location, clock.Time ).Singleton();

    private static Events Load( Load c, Clock clock ) =>
        new Loaded( c.Container, c.PlaceId, clock.Time ).Singleton();

    private static Events UnLoad( UnLoad c, Clock clock ) =>
        new UnLoaded( c.ContainerId, c.PlaceId, clock.Time ).Singleton();

    private static Events MarkAsVacant( MarkAsVacant c, Clock clock ) =>
       new Vacant(clock.Time ).Singleton();
 
}
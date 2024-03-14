using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;


public record TruckId(int Id);

// events
public record Loaded(Container Container, PlaceId PlaceId, DateTime Date ) : IEvent;
public record UnLoaded( ContainerId ContainerId, PlaceId PlaceId, DateTime Date ) : IEvent;
public record Assigned(BookingId BookingId, DateTime Date ) : IEvent;
public record Vacant( DateTime Date ) : IEvent;
public record Arrived(PlaceState Location, DateTime Date ) : IEvent;
public record Left( PlaceState Location, DateTime Date ) : IEvent;

// commands
public record Load( Container Container, PlaceId PlaceId, DateTime Date ) : ICommand;
public record UnLoad( ContainerId ContainerId, PlaceId PlaceId, DateTime Date ) : ICommand;
public record Assign(BookingId BookingId) : ICommand;
public record MarkAsVacant( DateTime Date ) : ICommand;
public record Arrive(PlaceState Location, DateTime Date ) : ICommand;
public record Leave( PlaceState Location, DateTime Date ) : ICommand;

public record TruckState( TruckId Id, BookingId? BookingId, Container? Container, PlaceState Destination, PlaceState? CurrentLocation, bool IsVacant )
{
    public static readonly TruckState Initial = new( new TruckId( 0 ), null, null, PlaceState.Initial, PlaceState.Initial, false );
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

    public static TruckState Fold( this IEnumerable<IEvent> history, TruckState state ) =>
        history.Aggregate( state, Evolve );

    public static TruckState Fold( this IEnumerable<IEvent> history ) =>
        history.Fold( TruckState.Initial );

    // handle commands

    public static Events Decide( this TruckState state, ICommand command, IClock clock ) =>
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

    private static Events Assign( Assign c, IClock clock ) =>
        new Assigned( c.BookingId, clock.Time ).Singleton();

    private static Events Arrive( Arrive c, IClock clock ) =>
        new Arrived(  c.Location, clock.Time ).Singleton();

    private static Events Leave( Leave c, IClock clock ) =>
        new Left( c.Location, clock.Time ).Singleton();

    private static Events Load( Load c, IClock clock ) =>
        new Loaded( c.Container, c.PlaceId, clock.Time ).Singleton();

    private static Events UnLoad( UnLoad c, IClock clock ) =>
        new UnLoaded( c.ContainerId, c.PlaceId, clock.Time ).Singleton();

    private static Events MarkAsVacant( MarkAsVacant c, IClock clock ) =>
       new Vacant(clock.Time ).Singleton();
 
}
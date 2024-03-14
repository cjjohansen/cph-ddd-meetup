using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportTycoon;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;

public record BookingId( int Id );


// events

public record Accepted(BookingId Id, ContainerId ContainerId, DateTime Date ) : IEvent;

// commands

public record Accept(BookingId Id, ContainerId ContainerId, DateTime Date ) : ICommand;





public record BookingState( BookingId Id, ContainerId ContainerId, DateTime AcceptedDate )
{
    public override string ToString() => $"Id {Id}, ConainerId {this.ContainerId},  AcceptedAt: {this.AcceptedDate }";
}

public static class BookingDecider
{
    // handle state

    private static BookingState Evolve( BookingState state, IEvent @event ) =>
        @event switch
        {
            Accepted accepted => state with { Id = accepted.Id, ContainerId = accepted.ContainerId, AcceptedDate = accepted.Date},
            _ => state
        };

    public static BookingState Fold( this IEnumerable<IEvent> history, BookingState state ) =>
        history.Aggregate( state, Evolve );

    public static bool IsAccepted( this BookingState state ) => true;


    // handle commands
    public static Events Decide( this BookingState state, ICommand command ) =>
        command switch
        {
            Accept c => Accept( c ),
            _ => throw new NotImplementedException()
        };

    private static Events Accept( Accept c ) =>
        new Accepted( c.Id, c.ContainerId, c.Date ).Singleton();
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain;

using Events = IReadOnlyCollection<IEvent>;

public record Transaction( decimal Amount, DateTime Date );

// events
public interface IEvent { } // used to mimic a discriminated union
public record Deposited( Transaction Transaction ) : IEvent;
public record Withdrawn( Transaction Transaction ) : IEvent;
public record Closed( DateTime Date ) : IEvent;

// commands
public interface ICommand { }
public record Deposit( decimal Amount, DateTime Date ) : ICommand;
public record Withdraw( decimal Amount, DateTime Date ) : ICommand;
public record Close( DateTime Date ) : ICommand;

public record State( decimal Balance, bool IsClosed )
{
    public static readonly State Initial = new( 0, false );
}

public static class Decider
{
    // handle state

    private static State Evolve( State state, IEvent @event ) =>
        @event switch
        {
            Deposited deposited => state with { Balance = state.Balance + deposited.Transaction.Amount },
            Withdrawn withdrawn => state with { Balance = state.Balance - withdrawn.Transaction.Amount },
            Closed _ => state with { IsClosed = true },
            _ => state
        };

    public static State Fold( this IEnumerable<IEvent> history, State state ) =>
        history.Aggregate( state, Evolve );

    public static State Fold( this IEnumerable<IEvent> history ) =>
        history.Fold( State.Initial );

    public static bool IsTerminal( this State state ) => state.IsClosed;

    // handle commands

    public static Events Decide( this State state, ICommand command ) =>
        command switch
        {
            Deposit c => Deposit( c ),
            Withdraw c => Withdraw( c ),
            Close c => Close( state, c ),
            _ => throw new NotImplementedException()
        };

    private static Events Deposit( Deposit c ) =>
        new Deposited( new( c.Amount, c.Date ) ).Singleton();

    private static Events Withdraw( Withdraw c ) =>
        new Withdrawn( new( c.Amount, c.Date ) ).Singleton();

    private static Events Close( State state, Close c )
    {
        var events = new List<IEvent>();
        if ( state.Balance > 0 )
            events.Add( new Withdrawn( new( state.Balance, c.Date ) ) );
        events.Add( new Closed( c.Date ) );
        return events;
    }

    // helpers

    public static Events Singleton( this IEvent e ) => new IEvent[ 1 ] { e };
}
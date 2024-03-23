using Cph.DDD.Meetup.Logistics.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain
{

    using Events = IReadOnlyCollection<IEvent>;


    



    //Events 
    public interface ITimeEvent : IEvent { };
    public record TimePassed(TimeSpan Time):ITimeEvent;

    // Command
    public interface ITimeCommand : ICommand { };
    public record PassTime( TimeSpan Time ):ITimeCommand;

    // State
    public record TotalTime( TimeSpan Time )
    {
        public static TotalTime Initial => new TotalTime(TimeSpan.Zero);
    };


    public static class TimeDecider
    {

        private static TotalTime Evolve(TotalTime timeState, IEvent @event)
        {
            switch ( @event )
            {
                case TimePassed timePassed:
                    {
                        return timeState with { Time = timeState.Time + timePassed.Time };
                    }

                default:
                    return timeState;
            };
        }
      
        public static TotalTime Fold(this IEnumerable<IEvent> history, TotalTime state ) =>
            history.Aggregate( state, Evolve );

        public static TotalTime Fold(this IEnumerable<IEvent> history ) =>
            history.Fold( TotalTime.Initial );

        public static Events Decide( this TotalTime totalTime, ICommand command ) =>
         command switch
         {
             PassTime c => PassTime( c ),
             _ => throw new NotImplementedException()
         };

        private static Events PassTime(PassTime c)=> new TimePassed(c.Time).Singleton();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain
{
    using Events = IReadOnlyCollection<IEvent>;
    public interface IEvent { } // used to mimic a discriminated union
    public interface ICommand { }

    public static class EventExtensions
    {
        public static Events Singleton( this IEvent e ) => new IEvent[ 1 ] { e };
    }

    public interface IClock
    {
        DateTime Time { get; }
    }

}

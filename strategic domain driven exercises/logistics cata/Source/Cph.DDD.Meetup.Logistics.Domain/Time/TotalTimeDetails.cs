using Cph.DDD.Meetup.Logistics.Domain.Common;


namespace Cph.DDD.Meetup.Logistics.Domain.Time
{
    public class TotalTimeDetails : IVersioned
    {
        public TotalTimeDetails( Guid id, uint totalTime )
        {

            Id = id;
            TotalTime = totalTime;
            Version = 1;
        }

        public TotalTimeDetails( Guid id, uint totalTime, ulong version ) : this( id, totalTime )
        {

            Version = version;
        }

        public Guid Id { get; set; }
        public uint TotalTime { get; set; }
        public ulong Version { get; set; }
    }
}

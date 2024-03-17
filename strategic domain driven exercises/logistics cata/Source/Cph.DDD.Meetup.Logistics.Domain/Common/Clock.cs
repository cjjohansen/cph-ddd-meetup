namespace Cph.DDD.Meetup.Logistics.Domain.Common
{
    public interface IClock
    {
        DateTime Time { get; }
    }

    public class Clock : IClock
    {

        public Clock( DateTime time )
        {
            Time = time;
        }

        public DateTime Time { get; set; }
    }

}

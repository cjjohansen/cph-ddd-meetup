namespace Cph.DDD.Meetup.Logistics.Domain
{
    public interface IRoutingSource
    {
        Route FindRoute(Place currentLocation, Place destination);
    }


    public class EmptyRoutingSource : IRoutingSource
    {
        public Route FindRoute( Place currentLocation, Place destination )
        {
            throw new NotImplementedException();
        }
    }
}

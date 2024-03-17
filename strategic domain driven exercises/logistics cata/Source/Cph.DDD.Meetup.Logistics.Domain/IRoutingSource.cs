namespace Cph.DDD.Meetup.Logistics.Domain
{
    public interface IRoutingSource
    {
        Route FindRoute( FreightLocation currentLocation, FreightLocation destination );
    }


    public class EmptyRoutingSource : IRoutingSource
    {
        public Route FindRoute( FreightLocation currentLocation, FreightLocation destination )
        {
            throw new NotImplementedException();
        }
    }
}

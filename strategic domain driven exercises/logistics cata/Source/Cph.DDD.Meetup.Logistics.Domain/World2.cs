using Cph.DDD.Meetup.Logistics.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain
{
    

    
    
    public class World2 : IRoutingSource
    {

        private readonly Dictionary<string, ContainerStore> Places = new Dictionary<string, ContainerStore>();
        private readonly HashSet<TruckState> Vehicles = new HashSet<TruckState>();
        private readonly ISet<ContainerState> Containers = new HashSet<ContainerState>();
        private readonly Dictionary<(FreightLocation, FreightLocation), Route> Routing = new Dictionary<(FreightLocation, FreightLocation), Route>();
        private readonly List<BookingState> Bookings = new List<BookingState>();

        private readonly DateTime now = new DateTime( 2024, 1, 1, 8, 0, 0, 0 );

        public IClock CurrentTime { get; private set; } 


        int bookingId = 1;
        // = "AABB".Select(x=> x.ToString()

        public World2( IEnumerable<string> dailyBookings)
        {

            var factoryLocation = new FreightLocation( new FreightLocationId( 1 ), "Factory" );
            var portLocation = new FreightLocation( new FreightLocationId( 2 ), "Port" );
            var warehouseALocation = new FreightLocation( new FreightLocationId( 3 ), "A" );
            var warehouseBLocation = new FreightLocation( new FreightLocationId( 4 ), "B" );

            CurrentTime = new Clock( now );




            var factory = new ContainerStore( new ContainerStoreId(IdProvider.GetNextId(typeof(ContainerStoreId)) ),factoryLocation , Containers );
            var port = new ContainerStore( new ContainerStoreId( IdProvider.GetNextId( typeof( ContainerStoreId ) ) ), portLocation, new HashSet<ContainerState>());
            var warehouseA = new ContainerStore( new ContainerStoreId( IdProvider.GetNextId( typeof( ContainerStoreId ) ) ),warehouseALocation, new HashSet<ContainerState>());
            var warehouseB = new ContainerStore( new ContainerStoreId( IdProvider.GetNextId( typeof( ContainerStoreId ) ) ), warehouseBLocation, new HashSet<ContainerState>());

            Places.Add( "Factory", factory );
            Places.Add( "Port", port);
            Places.Add( "A", warehouseA );
            Places.Add( "B", warehouseB);


            this.Routing.Add( (factory.Location, warehouseA.Location), new Route( factory.Location, port.Location, TimeSpan.FromHours( 1 ) ) );
            this.Routing.Add( (port.Location, warehouseA.Location), new Route( port.Location, warehouseA.Location, TimeSpan.FromHours( 4 ) ) );
            this.Routing.Add( (factory.Location, warehouseB.Location), new Route( factory.Location, warehouseB.Location, TimeSpan.FromHours( 5 ) ) );




            //foreach ( var booking in dailyBookings)
            //{

            //    var cmd = new 


            //    Containers.Add()




            //    Bookings.Add( BookingDecider.Decide(new Accept( new BookingId(bookingId), ) ))


            //} 

            

            //Containers.Add()


            


        }

        public Route FindRoute( FreightLocation currentLocation, FreightLocation destination )
        {
            return this.Routing[ (currentLocation, destination) ];
        }
    }
}

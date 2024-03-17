//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Cph.DDD.Meetup.Logistics.Domain
//{
//    public class World : IRoutingSource
//    {
//        public World(IEnumerable<string> containerDestinations)
//        {
//            var factory = new Place("Factory", this);
//            var port = new Place("Port", this);
//            var warehouseA = new Place("A", this);
//            var warehouseB = new Place("B", this);

//            this.Places.Add(factory.Name, factory);
//            this.Places.Add(port.Name, port);
//            this.Places.Add(warehouseA.Name, warehouseA);
//            this.Places.Add(warehouseB.Name, warehouseB);

//            this.Routing.Add((factory, warehouseA), new Route(factory., port, TimeSpan.FromHours(1)));
//            this.Routing.Add((port, warehouseA), new Route(port, warehouseA, TimeSpan.FromHours(4)));
//            this.Routing.Add((factory, warehouseB), new Route(factory, warehouseB, TimeSpan.FromHours(5)));

//            this.Vehicles.Add(new Vehicle("Truck 1", factory));
//            this.Vehicles.Add(new Vehicle("Truck 2", factory));
//            this.Vehicles.Add(new Vehicle("Ship", port));

//            foreach (var containerDestination in containerDestinations)
//                this.Containers.Add(new Container(this.Places[containerDestination], factory));
//        }

//        private readonly Dictionary<string, Place> Places = new Dictionary<string, Place>();
//        private readonly HashSet<Vehicle> Vehicles = new HashSet<Vehicle>();
//        private readonly HashSet<Container> Containers = new HashSet<Container>();
//        private readonly Dictionary<(FreightLocation, FreightLocation), Route> Routing = new Dictionary<(FreightLocation, FreightLocation), Route>();

//        public TimeSpan CurrentTime { get; private set; } = TimeSpan.Zero;

//        public void Deliver() 
//        {
//            this.Print();

//            while(!this.AreAllContainersDelivered)
//            {
//                foreach (var vehicle in this.Vehicles)
//                    vehicle.Load();

//                var elapsedTime = TimeSpan.FromHours(1);

//                this.CurrentTime += elapsedTime;

//                foreach (var vehicle in this.Vehicles)
//                    vehicle.Travel(elapsedTime);

//                foreach (var vehicle in this.Vehicles)
//                    vehicle.Unload();

//                this.Print();
//            }
//        }

//        public Route FindRoute(FreightLocation currentLocation, FreightLocation destination) => this.Routing[(currentLocation, destination)];

//        private bool AreAllContainersDelivered => this.Containers.All(x => x.IsAtDestination);

//        private void Print()
//        {
//            Console.WriteLine(this.CurrentTime);

//            foreach (var vehicle in this.Vehicles)
//                Console.WriteLine(vehicle);

//            foreach (var container in this.Containers)
//                Console.WriteLine(container);

//            Console.WriteLine(Environment.NewLine);
//        }
//    }
//}

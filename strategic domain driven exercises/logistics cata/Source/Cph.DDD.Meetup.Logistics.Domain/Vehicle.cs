//using System;

//namespace Cph.DDD.Meetup.Logistics.Domain
//{
//    enum Action { Loading, Travelling, Unloading, Returning }

//    public class Vehicle
//    {
//        public Vehicle(string name, Place location)
//        {
//            this.Name = name;
//            this.Location = location;
//        }

//        public string Name { get; }
//        public Route Route { get; private set; }
//        public Place Location { get; private set; }
//        public Container Container { get; private set; }

//        private Action Action { get; set; } = Action.Loading;
//        private TimeSpan TravellingEta { get; set; } = TimeSpan.Zero;
        
//        public void Load()
//        {
//            if (this.Action != Action.Loading)
//                return;

//            var routingInstructions = this.Location.LoadContainer();

//            if (routingInstructions != null)
//            {
//                this.Container = routingInstructions.Container;
//                this.Route = routingInstructions.Route;
//                this.Action = Action.Travelling;
//                this.TravellingEta = this.Route.Length;
//                this.Location = null;
//            }
//        }

//        public void Travel(TimeSpan elapsedTime)
//        {
//            if (this.Action != Action.Travelling && this.Action != Action.Returning)
//                return;
            
//            this.TravellingEta -= elapsedTime;

//            if (this.TravellingEta <= TimeSpan.Zero)
//            {
//                if (this.Action == Action.Travelling)
//                {
//                    this.Action = Action.Unloading;
//                    this.Location = this.Route.UnloadingPoint;
//                }
//                else if (this.Action == Action.Returning)
//                {
//                    this.Action = Action.Loading;
//                    this.Location = this.Route.LoadingPoint;
//                    this.Route = null;
//                }
//            }
//        }

//        public void Unload()
//        {
//            if (this.Action != Action.Unloading)
//                return;

//            this.Location.UnloadContainer(this.Container);
//            this.Container = null;

//            this.Action = Action.Returning;
//            this.TravellingEta = this.Route.Length;
//            this.Location = null;
//        }

//        public override string ToString()
//        {
//            var result = this.Name;

//            if (this.Location != null)
//                result += " at " + this.Location.Name;

//            if (this.Route != null)
//            {
//                var from = this.Action == Action.Travelling ? this.Route.LoadingPoint.Name : this.Route.UnloadingPoint.Name;
//                var to = this.Action == Action.Travelling ? this.Route.UnloadingPoint.Name : this.Route.LoadingPoint.Name;
//                var withContainer = this.Container != null ? $" with container for {this.Container.Destination.Name}" : string.Empty;

//                result += $" on route from {from} to {to}{withContainer} (ETA: {this.TravellingEta})";
//            }

//            return result;
//        }
//    }
//}

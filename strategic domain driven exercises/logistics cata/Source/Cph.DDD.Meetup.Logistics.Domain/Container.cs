namespace Cph.DDD.Meetup.Logistics.Domain
{
    public class Container
    {
        public Container(Place destination, Place currentLocation)
        {
            this.Destination = destination;
            this.CurrentLocation = currentLocation;

            this.CurrentLocation.UnloadContainer(this);
        }

        public Place Destination { get; }
        public Place CurrentLocation { get; private set; }
        public bool IsAtDestination { get; private set; }

        public void PlaceAt(Place currentLocation)
        {
            this.CurrentLocation = currentLocation;
            this.IsAtDestination = this.CurrentLocation == this.Destination;
        }

        public override string ToString() => this.IsAtDestination ? "At destination" : $"Destined for {this.Destination.Name}, currently at {this.CurrentLocation.Name}";
    }
}

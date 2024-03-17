namespace Cph.DDD.Meetup.Logistics.Domain
{
    public class Container
    {
        public Container(FreightLocation destination, FreightLocation currentLocation)
        {
            this.Destination = destination;
            this.CurrentLocation = currentLocation;

            //this.CurrentLocation.UnloadContainer(this);
        }

        public FreightLocation Destination { get; }
        public FreightLocation CurrentLocation { get; private set; }
        public bool IsAtDestination { get; private set; }

        public void PlaceAt(FreightLocation currentLocation)
        {
            this.CurrentLocation = currentLocation;
            this.IsAtDestination = this.CurrentLocation == this.Destination;
        }

        public override string ToString() => this.IsAtDestination ? "At destination" : $"Destined for {this.Destination.Name}, currently at {this.CurrentLocation.Name}";
    }
}

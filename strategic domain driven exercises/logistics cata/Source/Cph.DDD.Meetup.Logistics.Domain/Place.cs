using System.Collections.Generic;

namespace Cph.DDD.Meetup.Logistics.Domain
{
    public class Place : IContainerStore
    {
        public Place(string name, IRoutingSource routingSource)
        {
            this.Name = name;
            this.RoutingSource = routingSource;
        }

        public string Name { get; }
        public IRoutingSource RoutingSource { get; }
        private List<Container> Containers { get; } = new List<Container>();

        public RoutingInstructions LoadContainer()
        {
            if (this.Containers.Count == 0)
                return null;

            var container = this.Containers[0];
            this.Containers.RemoveAt(0);

            return new RoutingInstructions(container, this.RoutingSource.FindRoute(this, container.Destination));
        }

        public void UnloadContainer(Container container)
        {
            container.PlaceAt(this);
            this.Containers.Add(container);
        }
    }
}

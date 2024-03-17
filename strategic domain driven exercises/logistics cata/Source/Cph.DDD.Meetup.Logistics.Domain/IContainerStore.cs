namespace Cph.DDD.Meetup.Logistics.Domain
{
    public interface IContainerStore
    {
        FreightInstructions LoadContainer();
        void UnloadContainer(Container container);
    }

    public class FreightInstructions
    {
        public FreightInstructions(Container container, Route route)
        {
            this.Container = container;
            this.Route = route;
        }

        public Container Container { get; }
        public Route Route { get; }
    }
}

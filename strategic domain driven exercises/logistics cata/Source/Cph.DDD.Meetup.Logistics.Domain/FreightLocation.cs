namespace Cph.DDD.Meetup.Logistics.Domain
{
    public record FreightLocationId( int Id );

    public class FreightLocation
    {
        public FreightLocationId Id { get; init; }
        public string Name { get; init; }

        public FreightLocation( FreightLocationId id, string name )
        {
            Id = id;
            Name = name;
        }

        public static FreightLocation EmptyFreightLocation
        {
            get
            {
                return new FreightLocation(new  FreightLocationId(0), "Not Set");
            }   
        }

    }
}

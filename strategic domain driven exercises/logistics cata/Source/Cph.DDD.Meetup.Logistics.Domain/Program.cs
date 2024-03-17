
namespace Cph.DDD.Meetup.Logistics.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            var destinations = args[0];

            var world = new World2(destinations.Select(x => x.ToString()));
        }
    }
}

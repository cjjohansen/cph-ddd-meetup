using System.Linq;

namespace TransportTycoon
{
    class Program
    {
        static void Main(string[] args)
        {
            var destinations = args[0];

            var world = new World(destinations.Select(x => x.ToString()));
        }
    }
}

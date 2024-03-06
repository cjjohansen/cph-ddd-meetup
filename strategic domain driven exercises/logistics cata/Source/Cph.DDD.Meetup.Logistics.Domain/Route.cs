using System;

namespace Cph.DDD.Meetup.Logistics.Domain
{
    public class Route
    {
        public Route(Place loadingPoint, Place unloadingPoint, TimeSpan length)
        {
            this.LoadingPoint = loadingPoint;
            this.UnloadingPoint = unloadingPoint;
            this.Length = length;
        }
        
        public Place LoadingPoint { get; }
        public Place UnloadingPoint { get; }
        public TimeSpan Length { get; set; }
    }
}

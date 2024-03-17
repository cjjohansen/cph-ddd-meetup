using System;

namespace Cph.DDD.Meetup.Logistics.Domain
{
    public class Route
    {
        public Route(FreightLocation loadingPoint, FreightLocation unloadingPoint, TimeSpan length)
        {
            this.LoadingPoint = loadingPoint;
            this.UnloadingPoint = unloadingPoint;
            this.Length = length;
        }
        
        public FreightLocation LoadingPoint { get; }
        public FreightLocation UnloadingPoint { get; }
        public TimeSpan Length { get; set; }
    }
}

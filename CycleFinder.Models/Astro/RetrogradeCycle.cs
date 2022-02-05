using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models.Astro
{
    public class RetrogradeCycle
    {
        private readonly Dictionary<DateTime, Coordinates> _coordinates;

        public Planet Planet { get; }
        public RetrogradeStatus Status {get;}
        public DateTime Start { get; }
        public DateTime End { get; }
        public double MaxSpeed { get;  }
        public Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus RetrogradeStatus)> RetrogradeStatusByDay { get; }

        public RetrogradeCycle(Planet planet, Dictionary<DateTime, Coordinates> coordinates, RetrogradeStatus status, DateTime start, DateTime end)
        {
            _coordinates = coordinates;
            Planet = planet;
            Status = status;
            Start = start;
            End = end;

            MaxSpeed = Status == RetrogradeStatus.Direct ? _coordinates.Values.Max(_ => _.Speed) : _coordinates.Values.Min(_ => _.Speed);

            RetrogradeStatusByDay = _coordinates.ToDictionary(_ => _.Key, _ => (_.Value, _.Value.Speed == MaxSpeed ? GetMaxSpeedStatus : Status));
        }

        private RetrogradeStatus GetMaxSpeedStatus => 
            Status == RetrogradeStatus.Direct 
            ? RetrogradeStatus.MaxDirect 
            : Status == RetrogradeStatus.Retrograde 
                ? RetrogradeStatus.MaxRetrograde 
                : Status;
    }
}

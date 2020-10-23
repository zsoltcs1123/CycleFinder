using System;
using System.Collections.Generic;
using System.Text;

namespace CycleFinder.Models.Specifications
{
    public class ExtremeCandleMarkerSpecification : MarkerSpecification
    {
        public Extreme Extreme { get; set; }
        public override bool IsValid => Extreme == Extreme.Low || Extreme == Extreme.High;

    }
}

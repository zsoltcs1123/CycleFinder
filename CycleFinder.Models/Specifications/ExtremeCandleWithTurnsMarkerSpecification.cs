namespace CycleFinder.Models.Specifications
{
    public class ExtremeCandleWithTurnsMarkerSpecification : MarkerSpecification
    {
        public Extreme Extreme { get; set; }
        public bool IncluePrimaryStaticCycles { get; set; }
        public bool IncludeSecondaryStaticCycles { get; set; }

        public override bool IsValid => Extreme == Extreme.Low || Extreme == Extreme.High && (IncludeSecondaryStaticCycles || IncludeSecondaryStaticCycles);

    }
}

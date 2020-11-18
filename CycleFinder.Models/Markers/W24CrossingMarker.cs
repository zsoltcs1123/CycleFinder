using System;
using System.Drawing;
using CycleFinder.Models.Extensions;

namespace CycleFinder.Models.Markers
{
    public class W24CrossingMarker : EventMarker
    {
        public W24CrossingMarker(DateTime time, Planet planet, double longitude) : base(time)
        {
            //TODO color switch is defined in multiple places
            Color = planet switch
            {
                Planet.Moon => Color.Silver,
                Planet.Sun => Color.Goldenrod,
                Planet.Mercury => Color.Gray,
                Planet.Venus => Color.Violet,
                Planet.Mars => Color.Red,
                Planet.Jupiter => Color.Orange,
                Planet.Saturn => Color.Brown,
                Planet.Uranus => Color.Green,
                Planet.Neptune => Color.Purple,
                Planet.Pluto => Color.Blue,
                _ => Color.Black,
            };

            Text = $"{planet.GetDescription()} {longitude}";
            Text = "";
        }
    }
}

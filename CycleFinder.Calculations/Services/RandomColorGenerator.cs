using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CycleFinder.Calculations.Services
{
    public class RandomColorGenerator : IRandomColorGenerator
    {
        private static Random _rnd = new Random();
        private List<Color> _generatedColors = new List<Color>();
        public Color GetRandomColor()
        {
            var color = Color.FromArgb(_rnd.Next(256), _rnd.Next(256), _rnd.Next(256));
            return !ColorAlreadyGenerated(color) ? color : GetRandomColor();
        }

        private static bool AreColorsEqual(Color c1, Color c2) => c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        private bool ColorAlreadyGenerated(Color c) => _generatedColors.Any(_ => AreColorsEqual(_, c));
    }
}

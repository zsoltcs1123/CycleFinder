using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace CycleFinder.Models.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }

        public static IEnumerable<T> GetFlags<T>(this T e) where T:Enum
        {
            foreach (T value in Enum.GetValues(e.GetType()))
            {
                if (e.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        public static bool HasMultipleValues<T>(this T e) where T:Enum
        {
            return !Enum.IsDefined(typeof(T), e);
        }

        public static Color ToColor(this Planet planet)
        {
            return planet switch
            {
                Planet.Moon => Color.Silver,
                Planet.Sun => Color.Gold,
                Planet.Mercury => Color.Gray,
                Planet.Venus => Color.Pink,
                Planet.Mars => Color.Red,
                Planet.Jupiter => Color.Orange,
                Planet.Saturn => Color.Brown,
                Planet.Uranus => Color.Green,
                Planet.Neptune => Color.Purple,
                Planet.Pluto => Color.Blue,
                _ => Color.Black,
            };
        }
    }
}
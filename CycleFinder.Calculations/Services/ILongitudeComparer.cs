using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services
{
    public interface ILongitudeComparer
    {
        public bool AreEqual(Planets planet, double longitude1, double longitude2);
    }
}

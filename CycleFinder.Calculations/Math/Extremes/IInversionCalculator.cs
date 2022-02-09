using System.Collections.Generic;

namespace CycleFinder.Calculations.Math.Extremes
{
    /// <summary>
    /// Finds the inversion points in a directional data series. Useful for determining declination, latitude & speed changes.
    /// </summary>
    public interface IInversionCalculator
    {
        /// <summary>
        /// Finds the inversion points in a directional data series.
        /// </summary>
        /// <param name="arr">Array which contains the data.</param>
        /// <returns>IEnumerable of indices which are turning points in the data series.</returns>
        public IEnumerable<int> FindInversions(double[] arr);

        /// <summary>
        /// Finds the inversion points in a directional data series where the series reaches a minimum.
        /// </summary>
        /// <param name="arr">Array which contains the data.</param>
        /// <returns>IEnumerable of indices which are minima in the data series.</returns>
        public IEnumerable<int> FindMinima(double[] arr);

        /// <summary>
        /// Finds the inversion points in a directional data series where the series reaches a maximum.
        /// </summary>
        /// <param name="arr">Array which contains the data.</param>
        /// <returns>IEnumerable of indices which are maxima in the data series.</returns>
        public IEnumerable<int> FindMaxima(double[] arr);

    }
}

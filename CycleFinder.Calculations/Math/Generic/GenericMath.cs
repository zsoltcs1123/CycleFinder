using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Math.Generic
{
    public static class GenericMath
    {
        /// <summary>
        /// Returns a list of indices which are local minima in the array.
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="order">Specifies how many adjacent elements needed for the local minima to be valid.</param>
        /// <returns></returns>
        public static List<int> FindLocalMinima(double[] arr, int order = 1)
        {
            static bool c_func(double e1, double e2) => e1 <= e2;
            return FindLocalExtreme(arr, c_func, order);
        }

        /// <summary>
        /// Returns a list of indices which are local maxima in the array.
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="order">Specifies how many adjacent elements needed for the local maxima to be valid.</param>
        /// <returns></returns>
        public static List<int> FindLocalMaxima(double[] arr, int order = 1)
        {
            static bool c_func(double e1, double e2) => e1 >= e2;
            return FindLocalExtreme(arr, c_func, order);
        }

        //uses linear regression
        public static List<int> FindLocalMaxima(double[] arr)
        {
            var ret = new List<int>();

            var ascending = "asc";
            var descending = "desc";

            //calculate & map slopes & direction
            (double val, double slope, string direction)[] arrWithSlopes = new (double, double, string)[arr.Length];

            arrWithSlopes[0] = (arr[0], 0, "");
            for (int i=0; i<arr.Length-1; i++)
            {
                arrWithSlopes[i+1] = (arr[i+1], (System.Math.Round(arr[i] - arr[i + 1],2))*-1,"");

                if (arrWithSlopes[i+1].slope > 0)
                {
                    arrWithSlopes[i+1].direction = ascending;
                }
                else if (arrWithSlopes[i+1].slope == 0)
                {
                    if (arrWithSlopes[i].direction == ascending)
                        arrWithSlopes[i+1].direction = ascending;
                    else
                        arrWithSlopes[i+1].direction = descending;
                }
                else
                {
                    arrWithSlopes[i+1].direction = descending;
                }
            }

            //skip first 
            for (int i = 1; i < arrWithSlopes.Length - 1; i++)
            {
                if (arrWithSlopes[i].direction == ascending && arrWithSlopes[i].direction != arrWithSlopes[i+1].direction)
                {
                    ret.Add(i);
                }
            }
            return ret;
        }

        public static List<int> FindLocalMinima(double[] arr)
        {
            var ret = new List<int>();

            //calculate & map slopes
            (double val, double slope)[] arrWithSlopes = new (double, double)[arr.Length];

            arrWithSlopes[0] = (arr[0], 0);
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arrWithSlopes[i + 1] = (arr[i + 1], (System.Math.Round(arr[i] - arr[i + 1], 2)) * -1);
            }

            //skip first 
            for (int i = 1; i < arrWithSlopes.Length - 1; i++)
            {
                if (arrWithSlopes[i].slope <= 0 && arrWithSlopes[i + 1].slope > 0)
                {
                    ret.Add(i);
                }
            }
            return ret;
        }

        private static List<int> FindLocalExtreme(double[] arr, Func<double, double, bool> comparerFunc, int order = 1)
        {
            List<int> mn = new List<int>();

            //Ensure input array has at least 2 elements
            if (arr.Length < 2)
            {
                return new List<int>() { 0 };
            }

            bool firstElementIsExtreme(double[] arr, int order)
            {
                bool isExtreme = true;
                for (int i = 1; i <= order && isExtreme; i++)
                {
                    isExtreme = comparerFunc(arr[0], arr[0 + i]);
                }
                return isExtreme;
            }

            bool lastElementIsExtreme(double[] arr, int order)
            {
                bool isExtreme = true;
                for (int i = 1; i <= order && isExtreme; i++)
                {
                    isExtreme = comparerFunc(arr[arr.Length - 1], arr[arr.Length - 1 - i]);
                }
                return isExtreme;
            }

            bool elementIsExtreme(double[] arr, int index, int order)
            {
                bool isExtreme = true;
                for (int i = 1; i <= order && isExtreme; i++)
                {
                    if (index + i >= arr.Length - 1)
                    {
                        //only check backwards
                        isExtreme = comparerFunc(arr[index], arr[index - i]);
                    }
                    else if (index - i < 0)
                    {
                        //only check forward
                        isExtreme = comparerFunc(arr[index], arr[index + i]);
                    }
                    else
                    {
                        isExtreme = comparerFunc(arr[index], arr[index + i]) && comparerFunc(arr[index], arr[index - i]);
                    }
                }
                return isExtreme;
            }

            if (firstElementIsExtreme(arr, order))
            {
                mn.Add(0);
            }

            if (lastElementIsExtreme(arr, order))
            {
                mn.Add(arr.Length - 1);
            }

            for (int i = 1; i < arr.Length; i++)
            {
                if (elementIsExtreme(arr, i, order)) mn.Add(i);
            }

            return mn;
        }
    }
}

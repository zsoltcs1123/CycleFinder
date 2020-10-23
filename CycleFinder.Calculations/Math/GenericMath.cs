using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
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
            List<int> mn = new List<int>();

            //Ensure input array has at least 2 elements
            if (arr.Length < 2)
            {
                return new List<int>() { 0 };
            }

            static bool firstElementIsMinima(double[] arr, int order)
            {
                bool isMinima = true;
                for (int i = 1; i <= order && isMinima; i++)
                {
                    isMinima = arr[0] < arr[0 + i];
                }
                return isMinima;
            }

            static bool lastElementIsMinima(double[] arr, int order)
            {
                bool isMinima = true;
                for (int i = 1; i <= order && isMinima; i++)
                {
                    isMinima = arr[arr.Length - 1] < arr[(arr.Length-1) - i];
                }
                return isMinima;
            }

            static bool elementIsMinima(double[] arr, int index, int order)
            {
                bool isMinima = true;
                for (int i = 1; i <= order && isMinima; i++)
                {
                    if (index + i >= arr.Length - 1)
                    {
                        //only check backwards
                        isMinima = arr[index] < arr[index - i];
                    }
                    else if (index - i < 0)
                    {
                        //only check forward
                        isMinima = arr[index] < arr[index + i];
                    }
                    else
                    {
                        isMinima = arr[index] < arr[index + i] && arr[index] < arr[index - i];
                    }
                }
                return isMinima;
            }

            if (firstElementIsMinima(arr, order))
            {
                mn.Add(0);
            }

            if (lastElementIsMinima(arr, order))
            {
                mn.Add(arr.Length-1);
            }

            for (int i = 1; i < arr.Length; i++)
            {
                if (elementIsMinima(arr, i, order)) mn.Add(i);
            }

            return mn;
        }

        /// <summary>
        /// Returns a list of indices which are local maxima in the array.
        /// </summary>
        /// <param name="arr">Input array</param>
        /// <param name="order">Specifies how many adjacent elements needed for the local maxima to be valid.</param>
        /// <returns></returns>
        public static List<int> FindLocalMaxima(double[] arr, int order = 1)
        {
            return FindLocalExtreme(arr, (e1, e2) => e1 > e2, order);            
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
                    isExtreme = comparerFunc(arr[arr.Length - 1], arr[(arr.Length - 1) - i]);
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

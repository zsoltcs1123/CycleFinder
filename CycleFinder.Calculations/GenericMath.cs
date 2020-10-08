using System;
using System.Collections;
using System.Collections.Generic;

namespace CycleFinder.Calculations
{
    public static class GenericMath
    {
        // Function to find all the local maxima  
        // and minima in the given array arr[]  
        public static void findLocalMaximaMinima(int[] arr, int order)
        {
            int lenght = arr.Length;

            // Empty vector to store points of  
            // local maxima and minima  
            ArrayList mx = new ArrayList();
            ArrayList mn = new ArrayList();

            // Checking whether the first point is  
            // local maxima or minima or none  
            if (arr[0] > arr[1])
                mx.Add(0);

            else if (arr[0] < arr[1])
                mn.Add(0);

            // Iterating over all points to check  
            // local maxima and local minima  
            for (int i = 1; i < lenght - 1; i++)
            {

                // Condition for local minima   
                if ((arr[i - 1] > arr[i]) &&
                    (arr[i] < arr[i + 1]))
                    mn.Add(i);

                // Condition for local maxima  
                else if ((arr[i - 1] < arr[i]) &&
                         (arr[i] > arr[i + 1]))
                    mx.Add(i);
            }

            // Checking whether the last point is  
            // local maxima or minima or none  
            if (arr[lenght - 1] > arr[lenght - 2])
                mx.Add(lenght - 1);

            else if (arr[lenght - 1] < arr[lenght - 2])
                mn.Add(lenght - 1);
        }


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
    }
}

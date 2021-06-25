namespace CycleFinder.Calculations.Math.Fibonacci
{
    public class FibonacciCalculator : IFibonacciCalculator
    {
        public int GetFib(int term)
        {
            {
                if (term <= 2)
                    return 1;
                else
                    return GetFib(term - 1) + GetFib(term - 2);
            }
        }
    }
}

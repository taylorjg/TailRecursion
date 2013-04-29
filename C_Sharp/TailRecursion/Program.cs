using System;
using System.Linq;

namespace TailRecursion
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("TailRecursion (C#)");
            foreach (var x in Enumerable.Range(1, 10))
            {
                Console.WriteLine("Factorial({0}): {1}", x, Factorial(x));
            }
        }

        private static int Factorial(int x)
        {
            return FactorialHelper(x, 1);
        }

        private static int FactorialHelper(int x, int acc)
        {
            if (x <= 1)
            {
                return acc;
            }

            return FactorialHelper(x - 1, x * acc);
        }
    }
}

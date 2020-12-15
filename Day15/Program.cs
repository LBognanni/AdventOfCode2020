using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new int[] { 1, 2, 16, 19, 18, 0 };

            var result = EnumerateNumbers(input).Skip(2019).First();
            Console.WriteLine($"Part 1: The 2020th number is {result}");
        }

        private static IEnumerable<int> EnumerateNumbers(int[] seed)
        {
            var previousNumbers = new List<int>(seed[..^1]);
            var nextNumber = seed[^1];

            foreach(var number in seed)
            {
                yield return number;
            }

            while(true)
            {
                var index = previousNumbers.LastIndexOf(nextNumber) + 1;
                previousNumbers.Add(nextNumber);

                if (index == 0)
                {
                    nextNumber = index;
                }
                else
                {
                    nextNumber = previousNumbers.Count - index;
                }

                yield return nextNumber;
            }
        }
    }
}

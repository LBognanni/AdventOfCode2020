using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllLines("input.txt").Select(int.Parse);

            var differences = FindJoltageDifferences(numbers);
            Console.WriteLine($"Part 1: The solution is {differences}.");
        }

        private static long FindJoltageDifferences(IEnumerable<int> numbers)
        {
            int differencesOf1 = 0;
            int differencesOf3 = 1;
            int previousJoltage = 0;

            foreach (var joltage in numbers.OrderBy(j => j))
            {
                switch (joltage - previousJoltage)
                {
                    case 3:
                        differencesOf3++;
                        break;
                    case 1:
                        differencesOf1++;
                        break;
                }

                previousJoltage = joltage;
            }

            return differencesOf1 * differencesOf3;
        }
    }
}

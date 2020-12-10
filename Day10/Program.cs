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
            Console.WriteLine($"Part 1: The solution is {differences.difsOf1 * differences.difsOf3}.");

            var combinations = CountCombinations(differences.contiguosDifsOf1);

            Console.WriteLine($"Part 1: The solution is {combinations}.");
        }

        private static long CountCombinations(int[] contiguosDifsOf1)
        {
            // 7,10,11,12,15             would give a contiguous number of 2. We can take out the 11 there but not 10 or 12 = 1 combination
            // 7,10,11,12,13,16          would give a contiguous number of 3. We can take out the 11 or 12 or both = 3 combinations
            // 7,10,11,12,13,14,17       would give a contiguous number of 4. We can take out the 11,12,13 or both 11&12, 12&13, 11&13 = 6 combinations
            var numbersThatITotallyDidNotCalculateManually = new Dictionary<int, long>
            {
                [2] = 1,
                [3] = 3,
                [4] = 6,
                [5] = 12
            };

            return contiguosDifsOf1.Select(d => numbersThatITotallyDidNotCalculateManually[d]).Aggregate(1l, (x, y) => (x) * (y + 1));
        }

        private static (int difsOf1, int difsOf3, int []contiguosDifsOf1) FindJoltageDifferences(IEnumerable<int> numbers)
        {
            int differencesOf1 = 0;
            int differencesOf3 = 1;
            int previousJoltage = 0;
            int contiguous1 = 0;
            List<int> contiguousDifsOf1 = new List<int>();

            foreach (var joltage in numbers.OrderBy(j => j))
            {
                switch (joltage - previousJoltage)
                {
                    case 3:
                        differencesOf3++;
                        if(contiguous1 > 1)
                        {
                            contiguousDifsOf1.Add(contiguous1);
                        }
                        contiguous1 = 0;
                        break;
                    case 1:
                        differencesOf1++;
                        contiguous1++;
                        break;
                }

                previousJoltage = joltage;
            }
            if (contiguous1 > 1)
            {
                contiguousDifsOf1.Add(contiguous1);
            }

            return (differencesOf1, differencesOf3, contiguousDifsOf1.ToArray());
        }
    }
}

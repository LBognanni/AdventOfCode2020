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
           
            var resultPt2 = EnumerateNumbers(input).Skip(30000000-1).First();
            Console.WriteLine($"Part 2: The 2020th number is {resultPt2}");
        }

        private static IEnumerable<int> EnumerateNumbers(int[] seed)
        {
            int turn = 1;
            var lastSeen = new Dictionary<int, int>();
            var nextNumber = seed[^1];

            foreach(var number in seed[..^1])
            {
                lastSeen[number] = turn;
                turn++; 
                yield return number;
            }
            yield return nextNumber;

            while(true)
            {
                lastSeen.TryGetValue(nextNumber, out int index);
                lastSeen[nextNumber] = turn;

                if (index == 0)
                {
                    nextNumber = index;
                }
                else
                {
                    nextNumber = turn - index;
                }

                yield return nextNumber;
                turn++;
            }
        }
    }
}

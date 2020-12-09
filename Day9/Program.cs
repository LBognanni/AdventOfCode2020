using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();

            var notSum = FindNotSum(numbers, 0);

            Console.WriteLine($"Part 1: First number that is not a sum: {notSum}");

            var encriptionKey = FindEncryptionKey(numbers, notSum);

            Console.WriteLine($"Part 2: The encryption key is: {encriptionKey}");
        }

        private static long FindEncryptionKey(long[] numbers, long notSum)
        {
            for (int i = 0; i < numbers.Length; ++i)
            {
                int j = i + 1;
                var acc = numbers[i];

                for (; j < numbers.Length; ++j)
                {
                    acc += numbers[j];
                    if (acc >= notSum)
                    {
                        break;
                    }
                }

                if (acc == notSum)
                {
                    var subset = numbers.Skip(i).Take(j - i);
                    return subset.Min() + subset.Max();
                }
            }

            return -1;
        }

        private static long FindNotSum(long[] numbers, int idx)
        {
            var take = idx + 25;
            
            if (numbers.Length <= take)
                return -1;

            var preamble = numbers.Take(take).ToArray();
            var nextNumber = numbers.Skip(take).First();
            if(!IsSumOfAny2(preamble, nextNumber))
            {
                return nextNumber;
            }

            return FindNotSum(numbers, idx + 1);
        }

        private static bool IsSumOfAny2(long[] preamble, long nextNumber) =>
            preamble.Any(num => preamble.Any(n => n != num && n + num == nextNumber));
    }
}

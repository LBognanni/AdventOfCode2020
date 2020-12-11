using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();

            int mutations = 0;
            while(Mutate(ref input))
            {
                mutations++;
            }
            Console.WriteLine($"Saw {mutations} mutations.");

            var occupied = input.SelectMany(l => l).Count(c => c == '#');
            Console.WriteLine($"Part 1: There are {occupied} occupied seats");
        }

        private static bool Mutate(ref char[][] input)
        {
            bool hasMutated = false;
            var mutated = new char[input.Length][];

            for (var y = 0; y < input.Length; ++y)
            {
                mutated[y] = new char[input[y].Length];
                for (var x = 0; x < input[y].Length; ++x)
                {
                    mutated[y][x] = input[y][x] switch
                    {
                        'L' => CanOccupy(input, x, y),
                        '#' => CanLeave(input, x, y),
                        _ => input[y][x]
                    };
                    if (mutated[y][x] != input[y][x])
                    {
                        hasMutated = true;
                    }
                }
            }
            var s = PrettyPrint(mutated);

            input = mutated;

            return hasMutated;
        }

        private static IEnumerable<string> PrettyPrint(char[][] mutated)
        {
            foreach(var line in mutated)
            {
                yield return new string(line);
            }
        }

        private static char CanLeave(char[][] input, int x, int y) => AtLeast(input, x, y, 4, '#') ? 'L' : '#';

        private static char CanOccupy(char[][] input, int x, int y) => AtLeast(input, x, y, 8, 'L', '.') ? '#' : 'L';

        private static bool AtLeast(char[][] input, int x, int y, int n, params char[] find)
        {
            var cells = new[]
            {
                ValueOrDefault(input, x-1, y-1),
                ValueOrDefault(input, x-1, y),
                ValueOrDefault(input, x-1, y+1),

                ValueOrDefault(input, x, y-1),
                ValueOrDefault(input, x, y+1),

                ValueOrDefault(input, x+1, y-1),
                ValueOrDefault(input, x+1, y),
                ValueOrDefault(input, x+1, y+1),
            };
            return cells.Where(c => find.Contains(c)).Count() >= n;
        }

        private static char ValueOrDefault(char[][] input, int x, int y)
        {
            const char defaultValue = '.';

            if (x < 0)
                return defaultValue;
            if (y < 0)
                return defaultValue;
            if (y >= input.Length)
                return defaultValue;
            if (x >= input[y].Length)
                return defaultValue;

            return input[y][x];
        }
    
    }
}

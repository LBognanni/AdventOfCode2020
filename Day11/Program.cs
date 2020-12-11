using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        private const char floor = '.';
        private const char occupied = '#';
        private const char free = 'L';

        static void Main(string[] args)
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();

            int mutations = 0;
            while (Mutate(ref input))
            {
                mutations++;
            }
            Console.WriteLine($"Part 1: Saw {mutations} mutations.");

            var occupied = input.SelectMany(l => l).Count(c => c == Program.occupied);
            Console.WriteLine($"Part 1: There are {occupied} occupied seats");
        }

        private static void Part2()
        {
            var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();

            int mutations = 0;
            while (MutateV2(ref input))
            {
                mutations++;
            }
            Console.WriteLine($"Part 2: Saw {mutations} mutations.");

            var occupied = input.SelectMany(l => l).Count(c => c == Program.occupied);
            Console.WriteLine($"Part 2: There are {occupied} occupied seats");
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
                        free => CanOccupy(input, x, y),
                        occupied => CanLeave(input, x, y),
                        _ => input[y][x]
                    };
                    if (mutated[y][x] != input[y][x])
                    {
                        hasMutated = true;
                    }
                }
            }

            input = mutated;

            return hasMutated;
        }
        private static bool MutateV2(ref char[][] input)
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
                        free => CanOccupyV2(input, x, y),
                        occupied => CanLeaveV2(input, x, y),
                        _ => input[y][x]
                    };
                    if (mutated[y][x] != input[y][x])
                    {
                        hasMutated = true;
                    }
                }
            }

            input = mutated;

            return hasMutated;
        }

        private static char CanLeaveV2(char[][] input, int x, int y) => NumberOf(input, x, y, occupied) > 4 ? free : occupied;

        private static char CanOccupyV2(char[][] input, int x, int y) => NumberOf(input, x, y, occupied) == 0 ? occupied : free;

        private static char CanLeave(char[][] input, int x, int y) => AtLeast(input, x, y, 4, occupied) ? free : occupied;

        private static char CanOccupy(char[][] input, int x, int y) => AtLeast(input, x, y, 8, free, floor) ? occupied : free;

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


        private static int NumberOf(char[][] input, int x, int y, char find)
        {
            var cells = new[]
            {
                ValueOrDefault(input, x, y, -1, -1),
                ValueOrDefault(input, x, y, -1, 0),
                ValueOrDefault(input, x, y, -1, 1),

                ValueOrDefault(input, x, y, 0, -1),
                ValueOrDefault(input, x, y, 0, 1),

                ValueOrDefault(input, x, y, 1, -1),
                ValueOrDefault(input, x, y, 1, 0),
                ValueOrDefault(input, x, y, 1, +1),
            };
            return cells.Where(c => find == c).Count();
        }

        private static char ValueOrDefault(char[][] input, int x, int y, int dx, int dy)
        {
            while (y >= 0 && y < input.Length && x >= 0 && x < input[y].Length)
            {
                x += dx;
                y += dy;
                var c = ValueOrDefault(input, x, y);
                if(c!= floor)
                    return c;
            }
            return floor;
        }


        private static char ValueOrDefault(char[][] input, int x, int y)
        {
            if (x < 0)
                return floor;
            if (y < 0)
                return floor;
            if (y >= input.Length)
                return floor;
            if (x >= input[y].Length)
                return floor;

            return input[y][x];
        }
    
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = LoadMap("input.txt");

            int numberOfTrees = Traverse(map, 1, 3);

            Console.WriteLine($"{numberOfTrees} trees encountered.");
        }

        private static int Traverse(bool[][] map, int slopeY, int slopeX)
        {
            int x = 0;
            int nTrees = 0;

            for (int y = 0; y < map.Length; y += slopeY, x+=slopeX)
            {
                if (map[y][x % map[y].Length])
                    nTrees++;
            }

            return nTrees;
        }

        private static bool[][] LoadMap(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var buf = new List<bool[]>();

            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                buf.Add(ParseLine(line));
            }

            return buf.ToArray();
        }

        private static bool[] ParseLine(string line)
        {
            var buf = new bool[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                buf[i] = line[i] == '#';
            }
            return buf;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = parseInstructions("input.txt");

            var offsets = new Dictionary<string, (int X, int Y)>
            {
                ["e"] = (2, 0),
                ["se"] = (1, 1),
                ["ne"] = (1, -1),
                ["w"] = (-2, 0),
                ["sw"] = (-1, 1),
                ["nw"] = (-1, -1),
            };

            HashSet<Point> points = new HashSet<Point>();
            foreach(var instructionList in instructions)
            {
                var pt = new Point(0, 0);
                foreach (var move in instructionList)
                {
                    var (x, y) = offsets[move];
                    pt = pt.Move(x, y);
                }

                if(points.Contains(pt))
                {
                    points.Remove(pt);
                }
                else
                {
                    points.Add(pt);
                }
            }

            Console.WriteLine($"Part 1: {points.Count} tiles were flipped.");
        }

        record Point(int X, int Y)
        {
            public Point Move(int oX, int oY) => new Point(X + oX, Y + oY);
        }

        static IEnumerable<string[]> parseInstructions(string fileName)
        {
            var rgx = new Regex(@"(e|se|ne|w|sw|nw)");

            foreach(var line in File.ReadAllLines(fileName))
            {
                var matches = rgx.Matches(line);
                yield return matches.Select(m => m.Value).ToArray();
            }
        }
    }
}

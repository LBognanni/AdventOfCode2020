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
            
            var points = FlipInitialTiles(instructions);
            Console.WriteLine($"Part 1: {points.Count} tiles were flipped.");

            for(int i=0;i<100;++i)
            {
                points = FlipTiles(points);
            }

            Console.WriteLine($"Part 2: {points.Count} tiles are black.");
        }

        private static IReadOnlySet<Point> FlipTiles(IReadOnlySet<Point> tiles)
        {
            var newTiles = new HashSet<Point>();
            var neighbours = CalculateNeighbours(tiles);

            foreach(var tile in tiles)
            {
                var allNeighbours = neighbours[tile];
                var activeNeighbours = allNeighbours.Intersect(tiles).Count();

                if (activeNeighbours != 0 && activeNeighbours <= 2)
                {
                    newTiles.Add(tile);
                }
            }

            foreach (var tile in neighbours.SelectMany(x => x.Value).Distinct().Except(tiles))
            {
                if (tiles.Count(tile.IsNeighbour) == 2)
                {
                    newTiles.Add(tile);
                }
            }

            return newTiles;
        }

        private static IReadOnlyDictionary<Point, Point[]> CalculateNeighbours(IEnumerable<Point> tiles)
        {
            var pts = new Dictionary<Point, Point[]>();

            foreach(var tile in tiles)
            {
                pts.Add(tile, Offsets.Values.Select(o => tile.Move(o.X, o.Y)).ToArray());
            }

            return pts;
        }

        static Dictionary<string, (int X, int Y)> Offsets = new Dictionary<string, (int X, int Y)>
        {
            ["e"] = (2, 0),
            ["se"] = (1, 1),
            ["ne"] = (1, -1),
            ["w"] = (-2, 0),
            ["sw"] = (-1, 1),
            ["nw"] = (-1, -1),
        };

        private static IReadOnlySet<Point> FlipInitialTiles(IEnumerable<string[]> instructions)
        {

            var points = new HashSet<Point>();
            foreach (var instructionList in instructions)
            {
                var pt = new Point(0, 0);
                foreach (var move in instructionList)
                {
                    var (x, y) = Offsets[move];
                    pt = pt.Move(x, y);
                }

                if (points.Contains(pt))
                {
                    points.Remove(pt);
                }
                else
                {
                    points.Add(pt);
                }
            }

            return points;
        }

        record Point(int X, int Y)
        {
            public Point Move(int oX, int oY) => new Point(X + oX, Y + oY);
            public bool IsNeighbour(Point pt)
            {
                foreach(var offset in Offsets.Values)
                {
                    if((pt.X == X +offset.X) && (pt.Y == Y + offset.Y))
                    {
                        return true;
                    }
                }
                return false;
            }
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

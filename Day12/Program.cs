using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        record Ferry(int X, int Y, int WayX, int WayY, char Direction)
        {
            public int Distance => (Math.Abs(X) + Math.Abs(Y));
        }

        record Command (char Action, int Value)
        {
            public static Command Parse(string s) => new Command(s[0], int.Parse(s[1..]));

            private static Dictionary<char, Func<Ferry, int, Ferry>> moves = new Dictionary<char, Func<Ferry, int, Ferry>>
            {
                ['N'] = (f, d) => (f with { Y = f.Y - d }),
                ['E'] = (f, d) => (f with { X = f.X + d }),
                ['W'] = (f, d) => (f with { X = f.X - d }),
                ['S'] = (f, d) => (f with { Y = f.Y + d }),
            };
            private static Dictionary<char, char> Opposites = new Dictionary<char, char>
            {
                ['N'] = 'S',
                ['E'] = 'W',
                ['W'] = 'E',
                ['S'] = 'N',
            };
            private static Dictionary<char, char> LeftRotations = new Dictionary<char, char>
            {
                ['N'] = 'W',
                ['E'] = 'N',
                ['W'] = 'S',
                ['S'] = 'E',
            };
            private static Dictionary<char, char> RightRotations = new Dictionary<char, char>
            {
                ['N'] = 'E',
                ['E'] = 'S',
                ['W'] = 'N',
                ['S'] = 'W',
            };

            public Ferry Apply(Ferry ferry)
            {
                if (moves.ContainsKey(Action))
                    return moves[Action](ferry, Value);

                switch(Action)
                {
                    case 'F':
                        return moves[ferry.Direction](ferry, Value);
                    case 'B':
                        return moves[Opposites[ferry.Direction]](ferry, Value);
                    case 'L':
                        return Rotate(ferry, LeftRotations, Value);
                    case 'R':
                        return Rotate(ferry, RightRotations, Value);
                }

                return ferry;
            }

            private Ferry Rotate(Ferry ferry, Dictionary<char, char> rotations, int value)
            {
                var direction = ferry.Direction;
                while (value > 0)
                {
                    direction = rotations[direction];
                    value -= 90;
                }
                return ferry with { Direction = direction };
            }

            private static Dictionary<char, Func<Ferry, int, Ferry>> waypointMoves = new Dictionary<char, Func<Ferry, int, Ferry>>
            {
                ['N'] = (f, d) => (f with { WayY = f.WayY - d }),
                ['E'] = (f, d) => (f with { WayX = f.WayX + d }),
                ['W'] = (f, d) => (f with { WayX = f.WayX - d }),
                ['S'] = (f, d) => (f with { WayY = f.WayY + d }),
            };

            public Ferry ApplyV2(Ferry ferry)
            {
                if (waypointMoves.ContainsKey(Action))
                    return waypointMoves[Action](ferry, Value);

                switch (Action)
                {
                    case 'F':
                        return ferry with { X = ferry.X + (ferry.WayX * Value), Y = ferry.Y + (ferry.WayY * Value) };
                    case 'B':
                        return ferry with { X = ferry.X - (ferry.WayX * Value), Y = ferry.Y - (ferry.WayY * Value) };
                    case 'L':
                        return RotateWaypointLeft(ferry);
                    case 'R':
                        return RotateWaypointRight(ferry);
                }

                return ferry;
            }

            private Ferry RotateWaypointLeft(Ferry ferry)
            {
                int value = Value;
                while (value > 0)
                {
                    ferry = ferry with { WayX = ferry.WayY, WayY = -ferry.WayX };
                    value -= 90;
                }
                return ferry;
            }
            private Ferry RotateWaypointRight(Ferry ferry)
            {

                int value = Value;
                while (value > 0)
                {
                    ferry = ferry with { WayX = -ferry.WayY, WayY = ferry.WayX };
                    value -= 90;
                }
                return ferry;
            }
        }

        static void Main(string[] args)
        {
            //var input = "F10\nN3\nF7\nR90\nF11".Split('\n').Select(Command.Parse).ToArray();

            var input = File.ReadAllLines("input.txt").Select(Command.Parse);
            var ferry = new Ferry(0, 0, 10, -1, 'E');

            foreach(var command in input)
            {
                ferry = command.Apply(ferry);
            }

            Console.WriteLine($"Part 1: Ferry is at {ferry.X}, {ferry.Y} facing {ferry.Direction}. The Manhattan distance is { ferry.Distance }");

            ferry = new Ferry(0, 0, 10, -1, 'E');

            foreach (var command in input)
            {
                ferry = command.ApplyV2(ferry);
            }

            Console.WriteLine($"Part 2: Ferry is at {ferry.X}, {ferry.Y}. The Manhattan distance is { ferry.Distance }");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var initialState = File.ReadAllLines("input.txt").SelectMany((l, y) => l.Select((c, x) => new Cube(new Position(x, y, 0, 0), c == '#')));
var finalState = RunCycles(initialState, 6);

Console.WriteLine($"Part 2: There are { finalState.Count(x=>x.Active) } active cubes");



IReadOnlyCollection<Cube> RunCycles(IEnumerable<Cube> cubes, int cycles)
{
    var newState = new Dictionary<Position, Cube>();
    var initialState = cubes.Where(c => c.Active).ToDictionary(c => c.Position);
    var minPos = new Position(0, 0, 0, 0);
    var maxPos = new Position(cubes.Max(c => c.Position.X), cubes.Max(c => c.Position.Y), 0, 0);

    for (int iCycle = 0; iCycle < cycles; ++iCycle)
    {
        newState = new Dictionary<Position, Cube>();
        minPos = minPos with { X = minPos.X - 1, Y = minPos.Y - 1, Z = minPos.Z - 1, W = minPos.W - 1 };
        maxPos = maxPos with { X = maxPos.X + 1, Y = maxPos.Y + 1, Z = maxPos.Z + 1, W = maxPos.W + 1 };

        for (int x = minPos.X; x <= maxPos.X; ++x)
        {
            for (int y = minPos.Y; y <= maxPos.Y; ++y)
            {
                for (int z = minPos.Z; z <= maxPos.Z; ++z)
                {
                    for (int w = minPos.W; w <= maxPos.W; ++w)
                    {
                        var pt = new Position(x, y, z, w);
                        var cube = GetCubeAt(pt, initialState);
                        var activeNeighbours = GetNeighbours(pt, initialState).Count(x => x.Active);

                        if ((!cube.Active && activeNeighbours == 3) || (cube.Active && ((activeNeighbours == 2 || activeNeighbours == 3))))
                        {
                            newState.Add(pt, new Cube(pt, true));
                        }
                        
                    }
                }
            }
        }

        initialState = newState;
    }

    return newState.Values;
}

IEnumerable<Cube> GetNeighbours(Position pt, Dictionary<Position, Cube> initialState)
{
    for (int x = pt.X - 1; x <= pt.X + 1; ++x)
    {
        for (int y = pt.Y - 1; y <= pt.Y + 1; ++y)
        {
            for (int z = pt.Z - 1; z <= pt.Z + 1; ++z)
            {
                for (int w = pt.W - 1; w <= pt.W + 1; ++w)
                {
                    var pos = new Position(x, y, z, w);
                    if (pt != pos)
                    {
                        yield return GetCubeAt(pos, initialState);
                    }
                }
            }
        }
    }
}

Cube GetCubeAt(Position pt, Dictionary<Position, Cube> initialState)
{
    if (initialState.TryGetValue(pt, out var cube))
    {
        return cube;
    }
    return new Cube(pt, false);
}

record Position (int X, int Y, int Z, int W);
record Cube(Position Position, bool Active);

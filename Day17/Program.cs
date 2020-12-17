using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var initialState = File.ReadAllLines("input.txt").SelectMany((l, y) => l.Select((c, x) => new Cube(x, y, 0, c == '#')));
var finalState = RunCycles(initialState, 6);

Console.WriteLine($"Part 1: There are { finalState.Count(x=>x.Active) } active cubes");



IReadOnlyCollection<Cube> RunCycles(IEnumerable<Cube> cubes, int cycles)
{
    var newState = new Dictionary<(int, int, int), Cube>();
    var initialState = cubes.ToDictionary(c => (c.X, c.Y, c.Z));
    int minX = 0, maxX = cubes.Max(c => c.X), 
        minY = 0, maxY = cubes.Max(c => c.Y), 
        minZ = 0, maxZ = 0;
    
    for (int iCycle = 0; iCycle < cycles; ++iCycle)
    {
        newState = new Dictionary<(int, int, int), Cube>();
        minX--;
        maxX++;
        minY--;
        maxY++;
        minZ--;
        maxZ++;

        for (int x = minX; x <= maxX; ++x)
        {
            for (int y = minY; y <= maxY; ++y)
            {
                for (int z = minZ; z <= maxZ; ++z)
                {
                    var cube = GetCubeAt(x, y, z, initialState);
                    var activeNeighbours = GetNeighbours(cube, initialState).Count(x => x.Active);

                    if ((!cube.Active && activeNeighbours == 3) || (cube.Active && ((activeNeighbours == 2 || activeNeighbours == 3))))
                    {
                        newState.Add((x, y, z), new Cube(x, y, z, true));
                    }
                    else
                    {
                        newState.Add((x, y, z), new Cube(x, y, z, false));
                    }
                }
            }
        }

        initialState = newState;
    }

    return newState.Values;
}

IEnumerable<Cube> GetNeighbours(Cube pt, Dictionary<(int X, int Y, int Z), Cube> initialState)
{
    for (int x = pt.X - 1; x <= pt.X + 1; ++x)
    {
        for (int y = pt.Y - 1; y <= pt.Y + 1; ++y)
        {
            for (int z = pt.Z - 1; z <= pt.Z + 1; ++z)
            {
                if ((x, y, z) != (pt.X, pt.Y, pt.Z))
                {
                    yield return GetCubeAt(x, y, z, initialState);
                }
            }
        }
    }
}

Cube GetCubeAt(int x, int y, int z, Dictionary<(int X, int Y, int Z), Cube> initialState)
{
    if (initialState.TryGetValue((x, y, z), out var cube))
    {
        return cube;
    }
    return new Cube(x, y, z, false);
}

record Cube(int X, int Y, int Z, bool Active);

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var tiles = ReadTiles("input.txt").ToList();
var allEdges = tiles.SelectMany(t => t.Edges().Select(e => (Id: t.Id, Edge: e )));
var sortedTiles = tiles.OrderBy(t => CountEdges(t, allEdges));
var cornersValue = sortedTiles.Take(4).Aggregate(1L, (seed, tile)=> seed * tile.Id);
Console.WriteLine($"Part 1: the result is {cornersValue}");


int CountEdges(Tile tile, IEnumerable<(int Id, char[] Edge)> allEdges)
{
    var edges = tile.Edges().ToArray();
    return edges.Sum(edge => allEdges.Count(e => Enumerable.SequenceEqual(e.Edge, edge)));
}

IEnumerable<Tile> ReadTiles(string fileName)
{
    var tiles = File.ReadAllText(fileName).Split("\n\n");
    foreach(var tile in tiles)
    {
        var lines = tile.Split("\n");
        var id = int.Parse(lines[0].Split(" :".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
        yield return new Tile(id, lines.Skip(1).Select(l => l.ToCharArray()).ToArray());
    }
}

record Tile(int Id, char[][]Values)
{
    public IEnumerable<char[]> Edges()
    {
        yield return Values[0];
        yield return Values[0].Reverse().ToArray();

        yield return Values[Values.Length - 1];
        yield return Values[Values.Length - 1].Reverse().ToArray();

        yield return Values.Select(v => v[0]).ToArray();
        yield return Values.Select(v => v[0]).Reverse().ToArray();

        yield return Values.Select(v => v[v.Length - 1]).ToArray();
        yield return Values.Select(v => v[v.Length - 1]).Reverse().ToArray();
    }
}
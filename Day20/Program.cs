using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var tiles = ReadTiles("input.txt").ToList();
var allEdges = tiles.SelectMany(t => t.Edges().Select(e => (Id: t.Id, Edge: e.Edge )));
var sortedTiles = tiles.Select(t => (Tile: t, MatchingEdges: CountEdges(t, allEdges))).OrderBy(t => t.MatchingEdges);
var cornersValue = sortedTiles.Take(4).Aggregate(1L, (seed, tile)=> seed * tile.Tile.Id);
Console.WriteLine($"Part 1: the result is {cornersValue}");

var image = CreateImage(sortedTiles);
for (int y = 0; y < image.GetLength(1); ++y)
{
    for (int x = 0; x < image.GetLength(0); ++x)
    {
        Console.Write(image[x, y]);
    }
    Console.WriteLine();
}

var roughness = FindRoughness(image);
Console.WriteLine($"Part 2: the water roughness is {roughness}");





int FindRoughness(char[,] image)
{
    var pattern = LoadPattern();

    var used = 0;
    foreach(var subPattern in TransformPattern(pattern))
    {
        used = FindUsed(image, subPattern).Count;
        if (used != 0)
            break;
    }

    int nonEmpty = 0;
    for (int x = 0; x < image.GetLength(0); ++x)
    {
        for (int y = 0; y < image.GetLength(1); ++y)
        {
            if (image[x, y] == '#')
            {
                nonEmpty++;
            }
        }
    }

    return nonEmpty - used;
}

IEnumerable<ISet<(int X, int Y)>> TransformPattern(ISet<(int X, int Y)> pattern)
{
    var patternWidth = pattern.Max(p => p.X);
    var patternHeight = pattern.Max(p => p.Y);

    yield return pattern;

    yield return pattern.Select(pt => (patternWidth - pt.X, pt.Y)).ToHashSet(); // flip x
    yield return pattern.Select(pt => (patternWidth - pt.X, patternHeight - pt.Y)).ToHashSet(); // flip x,y
    yield return pattern.Select(pt => (pt.X, patternHeight - pt.Y)).ToHashSet(); // flip y

    yield return pattern.Select(pt => (pt.Y, pt.X)).ToHashSet(); // rotate
    yield return pattern.Select(pt => (patternHeight - pt.Y, pt.X)).ToHashSet(); 
    yield return pattern.Select(pt => (patternHeight - pt.Y, patternWidth - pt.X)).ToHashSet();
    yield return pattern.Select(pt => (pt.Y, patternWidth - pt.X)).ToHashSet();
}

ISet<(int X, int Y)> FindUsed(char[,] image, ISet<(int X, int Y)> pattern)
{

    var used = new List<(int X, int Y)>();
    var patternWidth = pattern.Max(p => p.X);
    var patternHeight = pattern.Max(p => p.Y);

    for (int ox = 0; ox < image.GetLength(0) - patternWidth; ox++)
    {
        for (int oy = 0; oy < image.GetLength(1) - patternHeight; ++oy)
        {
            var points = pattern.Select(pt => (X: ox + pt.X, Y: oy + pt.Y));
            if (points.All(pt => image[pt.X, pt.Y] == '#'))
            {
                used.AddRange(points);
            }
        }
    }

    return used.ToHashSet();
}

ISet<(int X, int Y)> LoadPattern()
{
    return File.ReadAllLines("monster.txt").Select(l => l.ToCharArray())
        .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
        .Where(c => c.c == '#')
        .Select(c => (X: c.x, Y: c.y))
        .ToHashSet();
}

char[,] CreateImage(IOrderedEnumerable<(Tile Tile, int MatchingEdges)> sortedTiles)
{
    var tiles = ArrangeTiles(sortedTiles);
    var sz = tiles[0][0].Values.Length - 2;
    char[,] bitmap = new char[sz * tiles.Count, sz * tiles.Count];

    for (int yTile = 0; yTile < tiles.Count; ++yTile)
    {
        for (int xTile = 0; xTile < tiles[yTile].Length; ++xTile)
        {
            for (int yValue = 0; yValue < sz; ++yValue)
            {
                for (int xValue = 0; xValue < sz; ++xValue)
                {
                    bitmap[(xTile * sz) + xValue, (yTile * sz) + yValue] = tiles[yTile][xTile].Values[yValue + 1][xValue + 1];
                }
            }
        }
    }

    return bitmap;
}

IList<Tile[]> ArrangeTiles(IOrderedEnumerable<(Tile Tile, int MatchingEdges)> sortedTiles)
{
    var tileGroups = sortedTiles.GroupBy(t => t.MatchingEdges).ToArray();
    var oCorners = tileGroups[0].Select(g => g.Tile).ToList();
    var oEdges = tileGroups[1].Select(g => g.Tile).ToList();
    var oInside = tileGroups[2].Select(g => g.Tile).ToList();
    var widthHeight = oEdges.Count / 4 + 2;

    var transforms = new Stack<Side>();
    foreach(var side in Enum.GetValues<Side>())
    {
        if (side != Side.Left)
        {
            transforms.Push(side);
        }
    }

    var corner = oCorners[0];
    var originalCorner = corner;
    oCorners.Remove(corner);

    for (; ; )
    {
        var corners = oCorners.ToArray().ToList();
        var edges = oEdges.ToArray().ToList();
        var inside = oInside.ToArray().ToList();
        var availableTiles = corners.Union(edges).Union(inside);

        try
        {
            var result = new List<Tile[]>();
            var row = new List<Tile>();
            row.Add(corner);
            char[] rightEdge = row[0].RightEdge;

            for (int x = 1; x < widthHeight - 1; ++x)
            {
                var nextTile = FindTile(edges, rightEdge);
                row.Add(nextTile);
                rightEdge = nextTile.RightEdge;
            }
            row.Add(FindTile(corners, rightEdge));

            result.Add(row.ToArray());
            var lastRow = row;

            for (int y = 1; y < widthHeight - 1; ++y)
            {
                row = new List<Tile>();
                for (int x = 0; x < widthHeight; ++x)
                {
                    Tile nextTile;
                    if (x == 0)
                    {
                        nextTile = FindTile(edges, lastRow[0].BottomEdge);
                        nextTile = nextTile.TransformToLeft(Side.Top);
                    }
                    else if (x == widthHeight - 1)
                    {
                        nextTile = FindTile2D(edges, rightEdge, lastRow[x].BottomEdge);
                    }
                    else
                    {
                        nextTile = FindTile2D(inside, rightEdge, lastRow[x].BottomEdge);
                    }
                    row.Add(nextTile);
                    rightEdge = nextTile.RightEdge;
                }
                result.Add(row.ToArray());
                lastRow = row;
            }

            row = new List<Tile>();
            for (int x = 0; x < widthHeight; ++x)
            {
                Tile nextTile;
                if (x == 0)
                {
                    nextTile = FindTile(corners, lastRow[0].BottomEdge).TransformToLeft(Side.Top);
                }
                else
                {
                    nextTile = FindTile2D(availableTiles.ToList(), rightEdge, lastRow[x].BottomEdge);
                }
                row.Add(nextTile);
                rightEdge = nextTile.RightEdge;
            }
            result.Add(row.ToArray());

            return result.ToArray();
        }
        catch
        {
            corner = originalCorner.TransformToLeft(transforms.Pop());
        }
    }
}

Tile FindTile(List<Tile> tiles, char[] rightEdge)
{
    var nextTileAndEdge = tiles.SelectMany(t => t.Edges().Select(e => (Tile: t, Edge: e))).First(x => Enumerable.SequenceEqual(x.Edge.Edge, rightEdge));
    var nextTile = nextTileAndEdge.Tile.TransformToLeft(nextTileAndEdge.Edge.Side);
    tiles.RemoveAll(t => t.Id == nextTile.Id);
    return nextTile;
}

Tile FindTile2D(List<Tile> tiles, char[] rightEdge, char[] topEdge)
{
    var nextTiles = tiles.SelectMany(t => t.Edges().Select(e => (Tile: t, Edge: e)))
        .Where(x => Enumerable.SequenceEqual(x.Edge.Edge, rightEdge))
        .Select(x => x.Tile.TransformToLeft(x.Edge.Side));

    var nextTile = nextTiles
        .First(x => Enumerable.SequenceEqual(x.TopEdge, topEdge));

    tiles.RemoveAll(t => t.Id == nextTile.Id);
    return nextTile;
}

int CountEdges(Tile tile, IEnumerable<(int Id, char[] Edge)> allEdges)
{
    var edges = tile.Edges().ToArray();
    return edges.Sum(edge => allEdges.Count(e => Enumerable.SequenceEqual(e.Edge, edge.Edge)));
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

record Tile(int Id, char[][] Values)
{
    public IEnumerable<(Side Side, char[] Edge)> Edges()
    {
        yield return (Side.Top, TopEdge);
        yield return (Side.TopFlipped, Values[0].Reverse().ToArray());

        yield return (Side.Bottom, BottomEdge);
        yield return (Side.BottomFlipped, Values[Values.Length - 1].Reverse().ToArray());

        yield return (Side.Left, Values.Select(v => v[0]).ToArray());
        yield return (Side.LeftFlipped, Values.Select(v => v[0]).Reverse().ToArray());

        yield return (Side.Right, RightEdge);
        yield return (Side.RightFlipped, Values.Select(v => v[v.Length - 1]).Reverse().ToArray());
    }

    public char[] RightEdge => Values.Select(v => v[v.Length - 1]).ToArray();
    public char[] BottomEdge => Values[Values.Length - 1];
    public char[] TopEdge => Values[0];

    public Tile TransformToLeft(Side sideThatShouldBeLeft) =>
        sideThatShouldBeLeft switch
        {
            Side.Top => SwapXY(),
            Side.TopFlipped => FlipX().SwapXY(),
            Side.Bottom => FlipY().SwapXY(),
            Side.BottomFlipped => FlipY().FlipX().SwapXY(),
            Side.LeftFlipped => FlipY(),
            Side.Right => FlipX(),
            Side.RightFlipped => FlipX().FlipY(),
            _ => this,
        };

    public Tile TransformToTop(Side sideThatShouldBeLeft) =>
        sideThatShouldBeLeft switch
        {
            Side.TopFlipped => FlipX(),
            Side.Bottom => FlipY(),
            Side.BottomFlipped => FlipY().FlipX(),
            Side.Left => SwapXY().FlipX(),
            Side.LeftFlipped => SwapXY(),
            Side.Right => FlipX().SwapXY(),
            Side.RightFlipped => FlipX().SwapXY().FlipX(),
            _ => this,
        };

    private Tile SwapXY()
    {
        var newValues = new List<char[]>();

        for(int x = 0; x<Values[0].Length;++x)
        {
            newValues.Add(Values.Select(v => v[x]).ToArray());
        }

        return this with
        {
            Values = newValues.ToArray()
        };
    }

    private Tile FlipY() => 
        this with
        {
            Values = Values.Reverse().ToArray()
        };

    private Tile FlipX() => 
        this with
        {
            Values = Values.Select(v => v.Reverse().ToArray()).ToArray()
        };

    public override string ToString()
    {
        return String.Join("\r\n", Values.Select(v => new string(v)));
    }
}

enum Side
{
    Top,
    TopFlipped,
    Bottom,
    BottomFlipped,
    Left,
    LeftFlipped,
    Right, 
    RightFlipped
}
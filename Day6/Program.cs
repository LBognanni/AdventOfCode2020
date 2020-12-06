using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var groups = ReadGroupAnswers(lines);

            var part1Count = groups.Select(g => g.Distinct().Count()).Sum();

            Console.WriteLine($"Part 1: The total count is {part1Count}");

            var exampleLines = new[]
            {
                "abc",
                "",
                "a",
                "b",
                "c",
                "",
                "ab",
                "ac",
                "",
                "a",
                "a",
                "a",
                "a",
                "",
                "b",
            };


            Console.WriteLine($"Example count is {ReadGroupAnswersPart2(exampleLines).Sum()}");

            var part2Count = ReadGroupAnswersPart2(lines).Sum();

            Console.WriteLine($"Part 2: The total count is {part2Count}");
        }

        private static IEnumerable<int> ReadGroupAnswersPart2(string[] lines)
        {
            var allAnswers = new List<char>();
            var newGroup = true;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return allAnswers.Count;
                    allAnswers = new List<char>();
                    newGroup = true;
                    continue;
                }

                if (newGroup)
                {
                    newGroup = false;
                    allAnswers.AddRange(line.ToCharArray());
                }
                else
                {
                    allAnswers = allAnswers.Intersect(line).ToList();
                }
            }
            if (allAnswers.Any())
            {
                yield return allAnswers.Count;
            }
        }

        private static IEnumerable<char[]> ReadGroupAnswers(string[] lines)
        {
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return sb.ToString().ToCharArray();
                    sb = new StringBuilder();
                    continue;
                }
                sb.Append(line);
            }
            if (sb.Length > 0)
            {
                yield return sb.ToString().ToCharArray();
            }
        }
    }
}

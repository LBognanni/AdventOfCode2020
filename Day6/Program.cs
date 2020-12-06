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

            var count = groups.Select(g => g.Distinct().Count()).Sum();

            Console.WriteLine($"The total count is {count}");
        }

        private static IEnumerable<char[]> ReadGroupAnswers(string[] lines)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line))
                {
                    yield return sb.ToString().ToCharArray();
                    sb = new StringBuilder();
                    continue;
                }
                sb.Append(line);
            }
            if(sb.Length>0)
            {
                yield return sb.ToString().ToCharArray();
            }
        }
    }
}

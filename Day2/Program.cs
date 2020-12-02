using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var rgx = new Regex(@"^([0-9]+)\-([0-9]+) ([a-z]): ([a-z]+)$");
            var validPasswords = 0;

            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var match = rgx.Match(line);
                if(!match.Success)
                {
                    Console.WriteLine($"Can't parse line {line} :(");
                    continue;
                }

                var minChars = int.Parse(match.Groups[1].Value);
                var maxChars = int.Parse(match.Groups[2].Value);
                var find = match.Groups[3].Value[0];
                var password = match.Groups[4].Value;
                var effective  = password.ToCharArray().Count(c => c == find);
                
                if ((effective >= minChars) && (effective <= maxChars))
                {
                    validPasswords++;
                }
            }

            Console.WriteLine($"{validPasswords} valid passwords found.");
        }
    }
}

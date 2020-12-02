using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {

        static int CountValidPasswords(string[] lines, Func<int,int,char,string, bool> rule)
        {
            var rgx = new Regex(@"^([0-9]+)\-([0-9]+) ([a-z]): ([a-z]+)$");
            var validPasswords = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var match = rgx.Match(line);
                if (!match.Success)
                {
                    Console.WriteLine($"Can't parse line {line} :(");
                    continue;
                }

                var minChars = int.Parse(match.Groups[1].Value);
                var maxChars = int.Parse(match.Groups[2].Value);
                var find = match.Groups[3].Value[0];
                var password = match.Groups[4].Value;

                if(rule(minChars, maxChars, find, password))
                {
                    validPasswords++;
                }
            }

            return validPasswords;
        }

        static bool Rule1(int minChars, int maxChars, char find, string password)
        {
            var effective = password.ToCharArray().Count(c => c == find);

            if ((effective >= minChars) && (effective <= maxChars))
            {
                return true;
            }
            return false;
        }

        static bool Rule2(int index1, int index2, char find, string password) => 
            password[index1 - 1] == find ^ password[index2 - 1] == find;

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var validStep1 = CountValidPasswords(lines, Rule1);

            Console.WriteLine($"Step 1: {validStep1} valid passwords found.");

            var validStep2 = CountValidPasswords(lines, Rule2);

            Console.WriteLine($"Step 1: {validStep2} valid passwords found.");
        }
    }
}

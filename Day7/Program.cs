using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day7
{
    class Program
    {
        record Rule(string Color, IEnumerable<SubRule> SubRules);
        record SubRule(string Color, int Quantity);

        static void Main(string[] args)
        {
            var rules = ReadRules().ToArray();
            var combinations = FindCombinations(rules, "shiny gold").Distinct().Count();
            Console.WriteLine($"Part 1: Found {combinations} combinations.");
        }

        private static IEnumerable<string> FindCombinations(Rule[] rules, params string[] find)
        {
            var found = rules.Where(r => r.SubRules.Any(sr => find.Contains(sr.Color))).Select(sr => sr.Color).ToArray();
            if(found.Any())
            {
                return found.Union(FindCombinations(rules, found));
            }
            return Array.Empty<string>();
        }

        private static IEnumerable<Rule> ReadRules()
        {
            var lines = File.ReadAllLines("input.txt");
            var bagNameRegx = new Regex(@"^(?<color>[a-z]+ [a-z]+) (bags|bag)\.*$");
            var bagNameQtyRegx = new Regex(@"^(?<qty>[0-9]+) (?<color>[a-z]+ [a-z]+) (bags|bag)\.*$");


            foreach (var line in lines)
            {
                var split1 = line.Split(" contain ", StringSplitOptions.RemoveEmptyEntries);
                var container = bagNameRegx.Match(split1[0]).Groups["color"].Value;
                if (split1[1].Equals("no other bags."))
                {
                    yield return new Rule(container, Array.Empty<SubRule>());
                    continue;
                }

                var contained = split1[1].Split(", ");
                
                var subRules = new List<SubRule>();
                foreach(var s in contained)
                {
                    var match = bagNameQtyRegx.Match(s);
                    subRules.Add(new SubRule(match.Groups["color"].Value, int.Parse(match.Groups["qty"].Value)));
                }
                yield return new Rule(container, subRules);
            }
        }
    }
}

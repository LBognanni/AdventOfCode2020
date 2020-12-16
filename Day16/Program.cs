using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    public class Program
    {
        record Rule (string Field, int Range1Min, int Range1Max, int Range2Min, int Range2Max)
        {
            public static Rule Parse(string text)
            {
                var portions = text.Split(new[] { ": ", " or ", "-"}, StringSplitOptions.RemoveEmptyEntries);
                return new Rule(portions[0], int.Parse(portions[1]), int.Parse(portions[2]), int.Parse(portions[3]), int.Parse(portions[4]));
            }

            private static bool IsBetween(int value, int min, int max) => value >= min && value <= max;

            public bool Accepts(int value) => IsBetween(value, Range1Min, Range1Max) || IsBetween(value, Range2Min, Range2Max);
        }

        static void Main(string[] args)
        {
            var (rules, myTicket, otherTickets) = ParseFile(File.ReadAllLines("input.txt"));

            Console.WriteLine($"{rules.Count()} rules, myticket has {myTicket.Length} fields, {otherTickets.Count()} other tickets");
            var invalid = FindInvalidValues(rules, otherTickets);

            Console.WriteLine($"Part 1: The result is {invalid.Sum()}");
        }

        private static IEnumerable<int> FindInvalidValues(IEnumerable<Rule> rules, IEnumerable<int[]> otherTickets)
        {
            foreach(var ticket in otherTickets)
            {
                foreach(var number in ticket)
                {
                    bool found = false;
                    foreach(var rule in rules)
                    {
                        if(rule.Accepts(number))
                        {
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        yield return number;
                    }
                }
            }
        }

        private static (IEnumerable<Rule> rules, int[]mine, IEnumerable<int[]>others) ParseFile(string []lines)
        {
            var rules = new List<Rule>();
            var others = new List<int[]>();
            int[] mine = null;

            int section = 1;
            
            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line == "your ticket:")
                {
                    section = 2;
                    continue;
                }
                if(line == "nearby tickets:")
                {
                    section = 3;
                    continue;
                }

                switch(section)
                {
                    case 1:
                        rules.Add(Rule.Parse(line));
                        break;
                    case 2:
                        mine = line.Split(",").Select(int.Parse).ToArray();
                        break;
                    case 3:
                        others.Add(line.Split(",").Select(int.Parse).ToArray());
                        break;
                }
            }


            return (rules, mine, others);
        }
    }
}

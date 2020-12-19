using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var (rules, lines) = ExtractRules(File.ReadAllLines("input.txt"));
            var rulesDictionary = rules.ToDictionary(r => r.Id);

            var regex = new Regex("^" + rulesDictionary[0].ToString(rulesDictionary) + "$");
            var count = lines.Count(regex.IsMatch);
            Console.WriteLine($"Part 1: {count} lines match rule 0.");

            //--------

            rulesDictionary[8] = new OrRules(8, new ReferenceRule(-1, new[] { 42 }), new ReferenceRule(-1, new[] { 42, 8 }));
            rulesDictionary[11] = new OrRules(11, new ReferenceRule(-1, new[] { 42, 31 }), new ReferenceRule(-1, new[] { 42, 11, 31}));

            regex = new Regex("^" + rulesDictionary[0].ToString(rulesDictionary) + "$");
            count = lines.Count(regex.IsMatch);
            Console.WriteLine($"Part 2: {count} lines match rule 0.");
        }

        abstract class Rule
        {
            public int Id { get; }

            public Rule(int id)
            {
                Id = id;
            }

            public abstract string ToString(IReadOnlyDictionary<int, Rule> rules);
        }

        class ValueRule : Rule
        {
            public ValueRule(int id, string value) : base(id)
            {
                Value = value;
            }

            public string Value { get; }

            public override string ToString(IReadOnlyDictionary<int, Rule> rules) => Value;
        }

        class ReferenceRule : Rule
        {
            public int[] RuleIds { get; }

            public ReferenceRule(int id, int[] ruleIds) : base(id)
            {
                RuleIds = ruleIds;
            }

            static Stack<int> SeenRuleIds = new Stack<int>();

            public override string ToString(IReadOnlyDictionary<int, Rule> rules)
            {
                var sb = new StringBuilder();

                foreach (var rule in RuleIds.Select(id=>rules[id]))
                {
                    if (SeenRuleIds.Count(r => r == rule.Id) > 20)
                        continue;

                    SeenRuleIds.Push(rule.Id);
                    sb.Append(rule.ToString(rules));
                    SeenRuleIds.Pop();
                }

                return sb.ToString();
            }
        }

        class OrRules : Rule
        {
            public OrRules(int id, Rule left, Rule right) : base(id)
            {
                Left = left;
                Right = right;
            }

            public Rule Left { get; }
            public Rule Right { get; }

            public override string ToString(IReadOnlyDictionary<int, Rule> rules) => $"({Left.ToString(rules)}|{Right.ToString(rules)})";
        }

        private static (IEnumerable<Rule>, IEnumerable<string>) ExtractRules(string[] lines)
        {
            var messages = new List<string>();
            var rules = new List<Rule>();

            foreach(var line in lines)
            {
                if(line.Contains(":"))
                {
                    var spl = line.Split(":");
                    int id = int.Parse(spl[0]);
                    rules.Add(ParseRule(id, spl[1]));
                }
                else if(!string.IsNullOrWhiteSpace(line))
                {
                    messages.Add(line);
                }
            }

            return (rules, messages);
        }

        private static Rule ParseRule(int id, string rule)
        {
            if (rule.Contains("|"))
            {
                var lr = rule.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return new OrRules(id, ParseRule(-1, lr[0]), ParseRule(-1, lr[1]));
            }

            if(rule.Contains("\""))
            {
                return new ValueRule(id, rule.Replace("\"", "").Trim());
            }

            return new ReferenceRule(id, rule.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
        }
    }
}

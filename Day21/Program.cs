using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Day20
{
    public static class Program
    {

        record Label(string[] Ingredients, string[] Allergens);

        public static void Main(string[] args)
        {
            var lines = ParseFile("input.txt");
            var combinations = lines.SelectMany(l => l.Allergens.SelectMany(a => l.Ingredients.Select(i => (Ingredient: i, Allergen: a))));
            var groups = combinations.GroupBy(c => c.Allergen);
            
            var allergensMap = new Dictionary<string, string>();
            foreach(var group in groups)
            {
                var mostCommonIngredients = group.GroupBy(g => g.Ingredient).OrderByDescending(g => g.Count());
                foreach(var ingredient in mostCommonIngredients)
                {
                    if(!allergensMap.ContainsKey(ingredient.Key))
                    {
                        allergensMap.Add(ingredient.Key, group.Key);
                        break;
                    }
                }
            }

            var nonAllergens = lines.Sum(l => l.Ingredients.Count(i => !allergensMap.ContainsKey(i)));
            Console.WriteLine($"Part 1: safe ingredients appear {nonAllergens} times");
        }

        static IEnumerable<Label> ParseFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var regex = new Regex(@"^(?<ingredients>[\w ]+)\(contains (?<allergens>[\w, ]+)\)$");

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var match = regex.Match(line);
                yield return new Label(
                    match.Groups["ingredients"].Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                    match.Groups["allergens"].Value.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    );
            }
        }
    }
}

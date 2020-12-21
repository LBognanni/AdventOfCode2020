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
            var allergensMap = CalculateAllergensMap(lines);

            var nonAllergens = lines.Sum(l => l.Ingredients.Count(i => !allergensMap.ContainsKey(i)));
            Console.WriteLine($"Part 1: safe ingredients appear {nonAllergens} times");

            Console.WriteLine();
            foreach (var kvp in allergensMap.OrderBy(kv => kv.Value))
            {
                Console.WriteLine($"{kvp.Key} contains {kvp.Value}");
            }
            Console.WriteLine();

            var dangerousIngredients = string.Join(",", allergensMap.OrderBy(kv => kv.Value).Select(kv => kv.Key));
            Console.WriteLine($"Part 2: dangerous ingredients: `{dangerousIngredients}`");

        }

        private static IDictionary<string, string> CalculateAllergensMap(IEnumerable<Label> lines)
        {
            var combinations = lines.SelectMany(l => l.Allergens.SelectMany(a => l.Ingredients.Select(i => (Ingredient: i, Allergen: a))));
                var byAllergen = combinations.GroupBy(c => c.Allergen);

            var allergensMap = new Dictionary<string, string>();
            var combinationsCount = new List<(string Ingredient, string Allergen, int Count)>();

            foreach (var allergenIngredients in byAllergen.OrderByDescending(g => g.Count()))
            {
                var mostCommonIngredients = allergenIngredients.GroupBy(g => g.Ingredient).Select(g => new { Ingredient = g.Key, Count = g.Count() }).OrderByDescending(g => g.Count);
                int maxCount = 0;
                foreach (var ingredient in mostCommonIngredients)
                {
                    if (maxCount == 0)
                    {
                        maxCount = ingredient.Count;
                    }
                    if (maxCount != ingredient.Count)
                    {
                        break;
                    }
                    combinationsCount.Add((ingredient.Ingredient, allergenIngredients.Key, ingredient.Count));
                }
            }

            var groups2 = combinationsCount.GroupBy(l => l.Allergen);
            foreach (var kvp in groups2.Where(g2 => g2.Count() == 1))
            {
                allergensMap.Add(kvp.First().Ingredient, kvp.Key);
            }
            foreach (var group in groups2)
            {
                if (allergensMap.Values.Contains(group.Key))
                    continue;
                foreach (var ingredient in group.Select(g => new { g.Ingredient, Count = g.Count - combinationsCount.Count(l => l.Ingredient == g.Ingredient) }).OrderByDescending(l => l.Count))
                {
                    if (allergensMap.ContainsKey(ingredient.Ingredient))
                        continue;
                    allergensMap.Add(ingredient.Ingredient, group.Key);
                    break;
                }
            }

            return allergensMap;
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

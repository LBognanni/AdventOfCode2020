using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {

        static int GetNumberOfValidPassports()
        {
            var lines = File.ReadAllLines("input.txt");
            int validPassports = 0;
            int currentPassportFields = 0;

            var requiredFields = new Dictionary<string, Func<string, bool>>
            {
                ["byr"] = (s) => NumberBetween(s, 1920, 2002),
                ["iyr"] = (s) => NumberBetween(s, 2010, 2020),
                ["eyr"] = (s) => NumberBetween(s, 2020, 2030),
                ["hgt"] = CheckHeight,
                ["hcl"] = (s) => Regex.IsMatch(s, @"^#([0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f])$"),
                ["ecl"] = (s) => Regex.IsMatch(s, @"^(amb|blu|brn|gry|grn|hzl|oth)$"),
                ["pid"] = (s) => Regex.IsMatch(s, @"^([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$"),
            };

            void UpdateCount()
            {
                if (currentPassportFields == requiredFields.Count)
                {
                    validPassports++;
                }
                currentPassportFields = 0;
            }

            foreach (var line in lines)
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    UpdateCount();
                    continue;
                }
                var fieldsAndValues = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach(var fieldWithValue in fieldsAndValues)
                {
                    var kvp = fieldWithValue.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if(requiredFields.ContainsKey(kvp[0]))
                    {
                        if (requiredFields[kvp[0]](kvp[1]))
                        {
                            currentPassportFields++;
                        }
                    }
                }
            }
            UpdateCount();

            return validPassports;
        }

        private static bool CheckHeight(string s)
        {
            if(s.EndsWith("in"))
            {
                return NumberBetween(s.Remove(s.Length - 2, 2), 59, 76);
            }
            else if(s.EndsWith("cm"))
            {
                return NumberBetween(s.Remove(s.Length - 2, 2), 150, 193);
            }
            return false;
        }

        private static bool NumberBetween(string s, int min, int max)
        {
            if(int.TryParse(s, out int num))
            {
                return num >= min && num <= max;
            }
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Found {GetNumberOfValidPassports()} valid passports.");
        }
    }
}

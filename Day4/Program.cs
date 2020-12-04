using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {

        static int GetNumberOfValidPassports()
        {
            var lines = File.ReadAllLines("input.txt");
            int validPassports = 0;
            int currentPassportFields = 0;

            string[] requiredFields = new[]
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid",
            };

            void UpdateCount()
            {
                if (currentPassportFields == requiredFields.Length)
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
                    var split = fieldWithValue.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if(requiredFields.Contains(split[0]))
                    {
                        currentPassportFields++;
                    }
                }
            }
            UpdateCount();

            return validPassports;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Found {GetNumberOfValidPassports()} valid passports.");
        }
    }
}

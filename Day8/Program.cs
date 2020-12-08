using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day8
{
    class Program
    {
        record Instruction (string Type, int Offset);


        static void Main(string[] args)
        {
            var instructions = ParseInstructions("input.txt").ToArray();
            var value = RunUntilRepeat(instructions).acc;
            Console.WriteLine($"Part 1: Accumulator value is {value}");

            var value2 = BruteForce(instructions);
            Console.WriteLine($"Part 2: Accumulator value is {value2}");
        }

        private static int BruteForce(Instruction[] instructions)
        {
            for (int i = instructions.Length - 2; i >= 0; --i)
            {
                if (instructions[i].Type == "acc")
                    continue;
                var result = ReplaceAndRun(instructions, i);
                if (!result.repeated)
                    return result.acc;
            }

            return -1;
        }

        private static (int acc, bool repeated) ReplaceAndRun(Instruction[] instructions, int i)
        {
            var orig = instructions[i].Type;
            var swap = orig == "jmp" ? "nop" : "jmp";
            instructions[i] = instructions[i] with { Type = swap };
            var result = RunUntilRepeat(instructions);
            instructions[i] = instructions[i] with { Type = orig };
            return result;
        }

        private static (int acc, bool repeated) RunUntilRepeat(Instruction[] instructions)
        {
            int acc = 0;
            int instructionIdx = 0;
            List<int> visitedIndices = new List<int>();

            while (instructionIdx < instructions.Length)
            {
                if (visitedIndices.Contains(instructionIdx))
                    return (acc, true);

                var inst = instructions[instructionIdx];
                visitedIndices.Add(instructionIdx);

                switch (inst.Type)
                {
                    case "acc":
                        acc += inst.Offset;
                        instructionIdx++;
                        break;
                    case "jmp":
                        instructionIdx += inst.Offset;
                        break;
                    default:
                        instructionIdx++;
                        break;
                }
            }

            return (acc, false);
        }

        private static IEnumerable<Instruction> ParseInstructions(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var regex = new Regex(@"(?<instruction>\w+) (?<offset>[-+0-9]+)");

            foreach(var line in lines)
            {
                var match = regex.Match(line);
                yield return new Instruction(match.Groups["instruction"].Value, int.Parse(match.Groups["offset"].Value));
            }
        }
    }
}

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
            var value = RunUntilRepeat(instructions);
            Console.WriteLine($"Part 1: Accumulator value is {value}");
        }

        private static int RunUntilRepeat(Instruction[] instructions)
        {
            int acc = 0;
            int instructionIdx = 0;
            List<int> visitedIndices = new List<int>();

            for(; ;)
            {
                if (visitedIndices.Contains(instructionIdx))
                    return acc;

                var inst = instructions[instructionIdx];
                visitedIndices.Add(instructionIdx);

                switch(inst.Type)
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

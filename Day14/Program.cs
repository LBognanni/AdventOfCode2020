using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        record Instruction(string Type, int Address, string Value)
        {
            public static Instruction Parse(string s)
            {
                var spl = s.Split(" = ");
                if(spl[0] == "mask")
                {
                    return new Instruction(spl[0], 0, spl[1]);
                }
                else
                {
                    var instr = spl[0].Split("[]".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return new Instruction(instr[0], int.Parse(instr[1]), spl[1]);
                }
            }
        }

        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt").Select(Instruction.Parse).ToArray();
            //var instructions = "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X\nmem[8] = 11\nmem[7] = 101\nmem[8] = 0".Split('\n').Select(Instruction.Parse).ToArray();


            var result = RunInstructions(instructions);
            Console.WriteLine($"Part 1: The sum is {result}");
        }

        private static long RunInstructions(Instruction[] instructions)
        {
            long AndMask = long.MaxValue;
            long OrMask = 0;
            var memory = new Dictionary<int, long>();


            foreach(var instruction in instructions)
            {
                if(instruction.Type == "mask")
                {
                    (AndMask, OrMask) = UpdateMask(instruction.Value);
                }
                else
                {
                    memory[instruction.Address] = (long.Parse(instruction.Value) | OrMask) & AndMask;
                }
            }

            return memory.Values.Sum();
        }

        private static (long AndMask, long OrMask) UpdateMask(string value)
        {
            var or = Convert.ToInt64(value.Replace('X', '0'), 2);
            var and = Convert.ToInt64(value.Replace('X', '1'), 2);
            return (and, or);
        }

    }
}

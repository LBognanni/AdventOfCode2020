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
            
            var result = RunInstructions(instructions);
            Console.WriteLine($"Part 1: The sum is {result}");

            result = RunInstructionsV2(instructions);
            Console.WriteLine($"Part 2: The sum is {result}");

        }

        private static long RunInstructionsV2(Instruction[] instructions)
        {
            var mem = new Dictionary<long, long>();
            string mask = "0";

            foreach(var instruction in instructions)
            {
                if (instruction.Type == "mask")
                {
                    mask = instruction.Value;
                }
                else
                {
                    foreach (var addr in GetAddresses(mask, instruction.Address))
                    {
                        mem[addr] = long.Parse(instruction.Value);
                    }
                }
            }

            return mem.Values.Sum();
        }

        private static IEnumerable<long> GetAddresses(string template, long startAddr)
        {
            foreach (var mask in EnumerateMasks(template.Replace('0', 'O')))
            {
                var or = Convert.ToInt64(mask.Replace('O', '0'), 2);
                var and = Convert.ToInt64(mask.Replace('O', '1'), 2);
                yield return (startAddr | or) & and;
            }
        }

        private static IEnumerable<string> EnumerateMasks(string mask)
        {
            var i = mask.IndexOf('X');
            if (i < 0)
            {
                yield return mask;
            }
            else
            {
                var with0 = mask[..i] + "0" + mask[(i + 1)..];
                foreach (var combination in EnumerateMasks(with0))
                {
                    yield return combination;
                }

                var with1 = mask[..i] + "1" + mask[(i + 1)..];
                foreach (var combination in EnumerateMasks(with1))
                {
                    yield return combination;
                }
            }
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

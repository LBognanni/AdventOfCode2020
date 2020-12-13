using System;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "939\n7,13,x,x,59,x,31,19".Split("\n");
            var input = File.ReadAllLines("input.txt");
            
            var result = Part1(input);
            Console.WriteLine($"Part 1: The answer is {result}");

            var timestamp = Part2(input[1]);
            Console.WriteLine($"Part 2: The answer is {timestamp}");

        }

        private static int Part1(string[] input)
        {
            var myTimeStamp = int.Parse(input[0]);
            var busses = input[1].Split(",x".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var busTimes = busses.Select(b => GetNearestTime(b, myTimeStamp));
            var closestBus = busTimes.OrderBy(bt => bt.time).First();
            var result = (closestBus.time - myTimeStamp) * closestBus.bus;
            return result;
        }

        private static (int bus, int time) GetNearestTime(int b, int myTimeStamp)
        {
            var inc = b;
            while (b <= myTimeStamp)
            {
                b += inc;
            }
            return (inc, b);
        }
        private static ulong Part2(string busTable)
        {
            var busses = busTable.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select((b, i) => (Bus: b, Index: i))
                .Where(x => x.Bus != "x")
                .Select(x => (Bus: ulong.Parse(x.Bus), Offset: (ulong)x.Index))
                .OrderBy(x => x.Bus)
                .ToArray();


            var step =  busses[0].Bus;
            ulong time =step;

            foreach (var (Bus, Offset) in busses.Skip(1))
            {
                while ((time + Offset) % Bus != 0)
                {
                    time += step;
                }
                step *= Bus;
            }

            return time;
        }
    }
}

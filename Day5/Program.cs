using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        static (int row, int seat) DecodeBoardingPass(string boardingPass)
        {
            (int min, int len) rowSpace = (0, 128);
            (int min, int len) seatSpace = (0, 8);

            foreach (char c in boardingPass)
            {
                switch(c)
                {
                    case 'F':
                        rowSpace = (rowSpace.min, rowSpace.len / 2);
                        break;
                    case 'B': 
                        rowSpace = (rowSpace.min + rowSpace.len / 2, rowSpace.len / 2);
                        break;
                    case 'L':
                        seatSpace = (seatSpace.min, seatSpace.len / 2);
                        break;
                    case 'R':
                        seatSpace = (seatSpace.min + seatSpace.len / 2, seatSpace.len / 2);
                        break;
                }
            }

            return (rowSpace.min + rowSpace.len - 1, seatSpace.min + seatSpace.len -1);
        }

        static int RowId((int row, int seat) point) => point.row * 8 + point.seat;

        static void Main(string[] args)
        {
            var exampleRow = DecodeBoardingPass("FBFBBFFRLR");
            Console.WriteLine($"Example row: {exampleRow} (should be 44,5?)");

            var samples = new Dictionary<string, int>
            {
                ["BFFFBBFRRR"] = 567,
                ["FFFBBBFRRR"] = 119,
                ["BBFFBBFRLL"] = 820,
            };

            foreach(var sample in samples)
            {
                var result = RowId(DecodeBoardingPass(sample.Key));
                if(sample.Value != result)
                {
                    throw new Exception($"wrong result: {sample.Key} should be {sample.Value} but was {result}");
                }
            }

            var seats = File.ReadAllLines("input.txt").Select(l => RowId(DecodeBoardingPass(l))).ToList();
            var step1Result = seats.Max();
            Console.WriteLine($"Step 1 result is {step1Result}");

            var freeSeats = Enumerable.Range(0, 128 * 8).Where(seat => !seats.Contains(seat)).ToArray();
            var mySeat = FindMySeat(freeSeats);
            Console.WriteLine($"My seat is one of { String.Join(", ", freeSeats)}");
            Console.WriteLine($"My seat is { FindMySeat(freeSeats) }");


        }

        private static int FindMySeat(int[] freeSeats)
        {
            int lastSeat = 0;
            foreach(var seat in freeSeats)
            {
                if (seat > (lastSeat + 1))
                    return seat;
                lastSeat = seat;
            }

            return -1;
        }
    }
}

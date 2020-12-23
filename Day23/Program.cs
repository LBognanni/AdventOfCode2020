using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            //var arr = new List<int> { 3, 8, 9, 1, 2, 5, 4, 6, 7 };
            var arr = new List<int> { 6,1,4,7,5,2,8,3,9 };
            for(int i=0;i<100;++i)
            {
                arr = Play(arr, i);
            }

            Console.WriteLine($"Part 1: {String.Join("", ShiftList(arr))}");
        }

        private static List<int> Play(List<int> arr, int current)
        {
            var newList = new List<int>(arr.Count);
            current = current % arr.Count;

            if (current >= 6)
            {
                var skip = current - 5;
                newList.AddRange(arr.Skip(skip).Take(6));
                newList.AddRange(arr.Skip(current + 4));
            }
            else
            {
                newList.AddRange(arr.Take(current + 1));
                newList.AddRange(arr.Skip(current + 4));
            }

            var destination = -1;
            var destinationValue = arr[current];
            while (destination == -1) 
            {
                destinationValue--;
                if (destinationValue == 0)
                    destinationValue = 9;
                destination = newList.IndexOf(destinationValue);
            }

            for (int i = 1; i <= 3; ++i)
            {
                newList.Insert(destination + i, arr[(current + i) % arr.Count]);
            }

            int shift = newList.IndexOf(arr[current]) - current;
            if(shift > 0)
            {
                newList.AddRange(newList.Take(shift));
                newList.RemoveRange(0, shift);
            }

            
            return newList;
        }

        static IEnumerable<int> ShiftList(List<int> list)
        {
            int shift = list.IndexOf(1);
            if (shift > 0)
            {
                return list.Skip(shift + 1).Union(list.Take(shift));
            }
            return list.Skip(1);
        }
    }
}

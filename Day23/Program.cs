using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new List<long> { 6, 1, 4, 7, 5, 2, 8, 3, 9 };

            for (int i = 0; i < 100; ++i)
            {
                arr = Play(arr, i);
            }

            Console.WriteLine($"Part 1: {String.Join("", ShiftList(arr))}");

            
            var (dict, list) = GenerateLinkedList(new long[] { 6, 1, 4, 7, 5, 2, 8, 3, 9 }, 1000000);
            for (long i = 0; i < 10000000; ++i)
            {
                PlayV2(dict, list, 1000000);
            }
            Console.WriteLine($"Part 2: the result is {dict[1].Next.Value * dict[1].Next.Next.Value}");
        }

        private static void PlayV2(IReadOnlyDictionary<long, LinkedListNode<long>> dict, LinkedList<long> list, long maxNumber)
        {
            var current = list.First;
            var nextThree = new Queue<LinkedListNode<long>>();
            nextThree.Enqueue(current.Next);
            nextThree.Enqueue(current.Next.Next);
            nextThree.Enqueue(current.Next.Next.Next);
            
            foreach(var node in nextThree)
            {
                list.Remove(node);
            }

            var destinationNumber = current.Value - 1;
            if (destinationNumber == 0)
            {
                destinationNumber = maxNumber;
            }
            while (nextThree.Any(x=>x.Value == destinationNumber))
            {
                destinationNumber--;
                if(destinationNumber == 0)
                {
                    destinationNumber = maxNumber;
                }
            }

            var addAfter = dict[destinationNumber];
            while(nextThree.Any())
            {
                var next = nextThree.Dequeue();
                list.AddAfter(addAfter, next);
                addAfter = next;
            }

            list.Remove(current);
            list.AddLast(current);
        }


        private static (IReadOnlyDictionary<long, LinkedListNode<long>>, LinkedList<long>) GenerateLinkedList(IEnumerable<long> seed, long maxNumber)
        {
            var dict = new Dictionary<long, LinkedListNode<long>>();
            var list = new LinkedList<long>();
            var max = seed.Max();

            foreach(var i in seed)
            {
                dict.Add(i, list.AddLast(i));
            }

            for (long i = max + 1; i <= maxNumber; ++i)
            {
                dict.Add(i, list.AddLast(i));
            }

            return (dict, list);
        }

        private static List<long> Play(List<long> arr, int current)
        {
            var newList = new List<long>(arr.Count);
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

            var shift = newList.IndexOf(arr[current]) - current;
            if(shift > 0)
            {
                newList.AddRange(newList.Take(shift));
                newList.RemoveRange(0, shift);
            }

            
            return newList;
        }

        static IEnumerable<long> ShiftList(List<long> list)
        {
            var shift = list.IndexOf(1);
            if (shift > 0)
            {
                return list.Skip(shift + 1).Union(list.Take(shift));
            }
            return list.Skip(1);
        }
    }
}

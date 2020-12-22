using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var decks = ReadDecks("input.txt");
            var finalDeck = PlayUntilCompletion(decks);
            var finalScore = CalculateScore(finalDeck);
            Console.WriteLine($"Part 1: the final score is {finalScore}");
        }

        private static object CalculateScore(IEnumerable<int> finalDeck)
        {
            int i = -1;
            var deck = finalDeck.ToList();
            return deck.Aggregate(0, (acc, n) =>
            {
                ++i;
                return acc + (n * (deck.Count - i));
            });
        }

        private static IEnumerable<int> PlayUntilCompletion(Queue<int>[] decks)
        {
            while (decks[0].Count > 0 && decks[1].Count > 0)
            {
                var card1 = decks[0].Dequeue();
                var card2 = decks[1].Dequeue();
                if(card1>card2)
                {
                    decks[0].Enqueue(card1);
                    decks[0].Enqueue(card2);
                }
                else
                {
                    decks[1].Enqueue(card2);
                    decks[1].Enqueue(card1);
                }
            }

            if (decks[0].Any())
                return decks[0];
            return decks[1];
        }

        private static Queue<int>[] ReadDecks(string fileName)
        {
            var decks = File.ReadAllText(fileName).Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return new[]
            {
                new Queue<int>(decks[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse)),
                new Queue<int>(decks[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse)),
            };
        }
    }
}

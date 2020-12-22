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
            var finalDeck = PlayUntilCompletion(decks.Select(d => new Queue<int>(d)).ToArray());
            var finalScore = CalculateScore(finalDeck);
            Console.WriteLine($"Part 1: the final score is {finalScore}");


            var (winner, finalRecursiveDeck) = PlayRecursively(decks.Select(d => new Queue<int>(d)).ToArray());
            var finalRecursiveScore = CalculateScore(finalRecursiveDeck);
            Console.WriteLine($"Part 2: the final score is {finalRecursiveScore}");
        }

        private static (int, IEnumerable<int>) PlayRecursively(Queue<int>[] decks)
        {
            var seenGames = new List<int[]>();

            while (decks[0].Count > 0 && decks[1].Count > 0)
            {
                if(seenGames.Any(sg=> Enumerable.SequenceEqual(sg, decks[0])))
                {
                    return (0, decks[0]);
                }
                seenGames.Add(decks[0].ToArray());

                var card1 = decks[0].Dequeue();
                var card2 = decks[1].Dequeue();
                int winner = 0;

                if ((card1 <= decks[0].Count) && (card2 <= decks[1].Count))
                {
                    (winner, _) = PlayRecursively(new[]
                        {
                            new Queue<int>(decks[0].Take(card1)),
                            new Queue<int>(decks[1].Take(card2))
                        });

                }
                else if (card1 > card2)
                {
                    winner = 0;
                }
                else
                {
                    winner = 1;
                }

                if (winner == 0)
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
                return (0, decks[0]);
            return (1, decks[1]);
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

        private static IEnumerable<int>[] ReadDecks(string fileName)
        {
            var decks = File.ReadAllText(fileName).Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            return new[]
            {
                decks[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray(),
                decks[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray(),
            };
        }
    }
}

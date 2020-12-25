using System;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            //var door = 17807724;
            //var card = 5764801;
            var door = 1526110;
            var card = 20175123;
            var cardLoopSize = FindLoopSize(7, card);
            var doorLoopSize = FindLoopSize(7, door);

            var doorEncryptionKey = CalculateEncryptionKey(card, doorLoopSize);
            var cardEncryptionKey = CalculateEncryptionKey(door, cardLoopSize);

            Console.WriteLine($"Encryption keys (should be equal): {doorEncryptionKey}, {cardEncryptionKey}");
        }

        private static long CalculateEncryptionKey(int subjectNumber, int loopSize)
        {
            long n = 1;

            for (int x = 0; x < loopSize; ++x)
            {
                n = n * subjectNumber % 20201227;
            }

            return n;
        }

        private static int FindLoopSize(int subjectNumber, int publicKey)
        {
            for (int n = 1, loopSize = 1; loopSize < int.MaxValue; loopSize++)
            {
                n = (subjectNumber * n) % 20201227;
                if (n == publicKey)
                    return loopSize;
            }

            return 0;
        }
    }
}

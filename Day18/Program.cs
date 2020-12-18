using System;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            Verify(26, "2 * 3 + (4 * 5)");
            Verify(437, "5 + (8 * 3 + 9 + 3 * 4 * 3)");
            Verify(12240, "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))");
            Verify(13632, "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2");

            var result = File.ReadAllLines("input.txt").Select(RunFormula).Sum();
            Console.WriteLine($"Part 1: The result is {result}");
        }

        private static void Verify(long expected, string formula)
        {
            var result = RunFormula(formula);
            if(result!= expected)
            {
                Console.WriteLine($"/!\\ Formula: {formula}. Expected {expected} but got {result}");
            }
            else
            {
                Console.WriteLine($"(i) Formula: {formula}. Got the expected result: {expected}");
            }
        }

        static long RunFormula(string formula)
        {
            long acc = 0;
            char op = ' ';

            void UpdateAcc(long n)
            {
                acc = op switch
                {
                    '+' => acc + n,
                    '*' => acc * n,
                    _ => n
                };
            }

            for (int i = 0; i < formula.Length; i++)
            {
                var c = formula[i];

                if (c == ' ')
                    continue;
                
                if (c == '+' || c == '*')
                {
                    op = c;
                    continue;
                }

                if (char.IsDigit(c))
                {
                    int n = int.Parse($"{c}");
                    UpdateAcc(n);
                    continue;
                }


                if(c=='(')
                {
                    int nOpen = 1;
                    for(var j = i+1;j<formula.Length; ++j)
                    {
                        if(formula[j] == '(')
                        {
                            nOpen++;
                        }
                        else if(formula[j] == ')')
                        {
                            nOpen--;
                            if(nOpen == 0)
                            {
                                UpdateAcc(RunFormula(formula[(i+1)..j]));
                                i = j + 1;
                                break;
                            }
                        }
                    }
                    continue;
                }
            }

            return acc;
        }
    }
}

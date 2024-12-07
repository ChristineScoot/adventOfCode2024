using System;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day07
    {
        public static long Part(string file, int part)
        {
            var lines = File.ReadAllLines(file);
            long result = 0;

            var operatorsPart1 = new[] { '+', '*' };
            var operatorsPart2 = new[] { '+', '*', '|' };
            var operators = part == 1 ? operatorsPart1 : operatorsPart2;

            foreach (var line in lines)
            {
                var parts = line.Split([' ', ':'], StringSplitOptions.RemoveEmptyEntries);
                var target = long.Parse(parts[0]);
                var numbersString = new string[parts.Length - 1];
                Array.Copy(parts, 1, numbersString, 0, parts.Length - 1);
                var numbers = numbersString.Select(long.Parse).ToArray();
                var n = numbers.Length;
                var totalCombinations = (int)Math.Pow(operators.Length, n - 1);

                for (var i = 0; i < totalCombinations; i++)
                {
                    var chosenOperators = GetOperators(i, n - 1, operators);

                    var valueToCheck = numbers[0];


                    for (var j = 0; j < chosenOperators.Length; j++)
                    {
                        valueToCheck = ApplyOperator(valueToCheck, numbers[j + 1], chosenOperators[j]);
                    }

                    if (target != valueToCheck) continue;
                    result += valueToCheck;
                    break;
                }
            }

            return result;
        }

        private static char[] GetOperators(int combination, int length, char[] operators)
        {
            var result = new char[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = operators[combination % operators.Length];
                combination /= operators.Length;
            }

            return result;
        }

        private static long ApplyOperator(long a, long b, char op)
        {
            return op switch
            {
                '+' => a + b,
                '*' => a * b,
                '|' => long.Parse($"{a}{b}"),
                _ => throw new ArgumentException($"Unsupported operator: {op}")
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace adventOfCode2024.Solutions
{
    public static class Day17
    {
        private static readonly Dictionary<char, int> Register = new();

        public static string Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            for (var i = 0; i < 3; i++) //Registers A, B, C
            {
                var line = lines[i];
                var numbers = Regex.Matches(line, @"\d+");
                Register[line.ToCharArray()[line.IndexOf(' ') + 1]] = int.Parse(numbers[0].Value);
            }

            var program = lines[4].Substring("Program: ".Length).Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var outputString = "";
            for (var i = 0; i < program.Length; i += 2)
            {
                var instructionOpcode = program[i];
                var comboOperand = program[i + 1];
                int operandValue;
                switch (comboOperand)
                {
                    case 4:
                        operandValue = Register['A'];
                        break;
                    case 5:
                        operandValue = Register['B'];
                        break;
                    case 6:
                        operandValue = Register['C'];
                        break;
                    case 7:
                        continue;
                    default:
                        operandValue = comboOperand;
                        break;
                }

                switch (instructionOpcode)
                {
                    case 0:
                        Register['A'] = (int)(Register['A'] / Math.Pow(2, operandValue));
                        break;
                    case 1:
                        Register['B'] ^= operandValue; //XOR
                        break;
                    case 2:
                        Register['B'] = operandValue % 8;
                        break;
                    case 3:
                        if (Register['A'] == 0) continue;
                        i = operandValue - 2;
                        break;
                    case 4:
                        Register['B'] ^= Register['C'];
                        break;
                    case 5:
                        outputString = outputString + "," + operandValue % 8;
                        break;
                    case 6:
                        Register['B'] = (int)(Register['A'] / Math.Pow(2, operandValue));
                        break;
                    case 7:
                        Register['C'] = (int)(Register['A'] / Math.Pow(2, operandValue));
                        break;
                }
            }

            return outputString.Substring(1);
        }

        public static BigInteger Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var program = lines[4].Substring("Program: ".Length).Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            return Find(program, 0L);
        }

        private static BigInteger Find(int[] program, BigInteger ans)
        {
            if (program.Length == 0) return ans;
            for (var b = 0; b < 8; b++) // b = a % 8
            {
                BigInteger tempB = b;
                var a = (ans << 3) | tempB;
                tempB ^= 2;
                var c = a >> (int)tempB;
                tempB ^= c % 8;
                tempB ^= 3;

                if (tempB % 8 != program[program.Length - 1]) continue;
                var sub = Find(program.Take(program.Length - 1).ToArray(), a);
                if (sub == -1) continue;
                return sub;
            }

            return -1;
        }
    }
}
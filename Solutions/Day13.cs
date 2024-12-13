using System.IO;
using System.Text.RegularExpressions;

namespace adventOfCode2024.Solutions
{
    public static class Day13
    {
        public static long Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            long result = 0;
            for (var i = 0; i < lines.Length; i += 4)
            {
                var buttonA = Regex.Matches(lines[i], @"\d+");
                var xa = int.Parse(buttonA[0].Value);
                var ya = int.Parse(buttonA[1].Value);
                var buttonB = Regex.Matches(lines[i + 1], @"\d+");
                var xb = int.Parse(buttonB[0].Value);
                var yb = int.Parse(buttonB[1].Value);
                var prize = Regex.Matches(lines[i + 2], @"\d+");
                var xPrize = int.Parse(prize[0].Value);
                var yPrize = int.Parse(prize[1].Value);

                // a1x+b1y=c1
                // a2x+b2y=c2
                // 94a + 22b = 8400
                // 34a + 67b = 5400
                var a = 1.0 * (yb * xPrize - xb * yPrize) / (yb * xa - xb * ya);
                var b = 1.0 * (ya * xPrize - xa * yPrize) / (ya * xb - xa * yb);
                if (a % 1 != 0 || b % 1 != 0)
                {
                    // Console.WriteLine("Couldn't find an integer number of button clicks. a= " + a + " b= " + b);
                    continue;
                }

                if (a > 100 || b > 100 || a < 0 || b < 0)
                {
                    // Console.WriteLine("A button would be pressed more than a 100 times. a= " + a + " b= " + b);
                    continue;
                }

                result += 3 * (long)a + (long)b;
            }

            return result;
        }

        public static long Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            long result = 0;
            for (var i = 0; i < lines.Length; i += 4)
            {
                var buttonA = Regex.Matches(lines[i], @"\d+");
                var xa = int.Parse(buttonA[0].Value);
                var ya = int.Parse(buttonA[1].Value);
                var buttonB = Regex.Matches(lines[i + 1], @"\d+");
                var xb = int.Parse(buttonB[0].Value);
                var yb = int.Parse(buttonB[1].Value);
                var prize = Regex.Matches(lines[i + 2], @"\d+");
                var xPrize = int.Parse(prize[0].Value) + 10000000000000;
                var yPrize = int.Parse(prize[1].Value) + 10000000000000;

                var a = 1.0 * (yb * xPrize - xb * yPrize) / (yb * xa - xb * ya);
                var b = 1.0 * (ya * xPrize - xa * yPrize) / (ya * xb - xa * yb);
                if (a % 1 != 0 || b % 1 != 0 || a < 0 || b < 0)
                {
                    continue;
                }

                result += 3 * (long)a + (long)b;
            }

            return result;
        }
    }
}
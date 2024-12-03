using System.IO;
using System.Text.RegularExpressions;

namespace adventOfCode2024.Solutions
{
    public static class Day03
    {
        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var result = 0;
            foreach (var line in lines)
            {
                var mulsInLine = Regex.Matches(line, @"mul\(\d+,\d+\)");
                foreach (var mul in mulsInLine)
                {
                    var numbers = Regex.Matches(mul.ToString(), @"\d+");
                    result += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
                }
            }

            return result;
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var result = 0;
            var isEnabled = true;
            foreach (var line in lines)
            {
                var mulsInLine = Regex.Matches(line, @"mul\(\d+,\d+\)|do\(\)|don't\(\)");
                foreach (var mul in mulsInLine)
                {
                    var mulString = mul.ToString();
                    switch (mulString)
                    {
                        case "do()":
                            isEnabled = true;
                            break;
                        case "don't()":
                            isEnabled = false;
                            break;
                        default:
                            if (!isEnabled) continue;
                            var numbers = Regex.Matches(mul.ToString(), @"\d+");
                            result += int.Parse(numbers[0].Value) * int.Parse(numbers[1].Value);
                            break;
                    }
                }
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventOfCode2024.Solutions
{
    public static class Day01
    {
        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var leftColumn = new List<int>();
            var rightColumn = new List<int>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                leftColumn.Add(int.Parse(parts[0]));
                rightColumn.Add(int.Parse(parts[1]));
            }

            leftColumn.Sort();
            rightColumn.Sort();

            return leftColumn.Select((t, i) => Math.Abs(t - rightColumn[i])).Sum();
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var leftColumn = new List<int>();
            var rightColumn = new List<int>();
            foreach (var line in lines)
            {
                var numbers = Regex.Matches(line, @"\d+");
                leftColumn.Add(int.Parse(numbers[0].Value));
                rightColumn.Add(int.Parse(numbers[1].Value));
            }

            return leftColumn.Sum(number => number * rightColumn.Count(n => n == number));
        }
    }
}
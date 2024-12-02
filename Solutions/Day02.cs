using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day02
    {
        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var safeReports = 0;
            foreach (var lineString in lines)
            {
                var line = lineString.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
                var ascendant = line[0] < line[1];
                for (var i = 0; i < line.Length - 1; i++)
                {
                    var difference = Math.Abs(line[i] - line[i + 1]);
                    var breaksOrder = line[i] < line[i + 1] != ascendant;
                    var breaksDifference = !(difference >= 1 && difference <= 3);
                    if (breaksOrder || breaksDifference)
                    {
                        break;
                    }

                    if (i == line.Length - 2)
                    {
                        safeReports += 1;
                    }
                }
            }

            return safeReports;
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var safeReports = 0;
            foreach (var lineString in lines)
            {
                var line = lineString.Split(' ').Select(n => Convert.ToInt32(n)).ToList();
                var ascendant = IsAscending(line);
                var indices = IndicesWithBrokenRules(ascendant, line);

                switch (indices.Count)
                {
                    case 0:
                        safeReports++;
                        break;
                    case 2:
                    case 3:
                        foreach (var index in indices)
                        {
                            var copy = new List<int>(line);
                            copy.RemoveAt(index);
                            if (IndicesWithBrokenRules(ascendant, copy).Count == 0)
                            {
                                safeReports++;
                                break;
                            }
                        }

                        break;
                }
            }

            return safeReports;
        }

        /**
         * returns indices of problematic numbers in a 'line'
         */
        private static HashSet<int> IndicesWithBrokenRules(bool ascendant, List<int> line)
        {
            var indices = new HashSet<int>();
            for (var i = 0; i < line.Count - 1; i++)
            {
                var difference = Math.Abs(line[i] - line[i + 1]);
                var breaksOrder = line[i] < line[i + 1] != ascendant;
                var breaksDifference = !(difference >= 1 && difference <= 3);
                if (breaksOrder || breaksDifference)
                {
                    indices.Add(i);
                    indices.Add(i + 1);
                }
            }

            return indices;
        }

        /**
         * assess how many 2 consecutive numbers are asc vs desc
         */
        private static bool IsAscending(List<int> line)
        {
            var asc = 0;
            var desc = 0;
            for (var i = 0; i < line.Count - 1; i++)
            {
                if (line[i] < line[i + 1])
                    asc++;
                else
                    desc++;
            }

            return asc > desc;
        }
    }
}
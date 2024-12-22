using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day19
    {
        private static Dictionary<string, bool> _visited = new();
        private static Dictionary<string, long> _visitedLong = new();
        private static int _maxTowelLength;
        private static HashSet<string> _availableTowels;

        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            List<string> patterns = [];
            var towels = lines[0].Split(',').Select(n => n.Trim()).ToArray();
            _availableTowels = [..towels];
            for (var i = 2; i < lines.Length; i++)
            {
                patterns.Add(lines[i]);
            }

            _maxTowelLength = towels.Max(n => n.Length);
            _visited = [];

            return patterns.Sum(pattern => FindATowel(pattern) ? 1 : 0);
        }

        private static bool FindATowel(string pattern)
        {
            if (_visited.TryGetValue(pattern, out var visited))
                return visited;
            if (_availableTowels.Contains(pattern)) return true;
            var found = false;
            var maxTowelLength = Math.Min(_maxTowelLength, pattern.Length - 1);
            for (var j = maxTowelLength; j > 0; j--)
            {
                if (FindATowel(pattern.Substring(0, j)) &&
                    FindATowel(pattern.Substring(j, pattern.Length - j)))
                {
                    found = true;
                    break;
                }
            }

            _visited[pattern] = found;
            return found;
        }

        private static long FindAllTowels(string pattern)
        {
            if (pattern == "") return 1;
            if (_visitedLong.TryGetValue(pattern, out var visited))
                return visited;
            var count = 0L;
            var maxTowelLength = Math.Min(_maxTowelLength, pattern.Length) + 1;
            for (var i = 0; i < maxTowelLength; i++)
            {
                var prefix = pattern.Substring(0, i);
                var suffix = pattern.Substring(i, pattern.Length - i);
                if (_availableTowels.Contains(prefix))
                {
                    count += FindAllTowels(suffix);
                }
            }

            _visitedLong[pattern] = count;
            return count;
        }

        public static long Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            List<string> patterns = [];
            var towels = lines[0].Split(',').Select(n => n.Trim()).ToArray();
            _availableTowels = [..towels];
            for (var i = 2; i < lines.Length; i++)
            {
                patterns.Add(lines[i]);
            }

            _maxTowelLength = _availableTowels.Max(n => n.Length);
            _visitedLong = [];
            return patterns.Sum(FindAllTowels);
        }
    }
}
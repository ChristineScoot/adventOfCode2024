using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day05
    {
        public static int Part(string file, int part)
        {
            var result = 0;
            var lines = File.ReadAllLines(file);
            var i = 0;
            var rules = new Dictionary<int, HashSet<int>>();
            while (!lines[i].Equals(""))
            {
                var rule = lines[i].Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
                var set = new HashSet<int>();
                if (rules.ContainsKey(int.Parse(rule[0])))
                    set = rules[int.Parse(rule[0])];
                set.Add(int.Parse(rule[1]));
                rules[int.Parse(rule[0])] = set;
                i++;
            }

            var customComparer = new CustomComparer(rules);
            for (i += 1; i < lines.Length; i++)
            {
                var sortedList = new SortedList<int, int>(customComparer);
                var list = new List<int>();
                var pages = lines[i].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var page in pages)
                {
                    var pageNumber = int.Parse(page);
                    list.Add(pageNumber);
                    sortedList.Add(pageNumber, 0);
                }

                switch (part)
                {
                    case 1 when sortedList.Keys.SequenceEqual(list):
                        result += list[list.Count / 2];
                        break;
                    case 2 when !sortedList.Keys.SequenceEqual(list):
                        result += sortedList.Keys[sortedList.Count / 2];
                        break;
                }
            }

            return result;
        }
    }
}
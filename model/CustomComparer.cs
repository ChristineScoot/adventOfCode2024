using System.Collections.Generic;

namespace adventOfCode2024.model
{
    public class CustomComparer : IComparer<int>
    {
        private readonly Dictionary<int, HashSet<int>> _rules;

        public CustomComparer(Dictionary<int, HashSet<int>> rules)
        {
            _rules = rules;
        }

        public int Compare(int x, int y)
        {
            if (_rules.TryGetValue(x, out var xRules) && xRules.Contains(y))
                return -1;

            if (_rules.TryGetValue(y, out var yRules) && yRules.Contains(x))
                return 1;

            return x.CompareTo(y);
        }
    }
}
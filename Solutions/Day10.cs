using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day10
    {
        private static readonly HashSet<Point> Directions = [];
        private static int[][] _map;
        private static Dictionary<Point, List<Point>> _trails = new();
        private static Dictionary<Point, int> _trailsCount = new();

        static Day10()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add(new Point(1, 0));
            Directions.Add(new Point(-1, 0));
            Directions.Add(new Point(0, -1));
            Directions.Add(new Point(0, 1));
        }

        public static long Part1(string file)
        {
            _trails = new Dictionary<Point, List<Point>>();
            var mapString = File.ReadAllLines(file);
            _map = mapString
                .Select(line => line.Select(ch => ch - '0').ToArray()).ToArray();

            long result = 0;
            for (var y = 0; y < _map.Length; y++)
            {
                for (var x = 0; x < _map[y].Length; x++)
                {
                    if (_map[y][x] != 0) continue;
                    var trailhead = new Point(x, y);
                    _trails.Add(trailhead, []);
                    FindATrail(trailhead, trailhead);
                }
            }

            foreach (var trailhead in _trails)
            {
                result += trailhead.Value.Count;
            }

            return result;
        }

        private static void FindATrail(Point head, Point current)
        {
            foreach (var direction in Directions)
            {
                var nextDirection = new Point(current.X + direction.X, current.Y + direction.Y);
                if (HelperMethods.IsOutOfBounds(nextDirection.X, nextDirection.Y, _map[0].Length, _map.Length))
                    continue;
                if (_map[nextDirection.Y][nextDirection.X] - _map[current.Y][current.X] != 1)
                    continue;
                if (_map[nextDirection.Y][nextDirection.X] == 9)
                {
                    if (_trails[head].Contains(nextDirection)) continue;
                    _trails[head].Add(nextDirection);
                }
                else
                    FindATrail(head, nextDirection);
            }
        }

        private static void FindATrail2(Point head, Point current)
        {
            foreach (var direction in Directions)
            {
                var nextDirection = new Point(current.X + direction.X, current.Y + direction.Y);
                if (HelperMethods.IsOutOfBounds(nextDirection.X, nextDirection.Y, _map[0].Length, _map.Length))
                    continue;
                if (_map[nextDirection.Y][nextDirection.X] - _map[current.Y][current.X] != 1)
                    continue;
                if (_map[nextDirection.Y][nextDirection.X] == 9)
                {
                    _trailsCount[head]++;
                }
                else
                    FindATrail2(head, nextDirection);
            }
        }

        public static long Part2(string file)
        {
            _trailsCount = new Dictionary<Point, int>();
            var mapString = File.ReadAllLines(file);
            _map = mapString
                .Select(line => line.Select(ch => ch - '0').ToArray()).ToArray();

            long result = 0;
            for (var y = 0; y < _map.Length; y++)
            {
                for (var x = 0; x < _map[y].Length; x++)
                {
                    if (_map[y][x] != 0) continue;
                    var trailhead = new Point(x, y);
                    _trailsCount.Add(trailhead, 0);
                    FindATrail2(trailhead, trailhead);
                }
            }

            foreach (var trailhead in _trailsCount)
            {
                result += trailhead.Value;
            }

            return result;
        }
    }
}
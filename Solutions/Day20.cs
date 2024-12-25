using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day20
    {
        private static readonly Dictionary<char, Point> Directions = new();
        private static char[][] _grid = [];
        private static int _result;
        private static Dictionary<char, char> _opposites = new();

        static Day20()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add('^', new Point(0, -1));
            Directions.Add('>', new Point(1, 0));
            Directions.Add('v', new Point(0, 1));
            Directions.Add('<', new Point(-1, 0));
            _opposites = new Dictionary<char, char>
            {
                ['<'] = '>',
                ['>'] = '<',
                ['^'] = 'v',
                ['v'] = '^'
            };
        }

        public static int Part1(string file)
        {
            _result = 0;
            var lines = File.ReadAllLines(file);
            _grid = new char[lines.Length][];
            var start = new Point(0, 0);
            var end = new Point(0, 0);
            for (var row = 0; row < _grid.Length; row++)
            {
                _grid[row] = lines[row].ToCharArray();
                if (_grid[row].Contains('S'))
                    start = new Point(lines[row].IndexOf('S'), row);
                if (_grid[row].Contains('E'))
                    end = new Point(lines[row].IndexOf('E'), row);
            }

            var track = GetTrack(start, end);

            foreach (var currentPosition in track)
            {
                if (currentPosition == end)
                {
                    break;
                }

                foreach (var kvp in Directions)
                {
                    var dir = kvp.Value;
                    var nextPosition = new Point(currentPosition.X + dir.X, currentPosition.Y + dir.Y);
                    if (HelperMethods.IsOutOfBounds(nextPosition, _grid[0].Length, _grid.Length)) continue;
                    if (_grid[nextPosition.Y][nextPosition.X] == '#')
                    {
                        FindCheat(currentPosition, nextPosition, 1, track);
                    }
                }
            }

            return _result;
        }

        private static List<Point> GetTrack(Point start, Point end)
        {
            var currentPosition = start;
            var currentDirection = '^';
            List<Point> track = [start];

            while (currentPosition != end)
            {
                foreach (var direction in Directions)
                {
                    if (_opposites[currentDirection] == direction.Key) continue;
                    var nextPosition = new Point(currentPosition.X + direction.Value.X,
                        currentPosition.Y + direction.Value.Y);
                    if (_grid[nextPosition.Y][nextPosition.X] == '#') continue;
                    currentPosition = nextPosition;
                    currentDirection = direction.Key;
                    track.Add(currentPosition);
                    break;
                }
            }

            return track;
        }

        private static void FindCheat(Point initialPathPoint, Point potentialCheatPosition, int cheatNumber,
            List<Point> track)
        {
            var currentPosition = potentialCheatPosition;
            if (cheatNumber > 1) return;

            foreach (var direction in Directions)
            {
                var directionPoint = direction.Value;
                var nextPosition = new Point(currentPosition.X + directionPoint.X,
                    currentPosition.Y + directionPoint.Y);
                if (HelperMethods.IsOutOfBounds(nextPosition, _grid[0].Length, _grid.Length)) return;
                if (_grid[nextPosition.Y][nextPosition.X] == '#') continue;

                var nextPathIndex = track.IndexOf(nextPosition);
                var previousPathIndex = track.IndexOf(initialPathPoint);
                if (nextPathIndex <= previousPathIndex) continue;
                var difference =
                    nextPathIndex - previousPathIndex - 2;
                if (difference >= 100) _result++;
            }
        }

        public static int Part2(string file, int saveAtLeast)
        {
            _result = 0;
            var lines = File.ReadAllLines(file);
            _grid = new char[lines.Length][];
            var s = new Point(0, 0);
            var e = new Point(0, 0);
            for (var row = 0; row < _grid.Length; row++)
            {
                _grid[row] = lines[row].ToCharArray();
                if (_grid[row].Contains('S'))
                    s = new Point(lines[row].IndexOf('S'), row);
                if (_grid[row].Contains('E'))
                    e = new Point(lines[row].IndexOf('E'), row);
            }

            var path = GetTrack(s, e);
            var pathMap = new Dictionary<Point, int>();
            for (var i = 0; i < path.Count; i++)
                pathMap.Add(path[i], i);

            var visited = new HashSet<Point>();
            for (var start = 0; start < path.Count; start++)
            {
                var current = path[start];
                for (var end = 2; end <= 20; end++)
                {
                    visited.Clear();
                    for (var d = 0; d <= end; d++)
                    {
                        if (!pathMap.TryGetValue(new Point(current.X + d, current.Y + end - d), out var cutCorner))
                            cutCorner = -1;
                        if (cutCorner != -1 && (cutCorner - start - end) >= saveAtLeast &&
                            visited.Add(new Point(start, cutCorner)))
                            _result++;

                        if (!pathMap.TryGetValue(new Point(current.X - d, current.Y + end - d), out cutCorner))
                            cutCorner = -1;
                        if (cutCorner != -1 && (cutCorner - start - end) >= saveAtLeast &&
                            visited.Add(new Point(start, cutCorner)))
                            _result++;

                        if (!pathMap.TryGetValue(new Point(current.X + d, current.Y - end + d), out cutCorner))
                            cutCorner = -1;
                        if (cutCorner != -1 && (cutCorner - start - end) >= saveAtLeast &&
                            visited.Add(new Point(start, cutCorner)))
                            _result++;

                        if (!pathMap.TryGetValue(new Point(current.X - d, current.Y - end + d), out cutCorner))
                            cutCorner = -1;
                        if (cutCorner != -1 && (cutCorner - start - end) >= saveAtLeast &&
                            visited.Add(new Point(start, cutCorner)))
                            _result++;
                    }
                }
            }

            return _result;
        }
    }
}
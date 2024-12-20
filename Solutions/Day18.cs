using System.Collections.Generic;
using System.Drawing;
using System.IO;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day18
    {
        private static readonly Dictionary<char, Point> Directions = new();
        private static char[][] _grid = [];
        private static int _width;
        private static int _height;

        static Day18()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add('^', new Point(0, -1));
            Directions.Add('>', new Point(1, 0));
            Directions.Add('v', new Point(0, 1));
            Directions.Add('<', new Point(-1, 0));
        }

        public static int Part1(string file, int width, int height, int bytes)
        {
            _width = width;
            _height = height;
            var lines = File.ReadAllLines(file);
            List<Point> points = [];
            foreach (var line in lines)
            {
                var coord = line.Split(',');
                points.Add(new Point(int.Parse(coord[0]), int.Parse(coord[1])));
            }

            _grid = new char[height][];
            for (var row = 0; row < _grid.Length; row++)
            {
                _grid[row] = new char[width];
                for (var col = 0; col < width; col++)
                {
                    _grid[row][col] = '.';
                }
            }

            for (var i = 0; i < bytes; i++)
            {
                var point = points[i];
                _grid[point.Y][point.X] = '#';
            }

            return FindShortestPath();
        }

        public static string Part2(string file, int width, int height, int bytes)
        {
            _width = width;
            _height = height;
            var lines = File.ReadAllLines(file);
            List<Point> points = [];
            foreach (var line in lines)
            {
                var coord = line.Split(',');
                points.Add(new Point(int.Parse(coord[0]), int.Parse(coord[1])));
            }

            _grid = new char[height][];
            for (var row = 0; row < _grid.Length; row++)
            {
                _grid[row] = new char[width];
                for (var col = 0; col < width; col++)
                {
                    _grid[row][col] = '.';
                }
            }

            for (var i = 0; i < bytes; i++)
            {
                var point = points[i];
                _grid[point.Y][point.X] = '#';
            }

            for (var i = bytes; i < points.Count; i++)
            {
                var point = points[i];
                _grid[point.Y][point.X] = '#';
                if (FindShortestPath() == -1)
                    return point.X + "," + point.Y;
            }

            return "Point not found";
        }

        private static int FindShortestPath()
        {
            var startPosition = new Point(0, 0);
            var endPosition = new Point(_width - 1, _height - 1);

            var queue = new Queue<Point>();
            var currentWeight = 0;
            var pathLengths = new Dictionary<Point, int> { [startPosition] = currentWeight };
            queue.Enqueue(startPosition);

            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();
                currentWeight = pathLengths[currentPosition];
                if (currentPosition == endPosition)
                {
                    continue;
                }

                foreach (var direction in Directions)
                {
                    var nextPosition = new Point(currentPosition.X + direction.Value.X,
                        currentPosition.Y + direction.Value.Y);
                    if (HelperMethods.IsOutOfBounds(nextPosition, _width, _height)) continue;
                    if (_grid[nextPosition.Y][nextPosition.X] == '#') continue;

                    if (pathLengths.ContainsKey(nextPosition) &&
                        pathLengths[nextPosition] <= currentWeight + 1) continue;
                    pathLengths[nextPosition] = currentWeight + 1;
                    queue.Enqueue(nextPosition);
                }
            }

            if (!pathLengths.TryGetValue(endPosition, out var shortestPathLength))
                shortestPathLength = -1;
            return shortestPathLength;
        }
    }
}
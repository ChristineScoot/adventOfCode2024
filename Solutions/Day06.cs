using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day06
    {
        private static readonly Dictionary<int, Point> Directions = new Dictionary<int, Point>();
        private static Dictionary<Point, int[]> _visited = new Dictionary<Point, int[]>();

        static Day06()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add(0, new Point(0, -1));
            Directions.Add(90, new Point(1, 0));
            Directions.Add(180, new Point(0, 1));
            Directions.Add(270, new Point(-1, 0));
        }

        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var grid = new char[lines.Length][];
            var currPosition = new Point(0, 0);

            for (var row = 0; row < lines.Length; row++)
            {
                grid[row] = lines[row].ToCharArray();
                if (!lines[row].Contains("^")) continue;
                var col = lines[row].IndexOf("^", StringComparison.Ordinal);
                currPosition.X = col;
                currPosition.Y = row;
                grid[row][col] = 'X';
            }

            var direction = 0;
            var result = 1;
            while (!HelperMethods.IsOutOfBounds(currPosition.X + Directions[direction].X,
                       currPosition.Y + Directions[direction].Y, grid[0].Length, grid.Length))
            {
                var xNextDirection = currPosition.X + Directions[direction].X;
                var yNextDirection = currPosition.Y + Directions[direction].Y;
                if (grid[yNextDirection][xNextDirection] == '#')
                {
                    direction = (direction + 90) % 360;
                    xNextDirection = currPosition.X + Directions[direction].X;
                    yNextDirection = currPosition.Y + Directions[direction].Y;
                }

                if (grid[yNextDirection][xNextDirection] == '.')
                {
                    grid[yNextDirection][xNextDirection] = 'X';
                    result++;
                }

                currPosition.X = xNextDirection;
                currPosition.Y = yNextDirection;
            }

            return result;
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var grid = new char[lines.Length][];
            var currPosition = new Point(0, 0);

            for (var row = 0; row < lines.Length; row++)
            {
                grid[row] = lines[row].ToCharArray();
                if (!lines[row].Contains("^")) continue;
                var col = lines[row].IndexOf("^", StringComparison.Ordinal);
                currPosition.X = col;
                currPosition.Y = row;
                grid[row][col] = 'X';
            }

            var originalGrid = DeepCopy(grid);
            var originalCurrPosition = currPosition;
            var result = 0;

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[0].Length; j++)
                {
                    if (i == originalCurrPosition.Y && j == originalCurrPosition.X) continue;
                    grid = DeepCopy(originalGrid);
                    if (grid[i][j] == '#') continue;
                    grid[i][j] = 'O';
                    currPosition = originalCurrPosition;
                    _visited = new Dictionary<Point, int[]>();
                    var direction = 0;
                    _visited[currPosition] = new[] { direction };

                    while (!HelperMethods.IsOutOfBounds(currPosition.X + Directions[direction].X,
                               currPosition.Y + Directions[direction].Y, grid[0].Length, grid.Length))
                    {
                        var xNextDirection = currPosition.X + Directions[direction].X;
                        var yNextDirection = currPosition.Y + Directions[direction].Y;
                        while (grid[yNextDirection][xNextDirection] == '#' ||
                               grid[yNextDirection][xNextDirection] == 'O')
                        {
                            direction = (direction + 90) % 360;

                            xNextDirection = currPosition.X + Directions[direction].X;
                            yNextDirection = currPosition.Y + Directions[direction].Y;
                        }

                        if (IsInLoop(new Point(xNextDirection, yNextDirection), direction))
                        {
                            result++;
                            break;
                        }

                        if (grid[yNextDirection][xNextDirection] == '.')
                        {
                            grid[yNextDirection][xNextDirection] = 'X';
                        }

                        AddToVisited(new Point(xNextDirection, yNextDirection), direction);
                        currPosition.X = xNextDirection;
                        currPosition.Y = yNextDirection;
                    }
                }
            }

            return result;
        }

        private static bool IsInLoop(Point point, int direction)
        {
            return _visited.TryGetValue(point, out var visitedDirection) && visitedDirection.Contains(direction);
        }

        private static void AddToVisited(Point point, int direction)
        {
            if (_visited.ContainsKey(point))
                _visited[point] = _visited[point].Concat(new[] { direction }).ToArray();
            else
                _visited[point] = new[] { direction };
        }

        private static char[][] DeepCopy(char[][] original)
        {
            var copy = new char[original.Length][];

            for (var i = 0; i < original.Length; i++)
            {
                copy[i] = new char[original[i].Length];
                Array.Copy(original[i], copy[i], original[i].Length);
            }

            return copy;
        }
    }
}
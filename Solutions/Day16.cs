using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day16
    {
        private static readonly Dictionary<char, Point> Directions = new();
        private static char[][] _grid = [];
        private static int minWeight;

        static Day16()
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

        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            _grid = new char[lines.Length][];
            for (var row = 0; row < _grid.Length; row++)
            {
                _grid[row] = lines[row].ToCharArray();
            }

            var startPosition = new Point(1, _grid.Length - 2);
            var endPosition = new Point(_grid[0].Length - 2, 1);
            var currentPosition = startPosition;
            var opposites = new Dictionary<char, char>
            {
                ['<'] = '>',
                ['>'] = '<',
                ['^'] = 'v',
                ['v'] = '^'
            };

            var queue = new PriorityQueue();
            queue.Enqueue(0, currentPosition, '>');
            var currentWeight = 0;
            while (currentPosition != endPosition)
            {
                var item = queue.Dequeue();
                currentPosition = item.Point;
                currentWeight = item.Weight;
                foreach (var direction in Directions)
                {
                    var nextWeight = currentWeight;
                    var directionPrev = item.Direction;
                    if (opposites[direction.Key] == directionPrev) continue;
                    var nextPosition = new Point(currentPosition.X + direction.Value.X,
                        currentPosition.Y + direction.Value.Y);
                    if (_grid[nextPosition.Y][nextPosition.X] == '#') continue; //wall
                    if (directionPrev != direction.Key) nextWeight += 1000;
                    nextWeight++;
                    if (queue.Contains(nextPosition))
                    {
                        if (queue.GetWeight(nextPosition) < nextWeight) continue;
                        queue.UpdateWeight(nextPosition,
                            nextWeight);
                        continue;
                    }

                    queue.Enqueue(nextWeight, nextPosition, direction.Key);
                }
            }

            return currentWeight;
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            _grid = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                _grid[i] = lines[i].ToCharArray();
            }

            var start = new Point(1, _grid.Length - 2);
            var end = new Point(_grid[0].Length - 2, 1);
            var queue = new Queue<GridState>();
            var visited = new Dictionary<PointDirection, int>();
            var shortestPathTiles = new HashSet<Point>();

            queue.Enqueue(new GridState(start, 0, [start], '>'));
            visited[new PointDirection(start, '>')] = 0;

            var shortestDistance = int.MaxValue;
            var allShortestPaths = new List<HashSet<Point>>();

            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();
                var currentPosition = currentState.Position;
                var currentDistance = currentState.Distance;
                var currentDirection = currentState.Direction;

                if (currentPosition == end)
                {
                    if (currentDistance < shortestDistance)
                    {
                        shortestDistance = currentDistance;
                        allShortestPaths.Clear();
                        allShortestPaths.Add(currentState.PathTiles);
                    }
                    else if (currentDistance == shortestDistance)
                    {
                        allShortestPaths.Add(currentState.PathTiles);
                    }

                    continue;
                }

                foreach (var kvp in Directions)
                {
                    var nextDirection = kvp.Key;
                    var dir = kvp.Value;
                    var nextPosition = new Point(currentPosition.X + dir.X, currentPosition.Y + dir.Y);
                    if (_grid[nextPosition.Y][nextPosition.X] == '#') continue;

                    var nextDistance = currentDistance + 1;
                    if (nextDirection != currentDirection)
                    {
                        nextDistance += 1000;
                    }

                    if (!visited.ContainsKey(new PointDirection(nextPosition, nextDirection)) ||
                        visited[new PointDirection(nextPosition, nextDirection)] >= nextDistance)
                    {
                        visited[new PointDirection(nextPosition, nextDirection)] = nextDistance;
                        var newPathTiles = new HashSet<Point>(currentState.PathTiles);
                        newPathTiles.Add(nextPosition);
                        queue.Enqueue(new GridState(nextPosition, nextDistance, newPathTiles, nextDirection));
                    }
                }
            }

            foreach (var path in allShortestPaths)
            {
                foreach (var tile in path)
                {
                    shortestPathTiles.Add(tile);
                }
            }

            // foreach (var tile in shortestPathTiles)
            // {
            //     if (_grid[tile.Y][tile.X] == '.' || _grid[tile.Y][tile.X] == 'S' || _grid[tile.Y][tile.X] == 'E')
            //         _grid[tile.Y][tile.X] = 'O';
            // }
            // HelperMethods.PrintArray(_grid);

            return shortestPathTiles.Count;
        }
    }

    internal record PointDirection()
    {
        public Point Point { get; set; }
        public int Direction { get; set; }

        public PointDirection(Point point, int direction) : this()
        {
            Point = point;
            Direction = direction;
        }
    }

    public class PriorityQueue
    {
        private readonly SortedList<int, Queue<QueueItem>> _queue = new();

        public void Enqueue(int weight, Point point, char direction)
        {
            if (!_queue.ContainsKey(weight))
                _queue[weight] = new Queue<QueueItem>();

            _queue[weight].Enqueue(new QueueItem(weight, point, direction));
        }

        public QueueItem Dequeue()
        {
            if (_queue.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var smallestWeight = _queue.First();
            var item = smallestWeight.Value.Dequeue();

            if (smallestWeight.Value.Count == 0)
                _queue.Remove(smallestWeight.Key);

            return item;
        }

        public bool Contains(Point point)
        {
            return _queue.Values.Any(queue => queue.Any(item => item.Point.Equals(point)));
        }

        public int? GetWeight(Point point)
        {
            foreach (var kv in _queue.Where(kv => kv.Value.Any(item => item.Point.Equals(point))))
            {
                return kv.Key;
            }

            return null;
        }

        public void UpdateWeight(Point point, int newWeight)
        {
            foreach (var kv in _queue)
            {
                var queue = kv.Value;
                var itemToUpdate = queue.FirstOrDefault(item => item.Point.Equals(point));
                if (itemToUpdate == null) continue;
                var direction = itemToUpdate.Direction;
                var updatedQueue = new Queue<QueueItem>(queue.Where(item => !item.Point.Equals(point)));
                _queue[kv.Key] = updatedQueue;
                if (_queue[kv.Key].Count == 0)
                    _queue.Remove(kv.Key);
                Enqueue(newWeight, point, direction);
                return;
            }

            throw new InvalidOperationException("Point not found in the queue.");
        }
    }

    public class QueueItem(int weight, Point point, char direction)
    {
        public int Weight { get; } = weight;
        public Point Point { get; } = point;
        public char Direction { get; } = direction;
    }

    internal class GridState(Point position, int distance, HashSet<Point> pathTiles, char direction)
    {
        public Point Position { get; } = position;
        public int Distance { get; } = distance;
        public HashSet<Point> PathTiles { get; } = pathTiles;
        public char Direction { get; } = direction;
    }
}
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
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day21
    {
        private static readonly Dictionary<char, Point> Directions = new();
        private static readonly Dictionary<PointPairDepth, long> Computed = new();
        private static long _result;
        private static Dictionary<PointPair, List<string>> _numSeqs;
        private static Dictionary<PointPair, List<string>> _dirSeqs;

        private static readonly char?[][] NumKeypad =
        [
            ['7', '8', '9'],
            ['4', '5', '6'],
            ['1', '2', '3'],
            [null, '0', 'A']
        ];

        private static readonly char?[][] DirKeypad =
        [
            [null, '^', 'A'],
            ['<', 'v', '>'],
        ];

        static Day21()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add('^', new Point(0, -1));
            Directions.Add('>', new Point(1, 0));
            Directions.Add('v', new Point(0, 1));
            Directions.Add('<', new Point(-1, 0));
            _numSeqs = compute_seqs(NumKeypad);
            _dirSeqs = compute_seqs(DirKeypad);
        }

        private static Dictionary<PointPair, List<string>> compute_seqs(char?[][] keypad)
        {
            var position = new Dictionary<char?, Point>();
            for (var y = 0; y < keypad.Length; y++)
            {
                for (var x = 0; x < keypad[y].Length; x++)
                {
                    if (keypad[y][x] == null) continue;
                    position[keypad[y][x]] = new Point(x, y);
                }
            }

            var sequences = new Dictionary<PointPair, List<string>>();
            foreach (var from in position)
            {
                foreach (var to in position)
                {
                    if (from.Value == to.Value)
                    {
                        sequences[new PointPair(from.Value, to.Value)] = ["A"];
                        continue;
                    }

                    //BFS
                    var possibilities = new List<string>();
                    Queue<PositionSequence> queue = [];
                    queue.Enqueue(new PositionSequence(from.Value, ""));
                    var optimalLength = int.MaxValue;
                    while (queue.Count > 0)
                    {
                        var isLongerPathFound = false;
                        var curr = queue.Dequeue();
                        var currPosition = curr.Position;
                        var currMoves = curr.Sequence;
                        foreach (var direction in Directions)
                        {
                            var nextPosition = new Point(currPosition.X + direction.Value.X,
                                currPosition.Y + direction.Value.Y);
                            if (HelperMethods.IsOutOfBounds(nextPosition, keypad[0].Length, keypad.Length)) continue;
                            if (keypad[nextPosition.Y][nextPosition.X] == null) continue;
                            if (keypad[nextPosition.Y][nextPosition.X] == to.Key)
                            {
                                if (optimalLength < currMoves.Length + 1)
                                {
                                    isLongerPathFound = true;
                                    break;
                                }

                                optimalLength = currMoves.Length + 1;
                                possibilities.Add(currMoves + direction.Key + "A");
                            }
                            else
                            {
                                queue.Enqueue(new PositionSequence(nextPosition, currMoves + direction.Key));
                            }
                        }

                        if (isLongerPathFound) break;
                    }

                    sequences[new PointPair(from.Value, to.Value)] = possibilities;
                }
            }

            return sequences;
        }

        /**
         * return shortest sequences that will work
         */
        private static List<string> Solve(string code, char?[][] keypad, Dictionary<PointPair, List<string>> sequences)
        {
            var position = new Dictionary<char?, Point>();
            for (var y = 0; y < keypad.Length; y++)
            {
                for (var x = 0; x < keypad[y].Length; x++)
                {
                    if (keypad[y][x] == null) continue;
                    position[keypad[y][x]] = new Point(x, y);
                }
            }

            var options = new List<List<string>>();
            code = "A" + code;
            for (var i = 0; i < code.Length - 1; i++)
            {
                position.TryGetValue(code[i], out var x);
                position.TryGetValue(code[i + 1], out var y);
                var pointPair = new PointPair(x, y);
                options.Add(sequences[pointPair]);
            }

            return options.Aggregate(new List<string> { "" },
                (acc, list) => acc.SelectMany(prefix => list.Select(item => prefix + item)).ToList());
        }

        private record PositionSequence()
        {
            public Point Position { get; set; }
            public string Sequence { get; set; }

            public PositionSequence(Point position, string sequence) : this()
            {
                Position = position;
                Sequence = sequence;
            }
        }

        private record PointPair()
        {
            public Point Point1 { get; set; }
            public Point Point2 { get; set; }

            public PointPair(Point point1, Point point2) : this()
            {
                Point1 = point1;
                Point2 = point2;
            }
        }

        private record PointPairDepth()
        {
            public Point Point1 { get; set; }
            public Point Point2 { get; set; }
            public int Depth { get; set; }

            public PointPairDepth(Point point1, Point point2, int depth) : this()
            {
                Point1 = point1;
                Point2 = point2;
                Depth = depth;
            }
        }

        public static long Part1(string file, int numDirectionalRobots)
        {
            _result = 0L;
            var codes = File.ReadAllLines(file);
            foreach (var code in codes)
            {
                var firstRobot = Solve(code, NumKeypad, _numSeqs);
                var nextRobot = firstRobot;
                for (var i = 0; i < numDirectionalRobots; i++)
                {
                    var nextPossibleRobot = new List<string>();
                    foreach (var moves in nextRobot)
                    {
                        nextPossibleRobot.AddRange(Solve(moves, DirKeypad, _dirSeqs));
                    }

                    var minimum = nextPossibleRobot.Min(n => n.Length);
                    var robot2 = nextPossibleRobot.Where(robotToCheck => robotToCheck.Length == minimum).ToList();

                    nextRobot = robot2;
                }

                var length = nextRobot[0].Length;
                var complexity = length * int.Parse(code.Substring(0, code.Length - 1));
                _result += complexity;
            }


            return _result;
        }

        public static long Part2(string file, int numDirectionalRobots)
        {
            _result = 0L;
            var position = new Dictionary<char?, Point>();
            for (var y = 0; y < DirKeypad.Length; y++)
            {
                for (var x = 0; x < DirKeypad[y].Length; x++)
                {
                    if (DirKeypad[y][x] == null) continue;
                    position[DirKeypad[y][x]] = new Point(x, y);
                }
            }

            var codes = File.ReadAllLines(file);
            foreach (var code in codes)
            {
                var firstRobot = Solve(code, NumKeypad, _numSeqs);
                var optimal = long.MaxValue;
                foreach (var seq in firstRobot)
                {
                    var length = 0L;
                    var seqA = "A" + seq;
                    for (var i = 0; i < seqA.Length - 1; i++)
                    {
                        position.TryGetValue(seqA[i], out var x);
                        position.TryGetValue(seqA[i + 1], out var y);
                        length += ComputeLength(x, y, DirKeypad, numDirectionalRobots);
                    }

                    optimal = Math.Min(optimal, length);
                }

                var complexity = optimal * int.Parse(code.Substring(0, code.Length - 1));
                _result += complexity;
            }


            return _result;
        }

        private static long ComputeLength(Point point1, Point point2, char?[][] keypad, int depth)
        {
            var pointPair = new PointPair(point1, point2);
            var pointPairDepth = new PointPairDepth(point1, point2, depth);
            if (Computed.TryGetValue(pointPairDepth, out var cached)) return cached;

            var position = new Dictionary<char?, Point>();
            for (var y = 0; y < keypad.Length; y++)
            {
                for (var x = 0; x < keypad[y].Length; x++)
                {
                    if (keypad[y][x] == null) continue;
                    position[keypad[y][x]] = new Point(x, y);
                }
            }

            if (depth == 1) return _dirSeqs[pointPair][0].Length;

            var optimal = long.MaxValue;
            foreach (var seq in _dirSeqs[pointPair])
            {
                var length = 0L;
                var seqA = "A" + seq;
                for (var i = 0; i < seqA.Length - 1; i++)
                {
                    position.TryGetValue(seqA[i], out var x);
                    position.TryGetValue(seqA[i + 1], out var y);
                    length += ComputeLength(x, y, keypad, depth - 1);
                }

                optimal = Math.Min(optimal, length);
            }

            Computed[pointPairDepth] = optimal;
            return optimal;
        }
    }
}
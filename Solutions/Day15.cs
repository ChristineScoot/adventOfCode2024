using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day15
    {
        private static readonly Dictionary<char, Point> Directions = new();
        private static char[][] _grid = [];

        static Day15()
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

        public static long Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var newLineIndex = Array.FindIndex(lines, x => x == "");
            _grid = new char[newLineIndex][];
            var instructions = string.Concat(lines.Skip(newLineIndex + 1)
                    .Take(lines.Length - newLineIndex - 1))
                .Replace("\n", "");
            var currRobotPosition = new Point(0, 0);

            for (var row = 0; row < newLineIndex; row++)
            {
                _grid[row] = lines[row].ToCharArray();
                if (!lines[row].Contains("@")) continue;
                var col = lines[row].IndexOf("@", StringComparison.Ordinal);
                currRobotPosition.X = col;
                currRobotPosition.Y = row;
            }

            foreach (var instruction in instructions)
            {
                var direction = Directions[instruction];
                var pointer = new Point(currRobotPosition.X + direction.X, currRobotPosition.Y + direction.Y);
                while (_grid[pointer.Y][pointer.X] == 'O')
                {
                    pointer = new Point(pointer.X + direction.X, pointer.Y + direction.Y);
                }

                if (_grid[pointer.Y][pointer.X] == '#') continue;
                currRobotPosition = new Point(currRobotPosition.X + direction.X, currRobotPosition.Y + direction.Y);
                _grid[pointer.Y][pointer.X] = 'O';
                _grid[currRobotPosition.Y][currRobotPosition.X] = '.';
            }

            return Gps('O');
        }

        public static long Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var newLineIndex = Array.FindIndex(lines, x => x == "");
            _grid = new char[newLineIndex][];
            var instructions = string.Concat(lines.Skip(newLineIndex + 1)
                    .Take(lines.Length - newLineIndex - 1))
                .Replace("\n", "");

            for (var row = 0; row < newLineIndex; row++)
            {
                _grid[row] = lines[row].ToCharArray();
                if (!lines[row].Contains("@")) continue;
            }

            var currRobotPosition = ScaleTheGrid();
            // HelperMethods.PrintArray(_grid);

            var sides = new Dictionary<char, char[]>
            {
                ['<'] = ['[', ']'],
                ['>'] = [']', '[']
            };
            char[] boxArray = ['[', ']'];
            foreach (var instruction in instructions)
            {
                _grid[currRobotPosition.Y][currRobotPosition.X] = '.';
                var direction = Directions[instruction];
                var pointer = new Point(currRobotPosition.X + direction.X, currRobotPosition.Y + direction.Y);
                if (instruction is '<' or '>')
                {
                    while (boxArray.Contains(_grid[pointer.Y][pointer.X]))
                    {
                        pointer = new Point(pointer.X + direction.X, pointer.Y + direction.Y);
                    }

                    if (_grid[pointer.Y][pointer.X] == '#') continue;
                    currRobotPosition = new Point(currRobotPosition.X + direction.X, currRobotPosition.Y + direction.Y);


                    var isEven = false;
                    while (pointer != currRobotPosition)
                    {
                        _grid[pointer.Y][pointer.X] = !isEven ? sides[instruction][0] : sides[instruction][1];
                        isEven = !isEven;
                        pointer = new Point(pointer.X - direction.X, pointer.Y - direction.Y);
                    }
                }
                else
                {
                    var affectedBoxes = new List<Box>();
                    var foundWall = false;
                    while (boxArray.Contains(_grid[pointer.Y][pointer.X]))
                    {
                        if (_grid[pointer.Y][pointer.X] is '[')
                            affectedBoxes.Add(new Box(new Point(pointer.X, pointer.Y),
                                new Point(pointer.X + 1, pointer.Y)));
                        else
                            affectedBoxes.Add(new Box(new Point(pointer.X - 1, pointer.Y),
                                new Point(pointer.X, pointer.Y)));
                        for (var i = 0; i < affectedBoxes.Count; i++)
                        {
                            var box = affectedBoxes[i];
                            var leftNeighbour = new Point(box.Left.X + direction.X, box.Left.Y + direction.Y);
                            var rightNeighbour = new Point(box.Right.X + direction.X, box.Right.Y + direction.Y);
                            if (_grid[leftNeighbour.Y][leftNeighbour.X] == '#' ||
                                _grid[rightNeighbour.Y][rightNeighbour.X] == '#')
                            {
                                foundWall = true;
                                break;
                            }

                            if (boxArray.Contains(_grid[leftNeighbour.Y][leftNeighbour.X]))
                            {
                                if (_grid[leftNeighbour.Y][leftNeighbour.X] is '[')
                                    affectedBoxes.Add(new Box(new Point(leftNeighbour.X, leftNeighbour.Y),
                                        new Point(leftNeighbour.X + 1, leftNeighbour.Y)));
                                else
                                    affectedBoxes.Add(new Box(new Point(leftNeighbour.X - 1, leftNeighbour.Y),
                                        new Point(leftNeighbour.X, leftNeighbour.Y)));
                            }

                            if (boxArray.Contains(_grid[rightNeighbour.Y][rightNeighbour.X]))
                            {
                                if (_grid[rightNeighbour.Y][rightNeighbour.X] is '[')
                                    affectedBoxes.Add(new Box(new Point(rightNeighbour.X, rightNeighbour.Y),
                                        new Point(rightNeighbour.X + 1, rightNeighbour.Y)));
                                else
                                    affectedBoxes.Add(new Box(new Point(rightNeighbour.X - 1, rightNeighbour.Y),
                                        new Point(rightNeighbour.X, rightNeighbour.Y)));
                            }
                        }

                        if (foundWall)
                            break;

                        pointer = new Point(pointer.X + direction.X, pointer.Y + direction.Y);
                    }

                    if (foundWall) continue;
                    var nextBoxPositions = new List<Box>();
                    foreach (var box in affectedBoxes)
                    {
                        _grid[box.Left.Y][box.Left.X] = '.';
                        _grid[box.Right.Y][box.Right.X] = '.';
                        var leftNeighbour = new Point(box.Left.X + direction.X, box.Left.Y + direction.Y);
                        var rightNeighbour = new Point(box.Right.X + direction.X, box.Right.Y + direction.Y);
                        nextBoxPositions.Add(new Box(leftNeighbour, rightNeighbour));
                    }

                    foreach (var box in nextBoxPositions)
                    {
                        _grid[box.Left.Y][box.Left.X] = boxArray[0];
                        _grid[box.Right.Y][box.Right.X] = boxArray[1];
                    }

                    if (_grid[pointer.Y][pointer.X] == '#') continue;
                    currRobotPosition = new Point(currRobotPosition.X + direction.X, currRobotPosition.Y + direction.Y);
                }

                _grid[currRobotPosition.Y][currRobotPosition.X] = '@';
            }
            // HelperMethods.PrintArray(_grid);

            return Gps('[');
        }

        private static long Gps(char box)
        {
            var result = 0L;
            for (var y = 1; y < _grid.Length - 1; y++)
            {
                for (var x = 1; x < _grid[0].Length - 1; x++)
                {
                    if (_grid[y][x] != box) continue;
                    result += 100 * y + x;
                }
            }

            return result;
        }

        private static Point ScaleTheGrid()
        {
            var transform = new Dictionary<char, char[]>
            {
                ['.'] = ['.', '.'],
                ['#'] = ['#', '#'],
                ['O'] = ['[', ']'],
                ['@'] = ['@', '.']
            };
            var robot = new Point(0, 0);
            for (var y = 0; y < _grid.Length; y++)
            {
                var gridLine = _grid[y];
                _grid[y] = new char[gridLine.Length * 2];
                for (int charIndex = 0, x = 0; x < _grid[y].Length; charIndex++, x += 2)
                {
                    _grid[y][x] = transform[gridLine[charIndex]][0];
                    _grid[y][x + 1] = transform[gridLine[charIndex]][1];
                    if (gridLine[charIndex] == '@')
                        robot = new Point(x, y);
                }
            }

            return robot;
        }
    }
}

public record Box()
{
    public Point Left;
    public Point Right;

    public Box(Point left, Point right) : this()
    {
        Left = left;
        Right = right;
    }
}
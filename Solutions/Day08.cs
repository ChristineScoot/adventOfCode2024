using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day08
    {
        private static Dictionary<char, List<Point>> _antennas = new();
        private static Dictionary<Point, int> _antinodes = new();

        private static int _sizeX;
        private static int _sizeY;

        private static void Initialise()
        {
            _antennas = new Dictionary<char, List<Point>>();
            _antinodes = new Dictionary<Point, int>();
        }

        public static int Part(string file, int part)
        {
            Initialise();
            var lines = File.ReadAllLines(file);
            _sizeX = lines[0].Length;
            _sizeY = lines.Length;

            for (var row = 0; row < lines.Length; row++)
            {
                for (var col = 0; col < lines[0].Length; col++)
                {
                    var character = lines[row][col];
                    if (!char.IsDigit(character) && !char.IsLetter(character)) continue;
                    if (!_antennas.TryGetValue(character, out var set))
                        set = [];
                    set.Add(new Point(col, row));
                    _antennas[character] = set;
                }
            }

            foreach (var allCoords in _antennas.Select(frequency => frequency.Value))
            {
                for (var first = 0; first < allCoords.Count - 1; first++)
                {
                    for (var second = first + 1; second < allCoords.Count; second++)
                    {
                        switch (part)
                        {
                            case 1:
                                CalculateAntinode(allCoords[first], allCoords[second]);
                                CalculateAntinode(allCoords[second], allCoords[first]);
                                break;
                            case 2:
                                CalculateAntinode2(allCoords[first], allCoords[second]);
                                CalculateAntinode2(allCoords[second], allCoords[first]);
                                break;
                        }
                    }
                }
            }

            return _antinodes.Count;
        }

        private static void CalculateAntinode(Point p1, Point p2)
        {
            var diffX = p2.X - p1.X;
            var diffY = p2.Y - p1.Y;
            var antinode = new Point(p1.X - diffX, p1.Y - diffY);
            if (HelperMethods.IsOutOfBounds(antinode.X, antinode.Y, _sizeX, _sizeY)) return;
            if (_antinodes.ContainsKey(antinode))
                _antinodes[antinode]++;
            else
                _antinodes[antinode] = 1;
        }

        private static void CalculateAntinode2(Point p1, Point p2)
        {
            if (_antinodes.ContainsKey(p1))
                _antinodes[p1]++;
            else
                _antinodes[p1] = 1;
            if (_antinodes.ContainsKey(p2))
                _antinodes[p2]++;
            else
                _antinodes[p2] = 1;
            do
            {
                var diffX = p2.X - p1.X;
                var diffY = p2.Y - p1.Y;
                var antinode = new Point(p1.X - diffX, p1.Y - diffY);
                if (HelperMethods.IsOutOfBounds(antinode.X, antinode.Y, _sizeX, _sizeY))
                    break;
                if (_antinodes.ContainsKey(antinode))
                    _antinodes[antinode]++;
                else
                    _antinodes[antinode] = 1;
                p2 = p1;
                p1 = antinode;
            } while (true);
        }
    }
}
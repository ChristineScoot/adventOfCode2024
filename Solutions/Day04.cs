using System.Collections.Generic;
using System.Drawing;
using System.IO;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day04
    {
        private static readonly HashSet<Point> Directions = new HashSet<Point>();

        private static readonly Point[] Diagonals =
            { new Point(-1, 1), new Point(1, -1), new Point(1, 1), new Point(-1, -1) };

        static Day04()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add(new Point(1, 0));
            Directions.Add(new Point(-1, 0));
            Directions.Add(new Point(0, -1));
            Directions.Add(new Point(0, 1));
            Directions.Add(new Point(-1, 1));
            Directions.Add(new Point(1, -1));
            Directions.Add(new Point(-1, -1));
            Directions.Add(new Point(1, 1));
        }

        public static int Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var charArray = new char[lines.Length][];
            var result = 0;
            const string word = "XMAS";
            for (var i = 0; i < lines.Length; i++)
            {
                charArray[i] = lines[i].ToCharArray();
            }

            for (var y = 0; y < charArray.Length; y++)
            {
                for (var x = 0; x < charArray[0].Length; x++)
                {
                    if (charArray[y][x] != word[0]) continue;
                    foreach (var direction in Directions)
                    {
                        var xCurrent = x;
                        var yCurrent = y;
                        var i = 0;
                        do
                        {
                            xCurrent += direction.X;
                            yCurrent += direction.Y;
                            i++;
                            if (HelperMethods.IsOutOfBounds(xCurrent, yCurrent, charArray[0].Length, charArray.Length))
                                break;
                            if (word[i] != charArray[yCurrent][xCurrent])
                                break;
                            if (i == word.Length - 1)
                                result++;
                        } while (i < word.Length - 1);
                    }
                }
            }

            return result;
        }

        public static int Part2(string file)
        {
            var lines = File.ReadAllLines(file);
            var charArray = new char[lines.Length][];
            var result = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                charArray[i] = lines[i].ToCharArray();
            }

            for (var y = 1; y < charArray.Length - 1; y++)
            {
                for (var x = 1; x < charArray[0].Length - 1; x++)
                {
                    if (charArray[y][x] != 'A') continue;
                    var x1 = x + Diagonals[0].X;
                    var y1 = y + Diagonals[0].Y;
                    var x1Opposite = x + Diagonals[1].X;
                    var y1Opposite = y + Diagonals[1].Y;

                    var x2 = x + Diagonals[2].X;
                    var y2 = y + Diagonals[2].Y;
                    var x2Opposite = x + Diagonals[3].X;
                    var y2Opposite = y + Diagonals[3].Y;

                    var side1 = charArray[y1Opposite][x1Opposite] == 'M' &&
                                charArray[y1][x1] == 'S';
                    var side1Opposite = charArray[y1Opposite][x1Opposite] == 'S' &&
                                        charArray[y1][x1] == 'M';

                    var side2 = charArray[y2Opposite][x2Opposite] == 'M' &&
                                charArray[y2][x2] == 'S';
                    var side2Opposite = charArray[y2Opposite][x2Opposite] == 'S' &&
                                        charArray[y2][x2] == 'M';

                    if ((side1 || side1Opposite) && (side2 || side2Opposite))
                        result++;
                }
            }

            return result;
        }
    }
}
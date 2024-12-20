using System;
using System.Drawing;

namespace adventOfCode2024.model
{
    public abstract class HelperMethods
    {
        public static void PrintArray(char[][] arr)
        {
            foreach (var chars in arr)
            {
                foreach (var aChar in chars)
                {
                    Console.Write(aChar);
                }

                Console.WriteLine();
            }
        }

        public static void PrintArrayStars(int[][] arr)
        {
            foreach (var ints in arr)
            {
                foreach (var aInt in ints)
                {
                    Console.Write(aInt == 0 ? " " : "*");
                }

                Console.WriteLine();
            }
        }

        public static void PrintArray(int[][] arr)
        {
            foreach (var ints in arr)
            {
                foreach (var aInt in ints)
                {
                    Console.Write(aInt);
                }

                Console.WriteLine();
            }
        }

        public static bool IsOutOfBounds(int x, int y, int sizeX, int sizeY)
        {
            return x < 0 || x >= sizeX || y < 0 || y >= sizeY;
        }

        public static bool IsOutOfBounds(Point point, int sizeX, int sizeY)
        {
            return point.X < 0 || point.X >= sizeX || point.Y < 0 || point.Y >= sizeY;
        }
    }
}
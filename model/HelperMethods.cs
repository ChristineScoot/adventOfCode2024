using System;

namespace adventOfCode2024.model
{
    public abstract class HelperMethods
    {
        public static void PrintArray(char[][] arr) {
            foreach (var chars in arr) {
                foreach (var aChar in chars) {
                    Console.Write(aChar);
                }
                Console.WriteLine();
            }
        }
        public static bool IsOutOfBounds(int x, int y, int sizeX, int sizeY)
        {
            return x < 0 || x >= sizeX || y < 0 || y >= sizeY;
        }
    }
}
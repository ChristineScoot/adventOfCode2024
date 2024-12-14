using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day14
    {
        public static long Part1(string file, int mapSizeX, int mapSizeY)
        {
            const int seconds = 100;
            var map = new int[mapSizeY][];
            for (var y = 0; y < mapSizeY; y++)
            {
                map[y] = new int[mapSizeX];
            }

            var quadrants = new int[4];
            var endPositions = new List<Point>();
            var robots = File.ReadAllLines(file);
            foreach (var robot in robots)
            {
                var robotCoords = Regex.Matches(robot, @"-?\d+");
                var startPoint = new Point(int.Parse(robotCoords[0].Value), int.Parse(robotCoords[1].Value));
                var velocityPoint = new Point(int.Parse(robotCoords[2].Value), int.Parse(robotCoords[3].Value));
                var xEnd = (startPoint.X + seconds * velocityPoint.X);
                var yEnd = (startPoint.Y + seconds * velocityPoint.Y);
                xEnd = Scale(xEnd, mapSizeX);
                yEnd = Scale(yEnd, mapSizeY);

                var endPosition = new Point(xEnd, yEnd);
                endPositions.Add(endPosition);

                var middleX = mapSizeX / 2;
                var middleY = mapSizeY / 2;
                if (xEnd == middleX || yEnd == middleY) continue;
                int q;
                if (xEnd < middleX) //left half
                    q = (yEnd < middleY) ? 0 : 1; //top / bottom
                else //bottom half
                    q = (yEnd < middleY) ? 2 : 3; //top / bottom
                quadrants[q] += 1;
            }

            foreach (var endPosition in endPositions)
            {
                map[endPosition.Y][endPosition.X] += 1;
            }

            var result = 1;
            foreach (var quadrant in quadrants)
            {
                result *= quadrant;
            }

            return result;
        }

        public static long Part2(string file, int mapSizeX, int mapSizeY)
        {
            const int seconds = 10000;
            var map = new int[mapSizeY][];

            var robots = File.ReadAllLines(file);
            int sec;
            List<Point> endPositions = [];
            for (sec = 0; sec < seconds; sec++)
            {
                for (var y = 0; y < mapSizeY; y++)
                {
                    map[y] = new int[mapSizeX];
                }

                var quadrants = new int[4];
                endPositions = [];

                foreach (var robot in robots)
                {
                    var robotCoords = Regex.Matches(robot, @"-?\d+");
                    var startPoint = new Point(int.Parse(robotCoords[0].Value), int.Parse(robotCoords[1].Value));
                    var velocityPoint = new Point(int.Parse(robotCoords[2].Value), int.Parse(robotCoords[3].Value));
                    var xEnd = (startPoint.X + sec * velocityPoint.X);
                    var yEnd = (startPoint.Y + sec * velocityPoint.Y);
                    xEnd = Scale(xEnd, mapSizeX);
                    yEnd = Scale(yEnd, mapSizeY);

                    var endPosition = new Point(xEnd, yEnd);
                    endPositions.Add(endPosition);

                    var middleX = mapSizeX / 2;
                    var middleY = mapSizeY / 2;
                    if (xEnd == middleX || yEnd == middleY) continue;
                    int q;
                    if (xEnd < middleX) //left half
                        q = (yEnd < middleY) ? 0 : 1; //top / bottom
                    else //bottom half
                        q = (yEnd < middleY) ? 2 : 3; //top / bottom
                    quadrants[q] += 1;
                }

                if (quadrants[0] <= 250 && quadrants[2] <= 250 && quadrants[1] <= 250 && quadrants[3] <= 250) continue;
                break;
            }

            foreach (var endPosition in endPositions)
            {
                map[endPosition.Y][endPosition.X] += 1;
            }

            HelperMethods.PrintArrayStars(map);

            return sec;
        }

        private static int Scale(int endPoint, int mapSize)
        {
            endPoint %= mapSize;
            if (endPoint < 0)
                endPoint += mapSize;

            return endPoint;
        }
    }
}
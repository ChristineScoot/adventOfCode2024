using System.Collections.Generic;
using System.Drawing;
using System.IO;
using adventOfCode2024.model;

namespace adventOfCode2024.Solutions
{
    public static class Day12
    {
        private static readonly HashSet<Point> Directions = [];
        private static HashSet<Point> _visited = [];
        private static List<Point> currPoints = [];
        private static char[][] grid;

        static Day12()
        {
            Initialise();
        }

        private static void Initialise()
        {
            Directions.Add(new Point(0, -1));
            Directions.Add(new Point(1, 0));
            Directions.Add(new Point(0, 1));
            Directions.Add(new Point(-1, 0));
        }

        public static long Part(string file, int part)
        {
            var lines = File.ReadAllLines(file);
            grid = new char[lines.Length][];
            for (var row = 0; row < lines.Length; row++)
            {
                grid[row] = lines[row].ToCharArray();
            }

            var gardenPlots = new List<GardenPlot>();
            _visited = [];

            for (var y = 0; y < grid.Length; y++)
            {
                for (var x = 0; x < grid[0].Length; x++)
                {
                    var plotPart = new Point(x, y);
                    if (_visited.Contains(plotPart)) continue;
                    currPoints = [];
                    GeneratePlot(plotPart);
                    gardenPlots.Add(new GardenPlot(currPoints));
                }
            }

            long result = 0;
            foreach (var plot in gardenPlots)
            {
                switch (part)
                {
                    case 1:
                        plot.CalculatePerimiter(grid);
                        break;
                    case 2:
                        plot.CalculateBulkPerimiter(grid);
                        break;
                }

                result += plot.Price();
            }

            return result;
        }

        private static void GeneratePlot(Point plotPart)
        {
            if (!_visited.Add(plotPart)) return;
            currPoints.Add(plotPart);

            foreach (var direction in Directions)
            {
                var neighbour = new Point(plotPart.X + direction.X, plotPart.Y + direction.Y);
                if (HelperMethods.IsOutOfBounds(neighbour.X, neighbour.Y, grid[0].Length, grid.Length)) continue;

                if (grid[neighbour.Y][neighbour.X] == grid[plotPart.Y][plotPart.X])
                {
                    GeneratePlot(neighbour);
                }
            }
        }
    }
}
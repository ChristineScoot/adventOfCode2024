using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace adventOfCode2024.model;

public class GardenPlot
{
    private List<Point> points = [];
    private long perimeter;
    private readonly Dictionary<string, Point> Directions = new();

    public GardenPlot(List<Point> points)
    {
        this.points = points;
        perimeter = 0;
        Directions.Add("up", new Point(0, -1));
        Directions.Add("right", new Point(1, 0));
        Directions.Add("down", new Point(0, 1));
        Directions.Add("left", new Point(-1, 0));
    }

    public long Price()
    {
        return points.Count * perimeter;
    }

    /**
     * Part 1
     */
    public long CalculatePerimiter(char[][] grid)
    {
        foreach (Point point in points)
        {
            int neighboursCount = 0;
            foreach (var direction in Directions)
            {
                var neighbour = new Point(point.X + direction.Value.X, point.Y + direction.Value.Y);
                if (HelperMethods.IsOutOfBounds(neighbour.X, neighbour.Y, grid[0].Length, grid.Length)) continue;
                if (grid[neighbour.Y][neighbour.X] == grid[point.Y][point.X])
                    neighboursCount++;
            }

            perimeter += 4 - neighboursCount;
        }

        return perimeter;
    }

    /**
     * Part 2
     */
    public long CalculateBulkPerimiter(char[][] grid)
    {
        //scale
        var maxY = points.Max(point => point.Y);
        var minY = points.Min(point => point.Y);
        var maxX = points.Max(point => point.X);
        var minX = points.Min(point => point.X);
        var newSizeY = maxY - minY + 1;
        var newSizeX = maxX - minX + 1;
        var newGrid = new char[newSizeY][];
        for (var y = 0; y < newSizeY; y++)
        {
            newGrid[y] = new char[newSizeX];
        }

        foreach (var point in points)
        {
            newGrid[point.Y - minY][point.X - minX] = grid[point.Y][point.X];
        }

        CalculateHorizontal(grid, newGrid, newSizeX, newSizeY, minX, minY);
        CalculateVertical(grid, newGrid, newSizeX, newSizeY, minX, minY);

        return perimeter;
    }

    private void CalculateHorizontal(char[][] grid, char[][] newGrid, int newSizeX, int newSizeY, int minX, int minY)
    {
        for (var y = 0; y < newSizeY; y++)
        {
            var isPreviousCalculated = new bool[2]; // Index 0: Up, Index 1: Down
            for (var x = 0; x < newSizeX; x++)
            {
                var currPoint = new Point(x, y);
                if (newGrid[currPoint.Y][currPoint.X] == '\0')
                {
                    isPreviousCalculated[0] = false;
                    isPreviousCalculated[1] = false;
                    continue;
                }

                for (var i = 0; i < 2; i++)
                {
                    var direction = i == 0 ? "up" : "down";
                    var neighbour = new Point(currPoint.X + Directions[direction].X,
                        currPoint.Y + Directions[direction].Y);

                    if (HelperMethods.IsOutOfBounds(neighbour.X, neighbour.Y, newGrid[0].Length, newGrid.Length))
                    {
                        if (!isPreviousCalculated[i])
                        {
                            isPreviousCalculated[i] = true;
                            perimeter++;
                        }

                        continue;
                    }

                    if (newGrid[neighbour.Y][neighbour.X] == newGrid[currPoint.Y][currPoint.X])
                    {
                        isPreviousCalculated[i] = false;
                    }
                    else if (!isPreviousCalculated[i] &&
                             newGrid[currPoint.Y][currPoint.X] == grid[y + minY][x + minX])
                    {
                        perimeter++;
                        isPreviousCalculated[i] = true;
                    }
                }
            }
        }
    }

    private void CalculateVertical(char[][] grid, char[][] newGrid, int newSizeX, int newSizeY, int minX, int minY)
    {
        for (var x = 0; x < newSizeX; x++)
        {
            var isPreviousCalculated = new bool[2];
            for (var y = 0; y < newSizeY; y++)
            {
                var currPoint = new Point(x, y);
                if (newGrid[currPoint.Y][currPoint.X] == '\0')
                {
                    isPreviousCalculated[0] = false;
                    isPreviousCalculated[1] = false;
                    continue;
                }

                for (var i = 0; i < 2; i++)
                {
                    var direction = i == 0 ? "right" : "left";
                    var neighbour = new Point(currPoint.X + Directions[direction].X,
                        currPoint.Y + Directions[direction].Y);

                    if (HelperMethods.IsOutOfBounds(neighbour.X, neighbour.Y, newGrid[0].Length, newGrid.Length))
                    {
                        if (!isPreviousCalculated[i])
                        {
                            isPreviousCalculated[i] = true;
                            perimeter++;
                        }

                        continue;
                    }

                    if (newGrid[neighbour.Y][neighbour.X] == newGrid[currPoint.Y][currPoint.X])
                    {
                        isPreviousCalculated[i] = false;
                    }
                    else if (!isPreviousCalculated[i] &&
                             newGrid[currPoint.Y][currPoint.X] == grid[y + minY][x + minX])
                    {
                        perimeter++;
                        isPreviousCalculated[i] = true;
                    }
                }
            }
        }
    }
}
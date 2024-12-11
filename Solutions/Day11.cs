using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day11
    {
        public static long Part(string file, int blinks)
        {
            var diskMapString = File.ReadAllText(file);
            var stones = diskMapString.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToDictionary(stone => stone, stone => 1L);

            for (var blink = 0; blink < blinks; blink++)
            {
                var newStones = new Dictionary<long, long>();
                foreach (var stone in stones)
                {
                    var stoneCount = stone.Value;
                    if (stone.Key == 0)
                        if (newStones.ContainsKey(1))
                            newStones[1] += stoneCount;
                        else
                            newStones[1] = stoneCount;
                    else if (HasEvenDigits(stone.Key))
                    {
                        var digitCount = (int)Math.Log10(Math.Abs(stone.Key)) + 1;
                        var zeroes = Math.Pow(10, digitCount / 2);
                        var newStone1 = (long)Math.Floor(stone.Key / zeroes);
                        var newStone2 = (long)(stone.Key - newStone1 * zeroes);
                        if (newStones.ContainsKey(newStone1))
                            newStones[newStone1] += stoneCount;
                        else
                            newStones[newStone1] = stoneCount;
                        if (newStones.ContainsKey(newStone2))
                            newStones[newStone2] += stoneCount;
                        else
                            newStones[newStone2] = stoneCount;
                    }
                    else
                        newStones.Add(stone.Key * 2024, stoneCount);
                }

                stones = newStones;
            }

            return stones.Sum(stone => stone.Value);
        }

        private static bool HasEvenDigits(long number)
        {
            var digitCount = (int)Math.Log10(Math.Abs(number)) + 1;
            return digitCount % 2 == 0;
        }
    }
}
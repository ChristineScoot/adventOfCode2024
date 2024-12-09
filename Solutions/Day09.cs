using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day09
    {
        public static long Part1(string file)
        {
            var diskMapString = File.ReadAllText(file);
            var diskMap = diskMapString.Select(c => int.Parse(c.ToString())).ToArray();

            var left = 0;
            var right = diskMap.Length - 1;
            var list = new List<int>();
            var queue = new Queue<int>();
            var fileId = 0;

            while (fileId <= right / 2)
            {
                for (var i = 0; i < diskMap[left]; i++) //fileLength
                {
                    list.Add(fileId);
                }

                fileId++;
                var freeSpaceLength = diskMap[left + 1];

                while (freeSpaceLength > queue.Count)
                {
                    for (var i = 0; i < diskMap[right]; i++)
                    {
                        queue.Enqueue(right / 2);
                    }

                    right -= 2;
                }

                for (var i = 0; i < freeSpaceLength; i++) //freeSpace
                {
                    list.Add(queue.Dequeue());
                }

                left += 2;
            }

            while (queue.Count > 0)
            {
                list.Add(queue.Dequeue());
            }

            long result = 0;
            for (var i = 0; i < list.Count; i++)
            {
                result += list[i] * i;
            }

            return result;
        }

        public static long Part2(string file)
        {
            var diskMapString = File.ReadAllText(file);
            var diskMap = diskMapString.Select(c => int.Parse(c.ToString())).ToArray();

            var fragmentedWithNulls = new List<int?>();

            var fileId = 0;
            for (var i = 0; i < diskMap.Length; i += 2)
            {
                for (var j = 0; j < diskMap[i]; j++)
                {
                    fragmentedWithNulls.Add(fileId);
                }

                if (i + 1 >= diskMap.Length) break;
                for (var j = 0; j < diskMap[i + 1]; j++)
                {
                    fragmentedWithNulls.Add(null);
                }

                fileId++;
            }

            for (var i = fragmentedWithNulls.Count - 1; i >= 0; i--)
            {
                var count = 1;
                var fileIdToFillNulls = fragmentedWithNulls[i];
                while (i > 0 && fragmentedWithNulls[i - 1] == fileIdToFillNulls)
                {
                    count++;
                    i--;
                }

                for (var j = 0; j < i; j++)
                {
                    var nullsFoundCount = 0;
                    while (j < fragmentedWithNulls.Count && fragmentedWithNulls[j] == null)
                    {
                        nullsFoundCount++;
                        j++;
                    }

                    if (nullsFoundCount < count) continue;
                    j -= nullsFoundCount;
                    for (var k = 0; k < count; k++)
                    {
                        fragmentedWithNulls[j + k] = fileIdToFillNulls;
                    }

                    for (var k = i; k < i + count; k++)
                    {
                        fragmentedWithNulls[k] = null;
                    }

                    break;
                }
            }

            return fragmentedWithNulls
                .Select((value, index) => value.HasValue ? (long)value * index : 0)
                .Sum();
        }
    }
}
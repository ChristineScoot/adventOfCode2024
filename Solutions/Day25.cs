using System.Collections.Generic;
using System.IO;

namespace adventOfCode2024.Solutions
{
    public static class Day25
    {
        private static string[] _lines;

        public static long Finale(string file)
        {
            _lines = File.ReadAllLines(file);
            var locksEmptySpace = new List<int[]>();
            var keysRequiredSpace = new List<int[]>();
            const string lockRegex = "#####";
            for (var i = 0; i < _lines.Length; i++)
            {
                var line = _lines[i];

                if (line.Equals(lockRegex))
                {
                    locksEmptySpace.Add(Calc(++i, '.'));
                }
                else
                {
                    keysRequiredSpace.Add(Calc(++i, '#'));
                }

                while (i < _lines.Length - 1 && !_lines[i].Equals(""))
                {
                    i++;
                }
            }

            var result = 0;
            foreach (var lockS in locksEmptySpace)
            {
                foreach (var key in keysRequiredSpace)
                {
                    var allKeyPinsMatches = true;
                    for (var i = 0; i < lockS.Length; i++)
                    {
                        if (lockS[i] < key[i])
                        {
                            allKeyPinsMatches = false;
                            break;
                        }
                    }

                    if (allKeyPinsMatches)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private static int[] Calc(int i, char character)
        {
            var arrayBuilder = new int[_lines[i].Length];

            while (i < _lines.Length && !_lines[i].Equals(""))
            {
                var currLine = _lines[i];
                for (var pinNum = 0; pinNum < currLine.Length; pinNum++)
                {
                    if (currLine[pinNum] == character)
                    {
                        arrayBuilder[pinNum] += 1;
                    }
                }

                i++;
            }

            return arrayBuilder;
        }
    }
}
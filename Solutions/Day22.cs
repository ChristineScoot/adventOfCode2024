using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day22
    {
        public static long Part1(string file, int numOfIterations)
        {
            var result = 0L;
            var initialSecretNumbers = File.ReadAllLines(file).Select(long.Parse).ToList();

            foreach (var secret in initialSecretNumbers)
            {
                var nextSecret = secret;
                for (var i = 0; i < numOfIterations; i++)
                {
                    nextSecret = CalculateNextSecret(nextSecret);
                }

                result += nextSecret;
            }

            return result;
        }

        private static long CalculateNextSecret(long secret)
        {
            //step 1
            secret ^= secret * 64;
            secret %= 16777216;
            //step 2
            secret ^= (long)Math.Floor(secret / 32.0);
            secret %= 16777216;
            //step 3
            secret ^= secret * 2048;
            secret %= 16777216;

            return secret;
        }

        public static long Part2(string file, int numOfIterations)
        {
            var initialSecretNumbers = File.ReadAllLines(file).Select(long.Parse).ToList();
            HashSet<FourSequence> allSequences = [];
            List<Dictionary<FourSequence, int>> sequencesList = [];
            var a = 0;
            var b = 0;
            var c = 0;
            var d = 0;
            foreach (var secret in initialSecretNumbers)
            {
                Dictionary<FourSequence, int> sequencesToPrice = [];
                var nextSecret = secret;
                var nextPrice = (int)(nextSecret % 10);
                var prevPrice = nextPrice;

                for (var i = 0; i < numOfIterations; i++)
                {
                    nextSecret = CalculateNextSecret(nextSecret);
                    nextPrice = (int)(nextSecret % 10);
                    var diff = nextPrice - prevPrice;
                    prevPrice = nextPrice;
                    a = b;
                    b = c;
                    c = d;
                    d = diff;
                    if (i < 3) continue;
                    var fourSequence = new FourSequence(a, b, c, d);
                    if (sequencesToPrice.ContainsKey(fourSequence)) continue;
                    sequencesToPrice.Add(fourSequence, nextPrice);
                    allSequences.Add(fourSequence);
                }

                sequencesList.Add(sequencesToPrice);
            }

            var maxBananas = long.MinValue;

            foreach (var firstSeq in allSequences)
            {
                var bananaCount = 0;
                foreach (var seq in sequencesList)
                {
                    if (!seq.TryGetValue(firstSeq, out var price)) continue;
                    bananaCount += price;
                }

                maxBananas = Math.Max(maxBananas, bananaCount);
            }

            return maxBananas;
        }

        private record FourSequence()
        {
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
            public int D { get; set; }

            public FourSequence(int a, int b, int c, int d) : this()
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }
        }
    }
}
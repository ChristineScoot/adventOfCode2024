using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day23
    {
        private static Dictionary<string, List<string>> _allComputers = [];
        private static int _result;
        private static readonly Dictionary<string, List<string>> Triplets = [];
        private static int _maxDepth;

        public static long Part1(string file, int numOfComputers)
        {
            _maxDepth = numOfComputers + 1;
            _allComputers = [];
            _result = 0;
            var lines = File.ReadAllLines(file);
            var tComputers = new HashSet<string>();
            foreach (var line in lines)
            {
                var names = line.Split('-');

                var comp1 = names[0];
                var comp2 = names[1];

                if (!_allComputers.TryGetValue(comp1, out var comp1Connections))
                {
                    comp1Connections = [];
                }

                comp1Connections.Add(comp2);
                _allComputers[comp1] = comp1Connections;

                if (!_allComputers.TryGetValue(comp2, out var comp2Connections))
                {
                    comp2Connections = [];
                }

                comp2Connections.Add(comp1);
                _allComputers[comp2] = comp2Connections;

                if (comp1[0] == 't')
                    tComputers.Add(comp1);
                if (comp2[0] == 't')
                    tComputers.Add(comp2);
            }

            foreach (var tComp in tComputers)
            {
                Triplets[tComp] = [];
                Recursion(tComp, tComp);
            }

            return _result;
        }

        private static void Recursion(string startingName, string name, string all = "", int depth = 1)
        {
            if (depth == _maxDepth)
            {
                if (name != startingName) return;
                var currentTriplets = all.Split([','], StringSplitOptions.RemoveEmptyEntries);
                foreach (var triplet in currentTriplets)
                {
                    if (!triplet.StartsWith("t") || !Triplets.TryGetValue(triplet, out var triplet1)) continue;
                    if (triplet1.Any(comp => AreStringsEqual(comp, all))) return;
                }

                Triplets[startingName].Add(all);
                _result++;
                return;
            }

            var connections = _allComputers[name];
            foreach (var conn in connections)
            {
                if (conn == startingName && depth < _maxDepth - 1) continue;
                Recursion(startingName, conn, all + "," + name, depth + 1);
            }
        }

        private static bool AreStringsEqual(string str1, string str2)
        {
            var elements1 = str1.Split([','], StringSplitOptions.RemoveEmptyEntries).OrderBy(s => s).ToArray();
            var elements2 = str2.Split([','], StringSplitOptions.RemoveEmptyEntries).OrderBy(s => s).ToArray();

            return elements1.SequenceEqual(elements2);
        }

        public static string Part2(string file)
        {
            _allComputers = [];
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                var names = line.Split('-');

                var comp1 = names[0];
                var comp2 = names[1];
                if (!_allComputers.TryGetValue(comp1, out var comp1Connections))
                {
                    comp1Connections = [];
                }

                comp1Connections.Add(comp2);
                _allComputers[comp1] = comp1Connections;

                if (!_allComputers.TryGetValue(comp2, out var comp2Connections))
                {
                    comp2Connections = [];
                }

                comp2Connections.Add(comp1);
                _allComputers[comp2] = comp2Connections;
            }

            var cliques = new List<HashSet<string>>();
            BronKerbosch([], [.._allComputers.Keys.ToList()], [], cliques);
            HashSet<string> largestClique = null;
            foreach (var clique in cliques)
            {
                if (largestClique == null || clique.Count > largestClique.Count)
                    largestClique = clique;
            }

            var sortedLargestClique = largestClique!.ToList();
            sortedLargestClique.Sort();

            var result = sortedLargestClique.Aggregate("", (current, lc) => current + (lc + ","));

            return result.Substring(0, result.Length - 1);
        }

        private static void BronKerbosch(HashSet<string> currClique, HashSet<string> potentialNodes,
            HashSet<string> visitedNodes,
            List<HashSet<string>> cliques)
        {
            if (potentialNodes.Count == 0 && visitedNodes.Count == 0)
            {
                cliques.Add([..currClique]);
                return;
            }

            foreach (var node in potentialNodes.ToList())
            {
                var newCurrClique = new HashSet<string>(currClique) { node };

                var neighbours = new HashSet<string>(_allComputers[node]);

                var newPotentialNodes = new HashSet<string>();
                foreach (var p in potentialNodes.Where(p => neighbours.Contains(p)))
                {
                    newPotentialNodes.Add(p);
                }

                var newVisited = new HashSet<string>();
                foreach (var x in visitedNodes.Where(x => neighbours.Contains(x)))
                {
                    newVisited.Add(x);
                }

                BronKerbosch(newCurrClique, newPotentialNodes, newVisited, cliques);

                potentialNodes.Remove(node);
                visitedNodes.Add(node);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day24
    {
        private static readonly SortedDictionary<string, List<string>> Graph = new SortedDictionary<string, List<string>>();
        private static readonly Dictionary<string, string> LogicGates = new Dictionary<string, string>();

        public static long Part1(string file)
        {
            var lines = File.ReadAllLines(file);
            var wires = new Dictionary<string, int>();
            var zWires = new SortedDictionary<string, int>();
            foreach (var line in lines)
            {
                if (line == "")
                    break;
                var value = line.Split(':').Select(n => n.Trim()).ToArray();

                wires[value[0]] = Convert.ToInt32(value[1]);
            }

            var queue = new Queue<string>();
            for (var i = wires.Count + 1; i < lines.Length; i++)
            {
                queue.Enqueue(lines[i]);
            }

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                var value = next.Split(' ').Select(n => n.Trim()).ToArray();

                var input1 = value[0];
                var input2 = value[2];
                if (!wires.ContainsKey(input1) || !wires.ContainsKey(input2))
                {
                    queue.Enqueue(next);
                    continue;
                }

                var logicGate = value[1];
                var outputKey = value[4];

                var outputValue = logicGate switch
                {
                    "AND" => wires[input1] & wires[input2],
                    "OR" => wires[input1] | wires[input2],
                    _ => wires[input1] ^ wires[input2]
                };

                wires[outputKey] = outputValue;
                if (outputKey.StartsWith("z"))
                    zWires[outputKey] = outputValue;
            }

            var binaryValue = zWires.Values.Reverse().Aggregate("", (current, zValue) => current + zValue);

            return Convert.ToInt64(binaryValue, 2);
        }

        public static string Part2(string file)
        {
            var result = "";
            var numOfBits = 45;
            for (int i = 0; i < numOfBits; i++)
            {
                result = Part2a(file, i);
            }

            return result;
        }

        private static string Part2a(string file, int num)
        {
            var lines = File.ReadAllLines(file);
            var wires = new Dictionary<string, int>();
            var zWires = new SortedDictionary<string, int>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line == "")
                    break;
                var value = line.Split(':').Select(n => n.Trim()).ToArray();

                if (i == num)
                    wires[value[0]] = 1;
                else
                    wires[value[0]] = 0;
            }

            var x = "";
            var y = "";
            for (var i = 0; i < wires.Count / 2; i++)
            {
                var number = i.ToString("D2");
                x = wires["x" + number] + x;
                y = wires["y" + number] + y;
            }

            var zTarget = Convert.ToInt64(x, 2) + Convert.ToInt64(y, 2);


            var queue = new Queue<string>();

            for (var i = wires.Count + 1; i < lines.Length; i++)
            {
                var line = lines[i];
                ParseLine(line);
                queue.Enqueue(line);
            }

            if (num == 0)
                SaveGraphToAFile();

            zWires = Calc(queue, wires);

            var binaryValue = zWires.Values.Reverse().Aggregate("", (current, zValue) => current + zValue);
            var zTargetBinary = Convert.ToString(zTarget, 2).PadLeft(binaryValue.Length, '0');
            PrintIncorrectBits(binaryValue, zTargetBinary);

            return ManuallyDeterminedSwaps(); // :)
        }

        /**
         * Visually determine swaps based on a generated graph in a logic_graph.dot in Graphviz
         * eg. https://dreampuf.github.io/GraphvizOnline/
         */
        private static string ManuallyDeterminedSwaps()
        {
            var dict = new SortedSet<string>();
            dict.Add("fkp"); //6 & 7 bit
            dict.Add("z06");
            dict.Add("ngr"); //11 & 12 bit
            dict.Add("z11");
            dict.Add("mfm"); //31 & 32 bit
            dict.Add("z31");
            dict.Add("krj"); //38 & 39
            dict.Add("bpt");

            var s = "";
            foreach (var d in dict)
            {
                s += d + ",";
            }

            return s.Substring(0, s.Length - 1);
        }

        private static void SaveGraphToAFile()
        {
            var dotContent = GenerateDotFile();
            File.WriteAllText("logic_graph.dot", dotContent);
        }

        private static void PrintIncorrectBits(string binaryValue, string zTargetBinary)
        {
            for (int i = zTargetBinary.Length - 1, bitid = 0; i >= 0; i--, bitid++)
            {
                if (zTargetBinary[i] != binaryValue[i])
                {
                    Console.WriteLine(zTargetBinary + " - target");
                    Console.WriteLine(binaryValue + " - current");
                    Console.WriteLine("Wrong bit: " + bitid + " !!!");
                    Console.WriteLine();
                }
            }
        }

        private static void ParseLine(string line)
        {
            var parts = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
            var leftSide = parts[0].Trim();
            var output = parts[1].Trim();

            var gateParts = leftSide.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var input1 = gateParts[0];
            var gate = gateParts[1];
            var input2 = gateParts[2];

            // Add edges to the graph
            if (!Graph.ContainsKey(input1))
                Graph[input1] = new List<string>();
            if (!Graph.ContainsKey(input2))
                Graph[input2] = new List<string>();
            if (!Graph.ContainsKey(output))
                Graph[output] = new List<string>();

            Graph[input1].Add(output);
            Graph[input2].Add(output);

            LogicGates[output] = gate;
        }

        private static string GenerateDotFile()
        {
            var dot = new StringWriter();
            dot.WriteLine("digraph LogicGraph {");
            dot.WriteLine("    rankdir=LR;"); // Left-to-right orientation
            dot.WriteLine("    node [shape=box];");

            // Add edges
            foreach (var kvp in Graph)
            {
                var from = kvp.Key;
                foreach (var to in kvp.Value)
                {
                    string gate = LogicGates.ContainsKey(to) ? $" [label=\"{LogicGates[to]}\"]" : "";
                    dot.WriteLine($"    \"{from}\" -> \"{to}\"{gate};");
                }
            }

            dot.WriteLine("}");
            return dot.ToString();
        }

        private static SortedDictionary<string, int> Calc(Queue<string> initialQueue, Dictionary<string, int> wires)
        {
            var zWires = new SortedDictionary<string, int>();
            Queue<string> queue = new Queue<string>(initialQueue);
            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                var value = next.Split(' ').Select(n => n.Trim()).ToArray();

                var input1 = value[0];
                var input2 = value[2];
                if (!wires.ContainsKey(input1) || !wires.ContainsKey(input2))
                {
                    queue.Enqueue(next);
                    continue;
                }

                var logicGate = value[1];
                var outputKey = value[4];

                var outputValue = logicGate switch
                {
                    "AND" => wires[input1] & wires[input2],
                    "OR" => wires[input1] | wires[input2],
                    _ => wires[input1] ^ wires[input2]
                };

                wires[outputKey] = outputValue;
                if (outputKey.StartsWith("z"))
                    zWires[outputKey] = outputValue;
            }

            return zWires;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventOfCode2024.Solutions
{
    public static class Day24
    {
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
    }
}
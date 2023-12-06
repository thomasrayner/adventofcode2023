using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day06
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines(Path.GetFullPath(args[0]));
            var time = lines[0]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Skip(1)
                .Select(long.Parse)
                .ToList();
            var distance = lines[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Skip(1)
                .Select(long.Parse)
                .ToList();

            long part1 = 1;

            for (int i = 0; i < time.Count; i++)
            {
                part1 *= DoMath(time[i], distance[i]);
            }

            Console.WriteLine($"P1: {part1}");

            long p2Time = long.Parse(Regex.Replace(lines[0], @"\D", ""));
            long p2Distance = long.Parse(Regex.Replace(lines[1], @"\D", ""));
            long part2 = DoMath(p2Time, p2Distance);

            Console.WriteLine($"P2: {part2}");
        }

        public static long DoMath(long t, long d)
        {
            // thank you Wolfram Alpha for the math
            double disc = Math.Sqrt(t * t - 4 * d);
            var count = (long)Math.Floor((t + disc) / 2) - (long)Math.Ceiling((t - disc) / 2) + 1;
            return count;
        }
    }
}
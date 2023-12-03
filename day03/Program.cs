using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day03
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines(Path.GetFullPath(args[0])).ToArray();
            var numbers = new Dictionary<(int line, int start, int end), string>();
            var symbols = new Dictionary<(int line, int start, int end), string>();

            for (int i = 0; i < lines.Length; i++)
            {
                foreach (var f in FindMatches(lines[i], i, @"\d+"))
                {
                    numbers.Add(f.Key, f.Value);
                }

                foreach (var f in FindMatches(lines[i], i, @"[^\d\.]"))
                {
                    symbols.Add(f.Key, f.Value);
                }
            }

            var part1 = numbers.Where(n => symbols.Any(s => IsAdjacent(n.Key, s.Key))).ToDictionary();
            Console.WriteLine($"P1: {part1.Values.Select(e => int.Parse(e)).Sum()}");

            var part2eligible = symbols.Where(s => numbers.Where(n => IsAdjacent(s.Key, n.Key)).Count() == 2);
            var part2 = 0;
            
            foreach (var p in part2eligible)
            {
                part2 += numbers.Where(n => IsAdjacent(p.Key, n.Key)).ToDictionary().Values.Aggregate(1, (acc, val) => acc * int.Parse(val));
            }

            Console.WriteLine($"P2: {part2}");
        }

        public static Dictionary<(int line, int start, int end), string> FindMatches(string line, int lineNum, string pattern)
        {
            var matches = new Dictionary<(int line, int start, int end), string>();
            var found = Regex.Matches(line, pattern);

            foreach (Match f in found)
            {
                matches.Add((lineNum, f.Index, f.Index + f.Length), f.Value);
            }

            return matches;
        }

        public static bool IsAdjacent((int line, int start, int end) a, (int line, int start, int end) b)
        {
            return a.line - 1 <= b.line && a.line + 1 >= b.line && (a.start <= b.end) && (a.end >= b.start);
        }
    }
}
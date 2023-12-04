using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace day04
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines(Path.GetFullPath(args[0]));
            var matches = new List<(int Quantity, int Score)>();

            foreach (var line in lines)
            {
                var cards = line.Split(":", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1]
                    .Split("|", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var winners = cards[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse);
                var mine = cards[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse);

                matches.Add((1, winners.Intersect(mine).Count()));
            }

            Console.WriteLine($"P1: {matches.Aggregate(0, (acc, val) => acc + (int)Math.Pow(2, val.Score - 1))}");

            for (int i = 0; i < matches.Count; i++)
            {
                for (int j = 1; j <= matches[i].Score; j++)
                {
                    matches[i + j] = (matches[i + j].Quantity + matches[i].Quantity, matches[i + j].Score);
                }
            }

            Console.WriteLine($"P2: {matches.Aggregate(0, (acc, val) => acc += val.Quantity)}");
        }
    }
}
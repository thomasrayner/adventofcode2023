using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day05
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines(Path.GetFullPath(args[0]));
            var seeds = lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToArray();

            var inputMap = new Dictionary<string, int>() {
                {"Seed", 0},
                {"Soil", 0},
                {"Fertilizer", 0},
                {"Water", 0},
                {"Light", 0},
                {"Temp", 0},
                {"Humidity", 0}
            };

            for (int i = 1; i < lines.Length; i++)
            {
                switch (lines[i])
                {
                    case "seed-to-soil map:":
                        inputMap["Seed"] = i;
                        break;
                    case "soil-to-fertilizer map:":
                        inputMap["Soil"] = i;
                        break;
                    case "fertilizer-to-water map:":
                        inputMap["Fertilizer"] = i;
                        break;
                    case "water-to-light map:":
                        inputMap["Water"] = i;
                        break;
                    case "light-to-temperature map:":
                        inputMap["Light"] = i;
                        break;
                    case "temperature-to-humidity map:":
                        inputMap["Temp"] = i;
                        break;
                    case "humidity-to-location map:":
                        inputMap["Humidity"] = i;
                        break;
                }
            }

            var minLoc = long.MaxValue;

            foreach (var seed in seeds)
            {
                long currLoc = seed;
                currLoc = GetNextDestination(currLoc, inputMap["Seed"] + 1, inputMap["Soil"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Soil"] + 1, inputMap["Fertilizer"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Fertilizer"] + 1, inputMap["Water"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Water"] + 1, inputMap["Light"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Light"] + 1, inputMap["Temp"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Temp"] + 1, inputMap["Humidity"] - 1, lines);
                currLoc = GetNextDestination(currLoc, inputMap["Humidity"] + 1, lines.Length - 1, lines);
            
                minLoc = Math.Min(minLoc, currLoc);
            }

            Console.WriteLine($"P1: {minLoc}");

            var ranges = new List<(long, long)>();
            long minRangeLoc = long.MaxValue;

            for (int i = 0; i < seeds.Length; i += 2)
            {
                ranges.Add((seeds[i], seeds[i + 1] - 1));
            }

            var consolidatedRanges = RangeConsolidator.ConsolidateRanges(ranges);

            foreach (var r in consolidatedRanges)
            {
                for (long i = r.Start; i <= r.Start + r.End; i++)
                {
                    long currLoc = i;
                    currLoc = GetNextDestination(currLoc, inputMap["Seed"] + 1, inputMap["Soil"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Soil"] + 1, inputMap["Fertilizer"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Fertilizer"] + 1, inputMap["Water"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Water"] + 1, inputMap["Light"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Light"] + 1, inputMap["Temp"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Temp"] + 1, inputMap["Humidity"] - 1, lines);
                    currLoc = GetNextDestination(currLoc, inputMap["Humidity"] + 1, lines.Length - 1, lines);

                    minRangeLoc = Math.Min(minRangeLoc, currLoc);
                }
            }

            Console.WriteLine($"P2: {minRangeLoc}");
        }

        public static long GetNextDestination(long id, int start, int end, string[] lines)
        {
            for (int lNum = start; lNum < end; lNum++)
            {
                var s = lines[lNum].Split(" ", 3).Select(long.Parse).ToArray();

                if (id >= s[1] && id < s[1] + s[2])
                {
                    return s[0] + (id - s[1]);
                }
            }

            return id;
        }
    }

    public class RangeConsolidator
    {
        public static List<(long Start, long End)> ConsolidateRanges(List<(long Start, long End)> ranges)
        {
            var sortedRanges = ranges.OrderBy(r => r.Start).ToList();
            var consolidatedRanges = new List<(long Start, long End)>();

            (long Start, long End)? currentRange = null;

            foreach (var range in sortedRanges)
            {
                if (currentRange == null)
                {
                    currentRange = range;
                }
                else if (currentRange.Value.End >= range.Start)
                {
                    currentRange = (currentRange.Value.Start, Math.Max(currentRange.Value.End, range.End));
                }
                else
                {
                    consolidatedRanges.Add(currentRange.Value);
                    currentRange = range;
                }
            }

            if (currentRange != null)
            {
                consolidatedRanges.Add(currentRange.Value);
            }

            return consolidatedRanges;
        }
    }
}

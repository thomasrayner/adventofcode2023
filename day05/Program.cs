using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace day05
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines(Path.GetFullPath(args[0]));
            var seeds = lines[0].Split(" ").Skip(1).Select(long.Parse).ToList();
            var maps = new List<List<(long source, long dest, long offset)>>();

            var currentMap = new List<(long source, long dest, long offset)>();
            foreach (var line in lines.Skip(2))
            {
                if (line.EndsWith(":"))
                {
                    currentMap = new List<(long source, long dest, long offset)>();
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line) && currentMap.Count > 0)
                {
                    maps.Add(currentMap);
                    currentMap = new List<(long source, long dest, long offset)>();
                    continue;
                }

                var elements = line.Split(" ").Select(long.Parse).ToArray();
                currentMap.Add((elements[1], elements[1] + elements[2] - 1, elements[0] - elements[1]));
            }

            if (currentMap.Count > 0)
            {
                maps.Add(currentMap);
            }

            var part1 = long.MaxValue;

            foreach (var seed in seeds)
            {
                var t = seed;
                foreach (var map in maps)
                {
                    foreach (var m in map)
                    {
                        if (t >= m.source && t <= m.dest)
                        {
                            t += m.offset;
                            break;
                        }
                    }
                }

                part1 = Math.Min(part1, t);
            }

            Console.WriteLine($"P1: {part1}");

            var seedRanges = new List<(long start, long end)>();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                seedRanges.Add((seeds[i], seeds[i] + seeds[i + 1] - 1));
            }

            foreach (var map in maps)
            {
                var oMap = map.OrderBy(q => q.source).ToList();
                var newSeedRanges = new List<(long start, long end)>();

                foreach (var r in seedRanges)
                {
                    var ra = r;

                    foreach (var om in oMap)
                    {
                        if (ra.start < om.source)
                        {
                            newSeedRanges.Add((ra.start, Math.Min(ra.end, om.source - 1)));
                            ra.start = om.source;

                            if (ra.start > ra.end)
                            {
                                break;
                            }
                        }

                        if (ra.start <= om.dest)
                        {
                            newSeedRanges.Add((ra.start + om.offset, Math.Min(ra.end, om.dest) + om.offset));
                            ra.start = om.dest + 1;

                            if (ra.start > ra.end)
                            {
                                break;
                            }
                        }
                    }

                    if (ra.start <= ra.end)
                    {
                        newSeedRanges.Add(ra);
                    }
                }

                seedRanges = newSeedRanges;
            }

            var part2 = seedRanges.Min(q => q.start);
            Console.WriteLine($"P2: {part2}");
        }
    }
}

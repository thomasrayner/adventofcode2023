using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines(Path.GetFullPath(args[0]));
            var validGames = new List<int>();
            var powers = new List<int>();

            foreach (var line in lines)
            {
                validGames.Add(Part1(line));
                powers.Add(Part2(line));
            }

            Console.WriteLine($"P1: {validGames.Sum()}");
            Console.WriteLine($"P2: {powers.Sum()}");
        }

        public static int Part1(string line)
        {
            var _maxColor = new Dictionary<string, int>() {
                {"red", 12},
                {"green", 13},
                {"blue", 14}
            };

            var parts = line.Split(":");
            int gameId = int.Parse(Regex.Replace(parts[0], @"\D", ""));
            var games = parts[1].Split(";");

            foreach (var game in games)
            {
                var balls = game.Split(",");
                
                foreach (var ball in balls)
                {
                    var count = ball.Trim().Split();

                    if (int.Parse(count[0].Trim()) > _maxColor[count[1]])
                    {
                        return 0;
                    }
                }
            }

            return gameId;
        }

        public static int Part2(string line)
        {
            var parts = line.Split(":");
            var games = parts[1].Split(";");
            var _mins = new Dictionary<string, int>() {
                {"red", 0},
                {"green", 0},
                {"blue", 0}
            };

            foreach (var game in games)
            {
                var balls = game.Split(",");

                foreach (var ball in balls)
                {
                    var count = ball.Trim().Split();

                    if (int.Parse(count[0].Trim()) > _mins[count[1]])
                    {
                        _mins[count[1]] = int.Parse(count[0].Trim());
                    }
                }
            }

            return _mins["red"] * _mins["blue"] * _mins["green"];
        }
    }
}
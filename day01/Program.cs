using System;
using System.IO;
using System.Text.RegularExpressions;

namespace day01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines(Path.GetFullPath(args[0]));
            var p1score = 0;
            var p2score = 0;
            string p2pattern = @"(\d|(zero)|(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine))";
            var numTextMap = new Dictionary<string, string>() {
                {"zero",  "0"},
                {"one", "1"},
                {"two", "2"},
                {"three", "3"},
                {"four", "4"},
                {"five", "5"},
                {"six", "6"},
                {"seven", "7"},
                {"eight", "8"},
                {"nine", "9"}
            };

            foreach (string line in lines)
            {
                var m1 = Regex.Match(line, @"(?<=^\D*)\d");
                var m2 = Regex.Match(line, @"\d(?=\D*$)");
                p1score += int.Parse($"{m1.Value}{m2.Value}");

                var m3 = Regex.Match(line, p2pattern);
                var m4 = Regex.Match(line, p2pattern, RegexOptions.RightToLeft);

                string add3 = numTextMap.ContainsKey(m3.Value) ? numTextMap[m3.Value] : m3.Value;
                string add4 = numTextMap.ContainsKey(m4.Value) ? numTextMap[m4.Value] : m4.Value;

                p2score += int.Parse($"{add3}{add4}");
            }

            Console.WriteLine($"P1: {p1score}");
            Console.WriteLine($"P2: {p2score}");
        }
    }
}
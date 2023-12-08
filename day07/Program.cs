using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

namespace day07
{
    class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines(Path.GetFullPath(args[0]));
            var hands1 = new List<Hand>();
            var hands2 = new List<Hand>();
            long part1 = 0;
            long part2 = 0;

            foreach (var line in lines)
            {
                hands1.Add(new Hand(line, 1));
                hands2.Add(new Hand(line, 2));
            }

            hands1.Sort();
            hands2.Sort();

            for (int i = 0; i < hands1.Count; i++)
            {
                part1 += hands1[i].Bid * (i + 1);
                part2 += hands2[i].Bid * (i + 1);
            }

            Console.WriteLine($"P1: {part1}");
            Console.WriteLine($"P2: {part2}");
        }
    }

    public enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }

    public class Hand : IComparable<Hand>
    {
        public char[] Cards { get; set; }
        public char[] EncodedCards { get; set; }
        public long Bid { get; set; }
        public HandType Score { get; set; }
        public int Part { get; set; }

        public Hand(string Input, int part)
        {
            // encode to make high card comparisons easier
            var encodingMap = new Dictionary<char, char>()
            {
                {'A', 'L'},
                {'K', 'M'},
                {'Q', 'N'},
                {'J', 'O'},
                {'T', 'P'},
                {'9', 'Q'},
                {'8', 'R'},
                {'7', 'S'},
                {'6', 'T'},
                {'5', 'U'},
                {'4', 'V'},
                {'3', 'W'},
                {'2', 'X'},
            };

            Part = part;
            if (Part == 2)
            {
                encodingMap['J'] = 'Z'; // make value of J worth less than all other cards
            }

            var parts = Input.Split(" ");
            Cards = parts[0].ToCharArray();
            Bid = long.Parse(parts[1]);
            EncodedCards = new char[Cards.Length];

            for (int i = 0; i < Cards.Length; i++)
            {
                EncodedCards[i] = encodingMap[Cards[i]];
            }

            var grouped = EncodedCards.GroupBy(x => x).OrderByDescending(x => x.Count());

            if (Part == 2 && grouped.Count() > 1 && grouped.Any(x => x.Key == encodingMap['J']))
            {
                var highest = grouped.First(x => x.Key != encodingMap['J']).Key;
                for (int i = 0; i < EncodedCards.Length; i++)
                {
                    if (EncodedCards[i] == encodingMap['J'])
                    {
                        EncodedCards[i] = highest;
                    }
                }

                grouped = EncodedCards.GroupBy(x => x).OrderByDescending(x => x.Count());
            }

            if (grouped.Count() == 1) // five of a kind
            {
                Score = HandType.FiveOfAKind;
            }
            else if (grouped.Count() == 2) // two kinds of cards, either four of a kind or full house
            {
                if (grouped.First().Count() == 4) // four of a kind
                {
                    Score = HandType.FourOfAKind;
                }
                else // full house
                {
                    Score = HandType.FullHouse;
                }
            }
            else if (grouped.Count() == 3) // three kinds of cards, either two pair or three of a kind
            {
                if (grouped.First().Count() == 3) // three of a kind
                {
                    Score = HandType.ThreeOfAKind;
                }
                else // two pair
                {
                    Score = HandType.TwoPair;
                }
            }
            else if (grouped.Count() == 4) // one pair, four unique cards in a hand of five
            {
                Score = HandType.OnePair;
            }
            else // five unique cards in the hand
            {
                Score = HandType.HighCard;
            }
        }

        public int CompareTo(Hand? other)
        {
            if (Score < other?.Score)
            {
                return 1;
            }
            else if (Score > other?.Score)
            {
                return -1;
            }

            // encode to make high card comparisons easier
            var encodingMap = new Dictionary<char, char>()
            {
                {'A', 'L'},
                {'K', 'M'},
                {'Q', 'N'},
                {'J', 'O'},
                {'T', 'P'},
                {'9', 'Q'},
                {'8', 'R'},
                {'7', 'S'},
                {'6', 'T'},
                {'5', 'U'},
                {'4', 'V'},
                {'3', 'W'},
                {'2', 'X'},
            };

            if (Part == 2)
            {
                encodingMap['J'] = 'Z'; // make value of J worth less than all other cards
            }

            var myCheck = new char[Cards.Length];
            var otherCheck = new char[Cards.Length];


            for (int i = 0; i < Cards.Length; i++)
            {
                myCheck[i] = encodingMap[Cards[i]];
            }

            for (int i = 0; i < Cards.Length; i++)
            {
                otherCheck[i] = encodingMap[other.Cards[i]];
            }

            for (int i = 0; i < myCheck.Length; i++)
            {
                if (myCheck[i] < otherCheck[i])
                {
                    return 1;
                }
                else if (myCheck[i] > otherCheck[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day10
{
    public class SyntaxScoring
    {
        private static Dictionary<char, char> expectedClosingCharactersTable = new Dictionary<char, char>()
        {
            { '{', '}' },
            { '(', ')' },
            { '[', ']' },
            { '<', '>' },
        };

        private static Dictionary<char, int> syntaxErrorScoreTable = new Dictionary<char, int>()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private static Dictionary<char, int> autoCompleteScoreTable = new Dictionary<char, int>()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 },
        };

        public static List<string> ReadInputs(string path)
        {
            var lines = new List<string>();
            var reader = new StreamReader(path);

            try
            {
                do
                {
                    string line = reader.ReadLine();
                    lines.Add(line);
                }
                while (reader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine("reader failed to read: " + e.Message);
            }
            finally
            {
                reader.Close();
            }

            return lines;
        }

        public static int CalculateTotalSyntaxErrorScore(List<string> lines)
        {
            int totalErrorScore = 0;

            foreach(string line in lines)
            {
                char closingBracket = FindFirstInvalidBracket(line);
                int errorScore = (closingBracket == ' ') ? 0 : syntaxErrorScoreTable[closingBracket];

                if (errorScore == 0)
                {
                    Console.WriteLine("error score is 0...");
                }
                totalErrorScore += errorScore;
            }

            return totalErrorScore;
        }

        // returning ' ' means no invalid brackets found in a given line
        private static char FindFirstInvalidBracket(string line)
        {
            var stack = new Stack<char>();
            string openingChars = "{([<";

            foreach (char c in line)
            {
                if (openingChars.Contains(c))
                {
                    stack.Push(c);
                }
                else
                {
                    // Incomplete line
                    if (stack.Count == 0)
                    {
                        Console.WriteLine("Incomplete line");
                        throw new Exception("Incomplete line");
                    }

                    char lastOpenCharacter = stack.Peek();
                    char expectedClosingCharacter = expectedClosingCharactersTable[lastOpenCharacter];
                    if (expectedClosingCharacter == c)
                    {
                        // Pop character off stack and don't store closing character onto stack
                        stack.Pop();
                    }
                    else
                    {
                        // return invalid syntax score for first character that violates the chunk rules
                        return c;
                    }
                }
            }

            return ' ';
        }

        public static long FindMiddleAutoCompleteScore (List<string> lines)
        {
            var scores = new List<long>();
            List<string> incompleteLines = FindIncompleteLines(lines);

            foreach (var incompleteLine in incompleteLines)
            {
                List<char> openingBrackets = GetOpeningBrackets(incompleteLine);
                long score = CalculateAutoCompleteScore(openingBrackets);
                scores.Add(score);
            }

            scores = scores.OrderBy(s => s).ToList();
            return scores[scores.Count / 2];
        }

        private static List<string> FindIncompleteLines(List<string> lines)
        {
            var incompleteLines = new List<string>();

            foreach(var line in lines)
            {
                char closingBracket = FindFirstInvalidBracket(line);
                if (closingBracket == ' ')
                {
                    incompleteLines.Add(line);
                }
            }

            return incompleteLines;
        }

        private static List<char> GetOpeningBrackets(string line)
        {
            var stack = new Stack<char>();
            string openingChars = "{([<";

            foreach (char c in line)
            {
                if (openingChars.Contains(c))
                {
                    stack.Push(c);
                }
                else
                {
                    // Bad Line
                    if (stack.Count == 0)
                    {
                        Console.WriteLine("Bad line");
                        throw new Exception("Bad Line");
                    }

                    stack.Pop();
                }
            }

            // Note: stacks naturally reverse the order in which we received the inputs from
            // when popping e.g. LIFO
            var leftoverChars = new List<char>();
            while(stack.Count > 0)
            {
                leftoverChars.Add(stack.Pop());
            }

            return leftoverChars;
        }

        private static long CalculateAutoCompleteScore(List<char> openingBrackets)
        {
            long totalScore = 0;
            foreach(char openBracket in openingBrackets)
            {
                char closingBracket = expectedClosingCharactersTable[openBracket];
                long score = autoCompleteScoreTable[closingBracket];

                totalScore = totalScore * 5;
                totalScore += score;
            }

            return totalScore;
        }

    }
}

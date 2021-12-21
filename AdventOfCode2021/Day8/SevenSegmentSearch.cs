using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day8
{
    // Facts
    // Don't know what the connections actually are since the display output got damaged
    // after escaping from whale
    // 10 signal patterns are all unique 

    // Note: we don't know what the 10 unique signal patterns map to for sure since
    // the display output is broken

    // Input: single entry example
    // contains 10 unique signal patterns a | delimiter and 4 digit output value

    // Unique signal patterns correspond to 10 different ways the submarine tries
    // to render a digit using current wire/segment connections

    // problem statement: how many times do digits 1,4, 7, or 8 appear?

        // If we have a map of how the digits are expected to be rendered
        // we can count the number of unique elements that make up that digit.
        // for example, we know that 0 is made up of 6 unique characters
        // 2 and 3 both consist of 5 unique characters
        // 0 and 6 both consist of 6 unique characters


        /*
         *  Digit Displayed -> unique character count
         *      0           ->  6
         *      1           ->  2
         *      2           ->  5
         *      4           ->  4
         *      7           ->  3
         *      8           ->  7
         */ 

        /*
         * To count the number of times 1,4, 7, or 8 appear for each entry in the entry list
         *  1. Create an array size 10 (index represents digit displayed | value represents unique character count required to render display)
         *  2. initialize array to contain number of unique character counts for each digit (index)
         *  3. Read problem input using a file reader
         *  3a. go through each line until no more lines are available
         *  3b. for a given line, split string into 2 strings using |, 1 representing unique signal patterns and another representing 4 digit outputs
         *  3c. split unique signal patterns into an array of strings separated by space
         *  3d. split 4 digit output strings into an array of string separated by space
         *  3e. trim any leading or trailing whitespace array containing unique signal patterns or array containing 4 digit output strings may have
         *  3f. add unique signal patterns to a list containing unique signal patterns
         *  3g. add 4 digit output array into a list containing 4 digit output arrays
         *  
         *  4. foreach item in list containing 4 digit output arrays, filter the list down to
         *  digits that are likely to display the numbers 1,4, 7, or 8 by comparing the output digit's
         *  character length against all character lengths that display 1,4,7, or 8
         *  
         *  5. go through list of 4 digit output strings and sum up the total count by
         *  counting the number of remaining items after filtering down the 4 digit outputs to only contain
         *  character lengths that are likely to display a 1,4,7, or 8 based on the unique character count
         */ 

    public class SevenSegmentSearch
    {
        public class BrokenDisplay
        {
            private string[] uniqueSignalPatterns;
            private string[] fourDigitOutput;

            public string[] UniqueSignalPatterns => uniqueSignalPatterns;
            public string[] FourDigitOutput => fourDigitOutput;

            public BrokenDisplay(string[] uniqueSignalPatterns, string[] fourDigitOutput)
            {
                this.uniqueSignalPatterns = uniqueSignalPatterns;
                this.fourDigitOutput = fourDigitOutput;
            }
        }

        public class Position
        {
            private int row;
            public int Row => row;

            private int col;
            public int Col => col;

            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }

        public class StartingCharConnectionPlacement
        {
            private char letter;
            public char Letter => letter;

            private Position position;
            public Position StartPosition => position;

            public StartingCharConnectionPlacement(char letter, Position position)
            {
                this.letter = letter;
                this.position = new Position(position.Row, position.Col);
            }
        }


        public static List<BrokenDisplay> ReadInputs(string path)
        {
            var displays = new List<BrokenDisplay>();
            var reader = new StreamReader(path);

            try
            {
                do
                {
                    string entry = reader.ReadLine();
                    string[] components = entry.Split('|');
                    string[] uniquePatternSignals = components[0].Split(' ');
                    string[] fourDigitOutput = components[1].Split(' ');
                    displays.Add(new BrokenDisplay(
                        uniquePatternSignals.Where(s => s.Length > 0).ToArray(), 
                        fourDigitOutput.Where(s => s.Length > 0).ToArray())
                    );
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

            return displays;
        }

        private static bool ContainsOneFourSevenOrEight(string digit)
        {
            // digit -> unique segment count
            // 1     ->     2
            // 4     ->     4
            // 7     ->     3
            // 8     ->     7
            var uniqueSegmentCounts = new int[] { 2, 4, 3, 7 }.ToList();
            return uniqueSegmentCounts.Contains(digit.Length);
        }

        // part 1 ~ 40 minutes?
        public static int GetOneFourSevenOrEightDigitCount(List<BrokenDisplay> displays)
        {
            int count = 0;

            foreach (var display in displays)
            {
                count += display.FourDigitOutput.Where(digit => ContainsOneFourSevenOrEight(digit)).Count();
            }

            return count;
        }

        // Credit to Riot Nu (https://www.youtube.com/watch?v=EYWJGFbgM4w&t=1148s)
        // for breaking down a possible solution
        // and my friend Alan for helping me out on how to solve the problem
        // returns the sum of all 4 digit decoded outputs
        public static int DecodeDisplayOutputs(List<BrokenDisplay> brokenDisplays)
        {
            /* Expected  Signal Patterns for Display BEFORE wires and connections got mixed up */
            var signalPatternToDigit = new Dictionary<string, int>()
            {
                { "abcefg", 0 },
                { "cf", 1 },
                { "acdeg", 2 },
                { "acdfg", 3 },
                { "bcdf", 4 },
                { "abdfg", 5 },
                { "abdefg", 6 },
                { "acf", 7 },
                { "abcdefg", 8 },
                { "abcdfg", 9 },
            };

            var patternSignalLengthToPossibleSegments = new Dictionary<int, HashSet<char>>();
            patternSignalLengthToPossibleSegments[2] = new HashSet<char>() { 'c', 'f' };
            patternSignalLengthToPossibleSegments[3] = new HashSet<char>() { 'a', 'c', 'f' };
            patternSignalLengthToPossibleSegments[4] = new HashSet<char>() { 'b', 'c', 'd', 'f' };
            patternSignalLengthToPossibleSegments[5] = new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            patternSignalLengthToPossibleSegments[6] = new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            patternSignalLengthToPossibleSegments[7] = new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

            // Maps total references to a given unscrambled segment to possible unscrambled segments
            var totalReferenceCountToSegments = new Dictionary<int, HashSet<char>>()
            {
                { 4, new HashSet<char> { 'e' } },
                { 6, new HashSet<char> { 'b' } },
                { 7, new HashSet<char> { 'd', 'g' } },
                { 8, new HashSet<char> { 'a', 'c' } },
                { 9, new HashSet<char> { 'f' } },
            };
            /* ------------------------------------------------------------------------------------ */

            int answer = 0;

            foreach(var display in brokenDisplays)
            {
                Dictionary<char, char> segmentMappings = DecodeDisplaySegments(
                    display.UniqueSignalPatterns,
                    totalReferenceCountToSegments,
                    patternSignalLengthToPossibleSegments
                );

                // build decoded segment mappings into a string
                int i = 3;
                int subAnswer = 0;
                foreach (string signalPattern in display.FourDigitOutput)
                {
                    var sb = new StringBuilder();
                    string decodedSignalPattern = "";
                    foreach (char segment in signalPattern)
                    {
                        sb.Append(segmentMappings[segment]);
                    }

                    decodedSignalPattern = sb.ToString();
                    decodedSignalPattern = new string(decodedSignalPattern.OrderBy(c => c).ToArray());

                    if (!signalPatternToDigit.ContainsKey(decodedSignalPattern))
                    {
                        Console.WriteLine("Decoded signal pattern: " + decodedSignalPattern + " does not exist in dictionary");
                        throw new Exception("Bad");
                    }

                    int digit = signalPatternToDigit[decodedSignalPattern];
                    // alternatively, I could have done subAnswer = subAnswer * 10 + digit
                    // to move the previous subAnswer's digits to the left by 1
                    subAnswer += (int)Math.Pow(10, i--) * digit;
                }

                Console.WriteLine("sub answer: " + subAnswer);
                answer += subAnswer;
            }

            return answer;
        }

        // Key represents scrambled segment and value represents unscrambled segment
        private static Dictionary<char, char> DecodeDisplaySegments(
            string[] signalPatterns,
            Dictionary<int, HashSet<char>> expectedTotalReferenceCountToSegments,
            Dictionary<int, HashSet<char>> segmentLengthToPossibleSegments
        )
        {
            var searchSpace = new Dictionary<char, HashSet<char>>()
            {
                { 'a', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'b', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'c', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'd', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'e', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'f', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'g', new HashSet<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
            };

            // 1. looks like it passes 1 test case matches whatever output Nu had shown on the video
            searchSpace = FilterBySegmentLength(
                signalPatterns,
                searchSpace,
                segmentLengthToPossibleSegments
            );

            // 2.
            searchSpace = FilterBySegmentTotalReferenceCount(
                signalPatterns,
                searchSpace,
                expectedTotalReferenceCountToSegments
            );


            // 3.
            searchSpace = FilterByKnownSegments(searchSpace);

            //4. Convert value set to 1 char value
            var result = new Dictionary<char, char>();
            foreach (var scrambledSegment in searchSpace.Keys)
            {
                if (searchSpace[scrambledSegment].Count != 1)
                {
                    Console.WriteLine("this segment: " + scrambledSegment + "-> " + string.Join(',', searchSpace[scrambledSegment].ToArray()) + " has more than 1 possibility");
                    throw new Exception("Bad");
                }
                result[scrambledSegment] = searchSpace[scrambledSegment].First();
            }

            return result;
        }

        private static Dictionary<char, HashSet<char>> FilterBySegmentLength(
            string[] signalPatterns,
            Dictionary<char, HashSet<char>> searchSpace,
            Dictionary<int, HashSet<char>> segmentLengthToPossibleSegments

        )
        {
            foreach (string signalPattern in signalPatterns)
            {
                int patternLength = signalPattern.Length;
                foreach (char segment in signalPattern)
                {
                    var possibleSegments = searchSpace[segment];
                    searchSpace[segment] = possibleSegments
                        .Intersect(segmentLengthToPossibleSegments[patternLength])
                        .ToHashSet();
                }
            }

            return searchSpace;
        }


        private static Dictionary<char, HashSet<char>> FilterBySegmentTotalReferenceCount(
            string[] signalPatterns,
            Dictionary<char, HashSet<char>> searchSpace,
            Dictionary<int, HashSet<char>> expectedTotalReferenceCountToSegments
        )
        {
            var segmentToReferenceCount = new Dictionary<char, int>();
            foreach (string pattern in signalPatterns)
            {
                foreach (char segment in pattern)
                {
                    if (segmentToReferenceCount.ContainsKey(segment))
                    {
                        ++segmentToReferenceCount[segment];
                    }
                    else
                    {
                        segmentToReferenceCount[segment] = 1;
                    }
                }
            }

            for (char segment = 'a'; segment <= 'g'; ++segment)
            {
                int segmentCount = segmentToReferenceCount[segment];
                HashSet<char> possibleSegments = searchSpace[segment];
                
                searchSpace[segment] = possibleSegments
                    .Intersect(expectedTotalReferenceCountToSegments[segmentCount])
                    .ToHashSet();
            }

            return searchSpace;
        }

        private static Dictionary<char, HashSet<char>> FilterByKnownSegments(
            Dictionary<char, HashSet<char>> searchSpace
        )
        {
            for (char segment = 'a'; segment <= 'g'; ++segment)
            {
                var possibleSegments = searchSpace[segment].ToList();
                // Remove segments from other sub search spaces if current search space has exactly 1 to 1 mapping
                if (possibleSegments.Count != 1)
                {
                    continue;
                }

                char solvedSegment = possibleSegments[0];
                for (char segmentToRemove = 'a'; segmentToRemove <= 'g'; ++segmentToRemove)
                {
                    if (segment == segmentToRemove)
                    {
                        continue;
                    }

                    // Eliminates solved segment for all other search spaces that share the same possible segment
                    searchSpace[segmentToRemove].Remove(solvedSegment);
                }
            }

            return searchSpace;
        }


        // Key represents segment and value represents number of references the key has in signal patterns    
        private static Dictionary<char, int> GetSegmentToReferenceCount(string[] signalPatterns)
        {
            var segmentToReferenceCount = new Dictionary<char, int>();

            foreach (string pattern in signalPatterns)
            {
                foreach (char segment in pattern)
                {
                    if (segmentToReferenceCount.ContainsKey(segment))
                    {
                        ++segmentToReferenceCount[segment];
                    }
                    else
                    {
                        segmentToReferenceCount[segment] = 1;
                    }
                }
            }

            return segmentToReferenceCount;
        }
    }
}

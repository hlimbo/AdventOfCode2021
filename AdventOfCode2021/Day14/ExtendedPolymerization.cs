using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day14
{
    public class ExtendedPolymerization
    {
        public class Polymer
        {
            public string source;
            public Dictionary<string, char> insertionPairRules;
            
            public Polymer(string source, Dictionary<string, char> insertionPairRules)
            {
                this.source = source;
                this.insertionPairRules = insertionPairRules;
            }
        }

        public static Polymer ReadInputs(string path)
        {
            var reader = new StreamReader(path);
            string sourcePolymer = "";
            var insertionPairRules = new Dictionary<string, char>();

            try
            {
                do
                {
                    string line = reader.ReadLine();
                    
                    if (sourcePolymer.Length == 0)
                    {
                        sourcePolymer = line;
                    }
                    else
                    {
                        if (line.Trim().Length == 0)
                        {
                            continue;
                        }

                        string[] rulePair = line.Split("->").Select(item => item.Trim()).ToArray();
                        insertionPairRules[rulePair[0]] = char.Parse(rulePair[1]);
                    }
                }
                while (reader.Peek() != -1);
            }
            catch(Exception e)
            {
                Console.WriteLine("ReadInputs bad path: " + e.Message);
            }
            finally
            {
                reader.Close();
            }

            return new Polymer(sourcePolymer, insertionPairRules);
        }

        public static int CalculateDiffBetweenMostCommonAndLeastCommonElements(string polymer)
        {
            // identify most common and least common elements
            var elementOccurrenceTable = new Dictionary<char, int>();

            foreach (char element in polymer)
            {
                if (!elementOccurrenceTable.ContainsKey(element))
                {
                    elementOccurrenceTable[element] = 1;
                }
                else
                {
                    ++elementOccurrenceTable[element];
                }
            }

            int maxOccurrence = 0;
            int minOccurrence = int.MaxValue;
            foreach (int occurrence in elementOccurrenceTable.Values)
            {
                maxOccurrence = Math.Max(maxOccurrence, occurrence);
                minOccurrence = Math.Min(minOccurrence, occurrence);
            }

            return maxOccurrence - minOccurrence;
        }

        // Note: this function only works up to maximum supported length of an integer value
        public static string ExtendPolymer(Polymer polymer, int steps)
        {
            string result = polymer.source;
            var insertionPairRules = polymer.insertionPairRules;

            for (int j = 0;j < steps; ++j)
            {
                var newElementsToInsert = new Dictionary<int, char>();

                for (int i = 1; i < result.Length; ++i)
                {
                    string pair = result.Substring(i - 1, 2);
                    
                    if (insertionPairRules.ContainsKey(pair))
                    {
                        newElementsToInsert[i] = insertionPairRules[pair];
                    }
                }

                var sourceQueue = new Queue<char>();
                foreach (char element in result)
                {
                    sourceQueue.Enqueue(element);
                }

                // construct new result using a string builder
                var sb = new StringBuilder();
                int counter = 0;
                while (sourceQueue.Count > 0)
                {
                    char currElement = sourceQueue.Dequeue();

                    if (newElementsToInsert.ContainsKey(counter))
                    {
                        sb.Append(newElementsToInsert[counter]);
                        sb.Append(currElement);
                    }
                    else
                    {
                        sb.Append(currElement);
                    }

                    ++counter;
                }

                result = sb.ToString();
            }

            return result;
        }

        // returns difference between most common and least common elements after
        // polymer has been extended for X steps
        public static long BuildPolymer(Polymer polymer, int steps)
        {
            // The idea here is to generate a lookup table of element pairs 
            // and number of times element pairs occur as a key value pair

            // Since element pairs in a growing polymer can be duplicates of each other
            // using a dictionary here would be good alternative as opposed to a string
            // since strings only can hold up to the max capacity of an Int

            var elementPairs = new Dictionary<string, long>();
            
            for (int i = 1;i < polymer.source.Length; ++i)
            {
                string currentPair = polymer.source.Substring(i - 1, 2);
                if (elementPairs.ContainsKey(currentPair))
                {
                    ++elementPairs[currentPair];
                }
                else
                {
                    elementPairs[currentPair] = 1;
                }
            }

            for (int i = 0;i < steps; ++i)
            {
                // build new pairs dictionary
                var newElementPairs = new Dictionary<string, long>();
                foreach (var pairKvp in elementPairs)
                {
                    string pair = pairKvp.Key;
                    long count = pairKvp.Value;

                    if (polymer.insertionPairRules.ContainsKey(pair))
                    {
                        char newElement = polymer.insertionPairRules[pair];
                        string firstPair = new string(new char[] { pair[0], newElement });
                        string secondPair = new string(new char[] { newElement, pair[1] });

                        if (!newElementPairs.ContainsKey(firstPair))
                        {
                            newElementPairs[firstPair] = 0;
                        }
                        // why add by count?
                        newElementPairs[firstPair] += count;

                        if (!newElementPairs.ContainsKey(secondPair))
                        {
                            newElementPairs[secondPair] = 0;
                        }
                        newElementPairs[secondPair] += count;
                    }
                    else
                    {
                        if (!newElementPairs.ContainsKey(pair))
                        {
                            newElementPairs[pair] = count;
                        }
                    }
                }

                elementPairs = newElementPairs;
            }

            // count number of times an element occurs in element pairs
            var elementOccurrences = new Dictionary<char, long>();
            foreach (var kvp in elementPairs)
            {
                // since letters overlap if reconstructed as a string, only grab the 2nd element to prevent
                // double counting
                char element = kvp.Key[1];
                long count = kvp.Value;

                if (!elementOccurrences.ContainsKey(element))
                {
                    elementOccurrences[element] = 0;
                }
                elementOccurrences[element] += count;
            }

            // calculate min and max element occurrences and take their difference
            long minOccurrences = long.MaxValue;
            long maxOccurrences = long.MinValue;

            foreach (long occurrences in elementOccurrences.Values)
            {
                minOccurrences = Math.Min(minOccurrences, occurrences);
                maxOccurrences = Math.Max(maxOccurrences, occurrences);
            }

            return maxOccurrences - minOccurrences;
        }
    }
}

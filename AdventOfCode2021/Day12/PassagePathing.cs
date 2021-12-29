using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day12
{
    public class PassagePathing
    {
        public enum NodeType
        {
            START, END, BIG_CAVE, SMALL_CAVE,
        };

        public class GraphNode
        {
            public string name;
            public NodeType nodeType;
            public bool isVisited;

            public List<GraphNode> adjacentNodes;

            public GraphNode(string name)
            {
                this.name = name;
                isVisited = false;
                DetermineNodeType();
                adjacentNodes = new List<GraphNode>();
            }

            public bool ContainsAdjacentNodes()
            {
                return adjacentNodes.Count > 0;
            }

            private void DetermineNodeType()
            {
                if (IsUpper(name))
                {
                    nodeType = NodeType.BIG_CAVE;
                }
                else if (name == "start")
                {
                    nodeType = NodeType.START;
                }
                else if (name == "end")
                {
                    nodeType = NodeType.END;
                }
                else
                {
                    nodeType = NodeType.SMALL_CAVE;
                }
            }

            private bool IsUpper(string s)
            {
                bool result = true;
                foreach (char c in s)
                {
                    if (!char.IsUpper(c))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        public static List<string> ReadInputs(string path)
        {
            var reader = new StreamReader(path);
            List<string> connections = new List<string>();

            try
            {
                do
                {
                    string line = reader.ReadLine();
                    connections.Add(line);
                }
                while (reader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad: " + e.Message);
            }
            finally
            {
                reader.Close();
            }

            return connections;
        }

        // key -> source cave
        // value -> set of adjacent outgoing caves
        public static Dictionary<string, HashSet<string>> ConstructCaveSystem(List<string> connections)
        {
            var caveSystem = new Dictionary<string, HashSet<string>>();

            foreach (string connection in connections)
            {
                string[] caves = connection.Split('-');
                
                if (caveSystem.ContainsKey(caves[0]))
                {
                    caveSystem[caves[0]].Add(caves[1]);
                }
                else
                {
                    caveSystem[caves[0]] = new HashSet<string>() { caves[1] };
                }

                if (caveSystem.ContainsKey(caves[1]))
                {
                    caveSystem[caves[1]].Add(caves[0]);
                }
                else
                {
                    caveSystem[caves[1]] = new HashSet<string>() { caves[0] };
                }
            }

            return caveSystem;
        }

        // Special Thanks to Riot Nu's solution: https://github.com/RiotNu/advent-of-code/blob/main/AdventOfCode/2021/Puzzle12.cpp
        // I need to re-review this problem in the future as this was a hard one
        public static int CountUniquePaths(Dictionary<string, HashSet<string>> caveSystem, bool canRevisit1SmallCave)
        {
            // Represents the number of distinct paths the algorithm will find in the caveSystem
            var finishedPaths = new List<List<string>>();
            // Represents the paths discovered in the previous step
            var previousPaths = new List<List<string>>();
            previousPaths.Add(new List<string>() { "start" });

            bool isSearchComplete = false;

            while(!isSearchComplete)
            {
                // assume the search is already complete given that there are 
                // either no more connected caves to traverse or when there are no more previousPaths to traverse
                isSearchComplete = true;

                // Represents paths that will be discovered while running the following 2 for loops below
                var currentPaths = new List<List<string>>();

                foreach(var previousPath in previousPaths)
                {
                    // get the last cave visited
                    string currentLocation = previousPath[previousPath.Count - 1];
                    // get all adjacent caves
                    HashSet<string> connectedCaves = caveSystem[currentLocation];
                    foreach (var cave in connectedCaves)
                    {
                        if (IsStart(cave))
                        {
                            continue;
                        }

                        // found a path to end
                        if (IsEnd(cave))
                        {
                            List<string> finishedPath = new List<string>(previousPath);
                            finishedPath.Add(cave);
                            finishedPaths.Add(finishedPath);
                            continue;
                        }

                        if (IsAllLowercase(cave))
                        {
                            // find out if cave is already visited..
                            bool isAlreadyVisited = false;
                            foreach (var prevCave in previousPath)
                            {
                                if (prevCave == cave)
                                {
                                    isAlreadyVisited = true;
                                    break;
                                }
                            }

                            // if cave is small and is already visited ...
                            if (isAlreadyVisited)
                            {
                                // if cannot revisit 1 small cave, skip
                                if (!canRevisit1SmallCave)
                                {
                                    continue;
                                }

                                // Find out if a small cave was already visited in previous path
                                bool didAlreadyRevisitASmallCave = false;
                                for (int i = 1;i < previousPath.Count; ++i)
                                {
                                    string previousCave = previousPath[i];
                                    if (IsAllLowercase(previousCave))
                                    {
                                        for (int j = i + 1;j < previousPath.Count; ++j)
                                        {
                                            if (previousCave == previousPath[j])
                                            {
                                                didAlreadyRevisitASmallCave = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                // if a small cave was already visited in the previous path 
                                // skip this cave
                                if (didAlreadyRevisitASmallCave)
                                {
                                    continue;
                                }
                            }
                        }

                        // Could be an all uppercase cave or an all lowercase cave
                        // depends on the conditions inside of if(IsAllLowercase(cave)) 
                        var currentPath = new List<string>(previousPath);
                        // add new cave as part of the path
                        currentPath.Add(cave);
                        currentPaths.Add(currentPath);
                        // search is not complete.. we only know it is complete when 
                        // there are either no more previous paths to traverse from
                        // OR when all previous paths don't have connected caves
                        isSearchComplete = false;

                    }
                }

                // update previous paths to the newly found caves added 
                // in currentPaths
                previousPaths = new List<List<string>>(currentPaths);

            }

            return finishedPaths.Count;
        }

        private static bool IsStart(string cave)
        {
            return cave == "start";
        }

        private static bool IsEnd(string cave)
        {
            return cave == "end";
        }

        private static bool IsAllLowercase(string cave)
        {
            bool isAllLower = true;
            foreach(char letter in cave)
            {
                if (!char.IsLower(letter))
                {
                    isAllLower = false;
                    break;
                }
            }

            return isAllLower;
        }
    }
}

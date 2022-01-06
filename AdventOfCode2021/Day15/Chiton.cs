using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2021.Day15
{
    public class Chiton
    {
        public static List<List<int>> ReadInputs(string path)
        {
            var reader = new StreamReader(path);

            var riskLevelMap = new List<List<int>>();

            try
            {
                do
                {
                    string line = reader.ReadLine();
                    var riskLevelRow = line.Select(c => (int)char.GetNumericValue(c)).ToList();
                    riskLevelMap.Add(riskLevelRow);
                }
                while (reader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            return riskLevelMap;
        }

        // Greedy way doesn't always work....could have high risk values in the beginning but 
        // towards end of the path through discovery, we can find several low risk values making some
        // path the min risk level out of all paths
        public static int FindLowestRiskPath(List<List<int>> riskLevelMap)
        {
            int rowCount = riskLevelMap.Count;
            int colCount = riskLevelMap.Count;

            int score = 0;
            int row = 0;
            int col = 0;
            while (row + 1 < rowCount && col + 1 < colCount)
            {
                int rightScore = riskLevelMap[row][col + 1];
                int botScore = riskLevelMap[row + 1][col];

                // greedy way
                score = score + Math.Min(rightScore, botScore);

                if (rightScore < botScore)
                {
                    ++col;
                }
                else if (rightScore >= botScore)
                {
                    ++row;
                }
            }

            return score;
        }

        // Passes on small input but fails on big input
        public static int FindLowestRiskPath(List<List<int>> riskLevelMap, int row, int col, int riskScore = 0, bool isEntry = true)
        {
            int rowCount = riskLevelMap.Count;
            // assume all columns have equal length
            int colCount = riskLevelMap[riskLevelMap.Count - 1].Count;

            // found 1 path
            if (row == rowCount && col == colCount)
            {
                Console.WriteLine("path found: " + riskScore);
                return riskScore;
            }

            // out of bounds
            if (row >= rowCount || col <= colCount)
            {
                return riskScore;
            }

            int currentScore = isEntry ? 0 : riskLevelMap[row][col];
            int rightPathScore = riskScore + FindLowestRiskPath(riskLevelMap, row, col + 1, currentScore, false);
            int botPathScore = riskScore + FindLowestRiskPath(riskLevelMap, row + 1, col, currentScore, false);

            int combinedPathScore = rightPathScore + botPathScore;
            return combinedPathScore;
        }

        // Let's try bottom-up approach Dynamic Programming to solve this problem
        // Works on small input but fails on big input :(
        public static int BottomUp(List<List<int>> riskLevelMap)
        {
            // create a list of list of ints that mirror the coordinate space of risk level map
            // each cell will hold the lowest risk level found so far....
            int rowCount = riskLevelMap.Count;
            int colCount = riskLevelMap[rowCount - 1].Count;
            var totalRiskCosts = new int[rowCount, colCount];

            // first row 2nd column
            totalRiskCosts[0, 1] = riskLevelMap[0][1];
            // 2nd row 1st column
            totalRiskCosts[1, 0] = riskLevelMap[1][0];

            // build first row risk costs - Bad Assumption I made here was that the first row risk costs will all be the lowest
            for (int c = 2;c < colCount; ++c)
            {
                totalRiskCosts[0, c] = riskLevelMap[0][c] + totalRiskCosts[0, c - 1];
            }

            // build first column risk costs - Bad Assumption I made here was that the first column risk costs will all be the lowest
            for (int r = 2;r < rowCount; ++r)
            {
                totalRiskCosts[r, 0] = riskLevelMap[r][0] + totalRiskCosts[r - 1, 0];
            }

            for (int r = 1; r < rowCount; ++r)
            {
                for (int c = 1; c < colCount; ++c)
                {
                    int currentRiskCost = riskLevelMap[r][c];
                    int leftCost = currentRiskCost + totalRiskCosts[r, c - 1];
                    int topCost = currentRiskCost + totalRiskCosts[r - 1, c];
                    totalRiskCosts[r, c] = Math.Min(leftCost, topCost);
                }
            }

            return totalRiskCosts[rowCount - 1, colCount - 1];
        }

        public class Vertex
        {
            public int row, col;
            public Vertex (int row, int col)
            {
                this.row = row;
                this.col = col;
            }

            public override bool Equals(object obj)
            {
                bool isVertexType = obj is Vertex;
                var other = obj as Vertex;

                return isVertexType && row == other.row && col == other.col;
            }

            public override int GetHashCode()
            {
                return 31 * row + 17 * col;
            }
        }

        // used for Collection classes like Dictionary and HashSet since these collections
        // use comparers internally to book keep the internals of the data structure
        private class VertexEqualityComparer: IEqualityComparer<Vertex>
        {
            public bool Equals(Vertex a, Vertex b)
            {
                return a.row == b.row && a.col == b.col;
            }

            public int GetHashCode(Vertex v)
            {
                return 31 * v.row + 17 * v.col;
            }
        }

        public class DebugPath
        {
            public Dictionary<Vertex, Vertex> prevVertex;
            public Vertex endVertex;
            public int riskCost;

            public DebugPath(Dictionary<Vertex, Vertex> prevVertex,Vertex endVertex, int riskCost)
            {
                this.riskCost = riskCost;
                this.endVertex = endVertex;
                this.prevVertex = prevVertex;
            }
        }

        public static DebugPath Dijkstra(List<List<int>> riskLevelMap)
        {
            var vertexComparer = new VertexEqualityComparer();

            // key represents destination node and value represents total risk cost it took to reach key vertex
            var distances = new Dictionary<Vertex, int>(vertexComparer);

            // key represents current node and value represents previous node current node is connected to
            var previousNode = new Dictionary<Vertex, Vertex>(vertexComparer);

            // use a priority queue here because we want to prioritize removing elements from the queue with the lowest risk cost first
            var priorityQueue = new PriorityQueue<Vertex, int>();

            var startVertex = new Vertex(0, 0);

            // initialize all locations excluding startVertex to have infinite distance (e.g. int.MaxValue)
            int rowCount = riskLevelMap.Count;
            int colCount = riskLevelMap[rowCount - 1].Count;

            for (int r = 0;r < rowCount; ++r)
            {
                for (int c = 0;c < colCount; ++c)
                {
                    var vertex = new Vertex(r, c);
                    distances[vertex] = int.MaxValue;
                    previousNode[vertex] = null;
                }
            }

            // overwrite starting index to have risk cost of 0 as you start at this location and only need to pay risk cost if you enter the location
            distances[startVertex] = 0;
            priorityQueue.Enqueue(startVertex, 0);

            while (priorityQueue.Count > 0)
            {
                var vertex = priorityQueue.Dequeue();

                List<Vertex> neighbors = GetNeighbors(vertex, rowCount, colCount);
                foreach (Vertex neighbor in neighbors)
                {
                    int newRiskCost = distances[vertex] + riskLevelMap[vertex.row][vertex.col];
                    // if a new path is found with a lower risk cost
                    // pick this path over the old path and update risk cost, previous node connection
                    // and add neighbor back to priority queue
                    if (newRiskCost < distances[neighbor])
                    {
                        distances[neighbor] = newRiskCost;
                        previousNode[neighbor] = vertex;
                        priorityQueue.Enqueue(neighbor, newRiskCost);
                    }
                }

            }

            // possibly calculate the min risk cost by backtracking through the path using previous node to add up all risks together
            // remember to exclude the top left corner of the map since that is where the submarine starts on

            var endVertex = new Vertex(rowCount - 1, colCount - 1);
            return new DebugPath(previousNode, endVertex, distances[endVertex]);
        }

        private static List<Vertex> GetNeighbors(Vertex source, int rowCount, int colCount)
        {
            var neighbors = new List<Vertex>();

            // left
            if (source.col - 1 > 0)
            {
                var vertex = new Vertex(source.row, source.col - 1);
                neighbors.Add(vertex);
            }

            // right
            if (source.col + 1 < colCount)
            {
                var vertex = new Vertex(source.row, source.col + 1);
                neighbors.Add(vertex);
            }

            // top
            if (source.row - 1 > 0)
            {
                var vertex = new Vertex(source.row - 1, source.col);
                neighbors.Add(vertex);
            }

            // bottom
            if (source.row + 1 < rowCount)
            {
                var vertex = new Vertex(source.row + 1, source.col);
                neighbors.Add(vertex);
            }

            return neighbors;
        }

        public static int[,] ExtendRiskTilesToFormCaveMap(List<List<int>> originalRiskTile, int tileMultiplier)
        {
            int rowCount = originalRiskTile.Count;
            int colCount = originalRiskTile[rowCount - 1].Count;
            int[,] caveSystem = new int[rowCount * tileMultiplier, colCount * tileMultiplier];

            for (int tileRow = 0;tileRow < tileMultiplier; ++tileRow)
            {
                for (int tileCol = 0;tileCol < tileMultiplier; ++tileCol)
                {
                    // copy original risk tile to first tile slot
                    if (tileRow == 0 && tileCol == 0)
                    {
                        for (int r = 0;r < rowCount; ++r)
                        {
                            for (int c = 0;c < colCount; ++c)
                            {
                                caveSystem[r,c] = originalRiskTile[r][c];
                            }
                        }
                    }
                    else
                    {
                        // increment risk level for adjacent tile based on the previous tile's risk values
                        for (int r = tileRow * rowCount; r < (tileRow + 1) * rowCount; ++r)
                        {
                            for (int c = tileCol * colCount; c < (tileCol + 1) * colCount; ++c)
                            {
                                // identify previous tile (tileRow, tileCol)
                                if (tileRow - 1 >= 0)
                                {
                                    int prevRiskCost = caveSystem[r - rowCount, c];
                                    int newRiskCost = (prevRiskCost + 1) % 10 == 0 ? 1 : prevRiskCost + 1;
                                    caveSystem[r, c] = newRiskCost;
                                }
                                else if (tileCol - 1 >= 0)
                                {
                                    int prevRiskCost = caveSystem[r, c - colCount];
                                    int newRiskCost = (prevRiskCost + 1) % 10 == 0 ? 1 : prevRiskCost + 1;
                                    caveSystem[r, c] = newRiskCost;
                                }
                            }
                        }
                    }
                }
            }

            return caveSystem;
        }

        public static List<List<int>> ConvertJaggedArrayToListOfLists(int[,] caveSystem)
        {
            var newCaveSystem = new List<List<int>>();

            int rowCount = caveSystem.GetLength(0);
            int colCount = caveSystem.GetLength(1);

            for (int r = 0; r < rowCount; ++r)
            {
                var row = new List<int>();
                for (int c = 0; c < colCount; ++c)
                {
                    row.Add(caveSystem[r, c]);
                }
                newCaveSystem.Add(row);
            }

            return newCaveSystem;
        }
    }
}

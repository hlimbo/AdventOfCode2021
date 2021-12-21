using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day9
{
    public class SmokeBasin
    {
        public static List<List<int>> ReadInputs(string path)
        {
            var heightMap = new List<List<int>>();
            var reader = new StreamReader(path);

            try
            {
                do
                {
                    string row = reader.ReadLine();
                    char[] charRow = row.Where(char.IsDigit).ToArray();
                    var newRow = new List<int>();
                    foreach (char d in charRow)
                    {
                        newRow.Add(int.Parse(d.ToString()));
                    }

                    heightMap.Add(newRow);
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

            return heightMap;
        }

        // Part 1
        /*
         * Input element range
         * 0 - lowest height
         * 9 - highest height
         * 
         * Go through all possible values in the height map
         * check if the top, bottom, left, or right adjacent values are all greater than the value currently visiting
         * if above is true, add this value to minHeightMap array, if false, skip and go to next value to the right
         * 
         * Once all min height values have been found, take the sum of all the min height values + length of the array 
         * (since we add 1 to every single min height value found) to calculate the answer
         * 
         */
        public static int CalculateRiskValueFromLowPoints(List<List<int>> heightMap)
        {
            var lowPoints = new List<int>();

            for (int i = 0; i < heightMap.Count; ++i)
            {
                for (int j = 0; j < heightMap[i].Count; ++j)
                {
                    int currentValue = heightMap[i][j];

                    bool isAdjacentBotValueBigger = false;
                    bool isAdjacentTopValueBigger = false;
                    bool isAdjacentLeftValueBigger = false;
                    bool isAdjacentRightValueBigger = false;

                    if (i + 1 >= heightMap.Count || (i + 1 < heightMap.Count && heightMap[i + 1][j] > currentValue))
                    {
                        isAdjacentBotValueBigger = true;
                    }

                    if (i - 1 < 0 || (i - 1 >= 0 && heightMap[i - 1][j] > currentValue))
                    {
                        isAdjacentTopValueBigger = true;
                    }

                    if (j - 1 < 0 || (j - 1 >= 0 && heightMap[i][j - 1] > currentValue))
                    {
                        isAdjacentLeftValueBigger = true;
                    }

                    if (j + 1 >= heightMap[i].Count || (j + 1 < heightMap[i].Count && heightMap[i][j + 1] > currentValue))
                    {
                        isAdjacentRightValueBigger = true;
                    }

                    if (isAdjacentBotValueBigger && isAdjacentTopValueBigger && isAdjacentLeftValueBigger && isAdjacentRightValueBigger)
                    {
                        lowPoints.Add(currentValue);
                    }

                }
            }

            return lowPoints.Sum() + lowPoints.Count;
        }

        private static List<int[]> FindLowPointLocationsFromHeightMap(List<List<int>> heightMap)
        {
            var lowPoints = new List<int[]>();

            for (int i = 0; i < heightMap.Count; ++i)
            {
                for (int j = 0; j < heightMap[i].Count; ++j)
                {
                    int currentValue = heightMap[i][j];

                    bool isAdjacentBotValueBigger = false;
                    bool isAdjacentTopValueBigger = false;
                    bool isAdjacentLeftValueBigger = false;
                    bool isAdjacentRightValueBigger = false;

                    if (i + 1 >= heightMap.Count || (i + 1 < heightMap.Count && heightMap[i + 1][j] > currentValue))
                    {
                        isAdjacentBotValueBigger = true;
                    }

                    if (i - 1 < 0 || (i - 1 >= 0 && heightMap[i - 1][j] > currentValue))
                    {
                        isAdjacentTopValueBigger = true;
                    }

                    if (j - 1 < 0 || (j - 1 >= 0 && heightMap[i][j - 1] > currentValue))
                    {
                        isAdjacentLeftValueBigger = true;
                    }

                    if (j + 1 >= heightMap[i].Count || (j + 1 < heightMap[i].Count && heightMap[i][j + 1] > currentValue))
                    {
                        isAdjacentRightValueBigger = true;
                    }

                    if (isAdjacentBotValueBigger && isAdjacentTopValueBigger && isAdjacentLeftValueBigger && isAdjacentRightValueBigger)
                    {
                        lowPoints.Add(new int[] { i, j });
                    }

                }
            }

            return lowPoints;
        }
        /* Part 2
         * 
         * To find a basin, treat 9s in the height maps as walls that separate it from other basins
         * A basin contains all digits that are less than 9 but greater than or equal to 0'
         * The size of a basin is the number of locations whose height value is less than 9 but greater than or equal to 0
         * 
         * 
         */
         
            // my bfs is breaking why?
        private static int GetBasinSize(int row, int col, List<List<int>> heightMap, bool[,] isVisited)
        {
            if (row < 0 || row >= heightMap.Count || col < 0 || col >= heightMap[0].Count)
            {
                return 0;
            }

            if (isVisited[row, col] == true)
            {
                return 0;
            }

            isVisited[row, col] = true;

            // 9 does not count as part of a basin
            if (heightMap[row][col] == 9)
            {
                return 0;
            }

            return 1
                + GetBasinSize(row + 1, col, heightMap, isVisited)
                + GetBasinSize(row - 1, col, heightMap, isVisited)
                + GetBasinSize(row, col + 1, heightMap, isVisited)
                + GetBasinSize(row, col - 1, heightMap, isVisited);
        }

        public static int GetLargestBasinsProduct(List<List<int>> heightMap)
        {
            List<int[]> lowPointLocations = FindLowPointLocationsFromHeightMap(heightMap);
            List<int> basinSizes = new List<int>();

            foreach (var location in lowPointLocations)
            {
                var isVisited = new bool[heightMap.Count, heightMap[0].Count];
                int basinSize = GetBasinSize(location[0], location[1], heightMap, isVisited);
                basinSizes.Add(basinSize);
            }

            int[] threeLargestBasins = basinSizes.OrderByDescending(basinSize => basinSize).Take(3).ToArray();
            int product = threeLargestBasins[0] * threeLargestBasins[1] * threeLargestBasins[2];

            return product;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day11
{
    public class DumboOctopus
    {
        public class Octopus
        {
            private int energyLevel;
            public int EnergyLevel => energyLevel;
            public bool hasFlashed;

            public Octopus(int energyLevel)
            {
                hasFlashed = false;
                this.energyLevel = energyLevel;
            }

            public void IncrementEnergyLevel()
            {
                energyLevel += 1;
            }

            public void Reset()
            {
                energyLevel = 0;
                hasFlashed = false;
            }
        }

        public static List<List<Octopus>> ReadInputs(string path)
        {
            var reader = new StreamReader(path);
            var inputs = new List<List<Octopus>>();

            try
            {
                do
                {
                    string line = reader.ReadLine();
                    List<Octopus> octopuses = line
                        .Select(c => int.Parse(c.ToString()))
                        .Select(energyLevel => new Octopus(energyLevel))
                        .ToList();

                    inputs.Add(octopuses);
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

            return inputs;
        }

        public static int CalculateFlashCount(int steps, List<List<Octopus>> inputs)
        {
            int flashCount = 0;

            for (int i = 0;i < steps; ++i)
            {
                IncrementEnergyLevels(inputs);
                List<int[]> locations = CheckOctopusesEnergyLevels(inputs);
                // Note: this gives us the number of octopuses that will flash this step
                flashCount += locations.Count;

                Queue<int[]> pendingLocations = new Queue<int[]>();
                List<int[]> completedLocations = new List<int[]>();

                foreach(var location in locations)
                {
                    pendingLocations.Enqueue(location);
                }

                /*
                 *  1. for each flash location, increment every adjacent location's energy level by 1
                 *  2. collect every adjacent location from each flash location
                 *  3. check all locations whose energy level was recently incremented if it will flash this step
                 *  4. if flash, increment its adjacent locations energy levels by 1
                 *  5. if not, do nothing
                 *  6. repeat until no more octopuses have an energy level greater than 9 and have already flashed
                 */

                while (pendingLocations.Count > 0)
                {
                    int[] location = pendingLocations.Dequeue();
                    completedLocations.Add(location);

                    List<int[]> newLocations = IncrementAdjacentEnergyLevels(inputs, location);
                    List<int[]> newPendingLocations = CheckOctopusesEnergyLevels(inputs, newLocations);

                    flashCount += newPendingLocations.Count;
                    
                    foreach(var newPendingLocation in newPendingLocations)
                    {
                        pendingLocations.Enqueue(newPendingLocation);
                    }
                }

                ResetEnergyLevels(inputs, completedLocations);
            }

            return flashCount;
        }

        // returns the first step encountered when all octopuses flash
        public static int GetStepNumberWhenAllOctopusesFlash(List<List<Octopus>> inputs)
        {
            int flashCount = 0;
            // assuming that the nuumber of columns per row are equal
            int synchronizedFlashCount = inputs.Count * inputs[0].Count;

            int stepNumber = 0;

            while (flashCount != synchronizedFlashCount)
            {
                // reset flash count;
                flashCount = 0;

                IncrementEnergyLevels(inputs);
                List<int[]> locations = CheckOctopusesEnergyLevels(inputs);
                // Note: this gives us the number of octopuses that will flash this step
                flashCount += locations.Count;

                Queue<int[]> pendingLocations = new Queue<int[]>();
                List<int[]> completedLocations = new List<int[]>();

                foreach (var location in locations)
                {
                    pendingLocations.Enqueue(location);
                }

                /*
                 *  1. for each flash location, increment every adjacent location's energy level by 1
                 *  2. collect every adjacent location from each flash location
                 *  3. check all locations whose energy level was recently incremented if it will flash this step
                 *  4. if flash, increment its adjacent locations energy levels by 1
                 *  5. if not, do nothing
                 *  6. repeat until no more octopuses have an energy level greater than 9 and have already flashed
                 */

                while (pendingLocations.Count > 0)
                {
                    int[] location = pendingLocations.Dequeue();
                    completedLocations.Add(location);

                    List<int[]> newLocations = IncrementAdjacentEnergyLevels(inputs, location);
                    List<int[]> newPendingLocations = CheckOctopusesEnergyLevels(inputs, newLocations);

                    flashCount += newPendingLocations.Count;

                    foreach (var newPendingLocation in newPendingLocations)
                    {
                        pendingLocations.Enqueue(newPendingLocation);
                    }
                }

                ResetEnergyLevels(inputs, completedLocations);
                ++stepNumber;

            }

            return stepNumber;
        }

        private static void IncrementEnergyLevels(List<List<Octopus>> inputs)
        {
            for (int i = 0;i < inputs.Count; ++i)
            {
                for (int j = 0;j < inputs[i].Count; ++j)
                {
                    inputs[i][j].IncrementEnergyLevel();
                }
            }
        }

        // returns the number of octopuses that will flash this step and their respective locations
        private static List<int[]> CheckOctopusesEnergyLevels(List<List<Octopus>> inputs)
        {
            var locations = new List<int[]>();

            for (int i = 0; i < inputs.Count; ++i)
            {
                for (int j = 0; j < inputs[i].Count; ++j)
                {
                    if (!inputs[i][j].hasFlashed && inputs[i][j].EnergyLevel > 9)
                    {
                        inputs[i][j].hasFlashed = true;
                        locations.Add(new int[] { i, j });
                    }
                }
            }

            return locations;
        }

        private static List<int[]> CheckOctopusesEnergyLevels(List<List<Octopus>> inputs, List<int[]> locations)
        {
            var newLocations = new List<int[]>();

            foreach(var location in locations)
            {
                int row = location[0];
                int col = location[1];
                if (!inputs[row][col].hasFlashed && inputs[row][col].EnergyLevel > 9)
                {
                    inputs[row][col].hasFlashed = true;
                    newLocations.Add(new int[] { row, col });
                }
            }


            return newLocations;
        }

        private static void ResetEnergyLevels(List<List<Octopus>> inputs, List<int[]> locations)
        {
            foreach (int[] location in locations)
            {
                int row = location[0];
                int col = location[1];
                inputs[row][col].Reset();
            }
        }

        private static List<int[]> IncrementAdjacentEnergyLevels(List<List<Octopus>> octopuses, int[] location)
        {
            int[] left = new int[] { location[0], location[1] - 1 };
            int[] right = new int[] { location[0], location[1] + 1 };
            int[] up = new int[] { location[0] - 1, location[1] };
            int[] down = new int[] { location[0] + 1, location[1] };

            int[] upLeft = new int[] { location[0] - 1, location[1] - 1 };
            int[] upRight = new int[] { location[0] - 1, location[1] + 1 };
            int[] downLeft = new int[] { location[0] + 1, location[1] - 1 };
            int[] downRight = new int[] { location[0] + 1, location[1] + 1 };

            var adjacentLocations = new List<int[]>();
            adjacentLocations.Add(left);
            adjacentLocations.Add(right);
            adjacentLocations.Add(up);
            adjacentLocations.Add(down);
            adjacentLocations.Add(upLeft);
            adjacentLocations.Add(upRight);
            adjacentLocations.Add(downLeft);
            adjacentLocations.Add(downRight);

            int maxRow = octopuses.Count;
            // assumption: all columns will have equal length in the grid
            int maxCol = octopuses[0].Count;

            List<int[]> validLocations = new List<int[]>();

            foreach (var adjacentLocation in adjacentLocations)
            {
                if (IsInBounds(adjacentLocation, maxRow, maxCol))
                {
                    int row = adjacentLocation[0];
                    int col = adjacentLocation[1];
                    // do we want to increment octopus energy level if it already flashed this step?
                    octopuses[row][col].IncrementEnergyLevel();
                    validLocations.Add(adjacentLocation);
                }
            }

            return validLocations;
        }

        private static bool IsInBounds(int[] location, int maxRow, int maxCol)
        {
            int row = location[0];
            int col = location[1];
            return row < maxRow && row >= 0 && col < maxCol && col >= 0;
        }
    }
}

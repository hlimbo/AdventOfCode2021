using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day7
{

    // 1 horizontal move unit costs 1 fuel per crab submarine
    // Q1: can the crab submarine move left (negative unit) or right (positive unit)?

    // Problem: Determine the cheapest possible fuel intake for all crab submarines to align with each other
    // Here... alignment means all the crabs have equal horizontal positions where horizontal position is in 1D
    
    public class TreacheryOfWhales
    {
        public static int[] ReadInputs(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);

            List<int> crabSubmarinePositions = new List<int>();

            try
            {
                string rawInput = reader.ReadLine();
                crabSubmarinePositions = rawInput.Split(',').Select(num => int.Parse(num)).ToList();
            }
            catch
            {
                Console.WriteLine("Error reading path at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }

            return crabSubmarinePositions.ToArray();
        }
    
        /*
         * Possibilities:
         * 1. sum up all crab horizontal positions and find average. Use average as alignment value
         * 2. Pick the smallest horizontal position as alignment value
         * 3. Treat it like a DP problem and use a brute force solution to find minimum fuel cost to align crabs on 1 horizontal position
         * Q2: do the crabs submarines need to align on a pre-existing position from one of the crab submarines?
         * A2: No
         * 
         * Need to find a horizontal unit between min horizontal unit and max horizontal unit that will cost crab sub fuel the least
         * ... Try out all possibilities from min to max inclusive
         * O(N * M) solution where N is the range between min horizontal and max horizontal positions
         * and M is the length of crab submarine horizontal positions
         * Can we do better?
         *
         */

        // Assume 1 unit movement costs 1 fuel for a crab submarine
        public static int GetMinimumFuelCost(int[] crabSubPositions)
        {
            // find min && max positions
            int minPosition = crabSubPositions[0];
            int maxPosition = crabSubPositions[0];
            for(int i = 1;i < crabSubPositions.Length; ++i)
            {
                minPosition = Math.Min(minPosition, crabSubPositions[i]);
                maxPosition = Math.Max(maxPosition, crabSubPositions[i]);
            }

            // bottom up approach
            /* index is fuel cost where cost rate is constant | value is the fuel cost where the cost rate increases linearly
             * a[0] = 0
             * a[1] = a[0] + 1 = 1
             * a[2] = a[1] + 2 = 3
             * a[3] = a[2] + 3 = 6
             * a[4] = a[3] + 4 = 10
             */
            // Memo table
            int[] fuelCosts = new int[maxPosition + 1];

            if (fuelCosts.Length > 1)
            {
                for (int i = 1; i < fuelCosts.Length; ++i)
                {
                    fuelCosts[i] = fuelCosts[i - 1] + i;
                }
            }

            // Try out all possible crab positions to find the minimum fuel cost to align crabs at a given position
            int minFuelCost = int.MaxValue;
            for(int alignedPosition = minPosition; alignedPosition <= maxPosition; ++alignedPosition)
            {
                int fuelCost = 0;
                // calculate fuel cost for assumed aligned position
                foreach (var position in crabSubPositions)
                {
                    // Part 1
                    // fuelCost += GetConstantFuelCost(alignedPosition, position);
                    // Part 2
                    // fuelCost += GetLinearFuelCost(alignedPosition, position, fuelCosts);
                    // Part 2 improved
                    int constantFuelCost = GetFuelCostConstantRate(alignedPosition, position);
                    fuelCost += fuelCosts[constantFuelCost];
                }

                minFuelCost = Math.Min(minFuelCost, fuelCost);
            }

            // Console.WriteLine(string.Join(',', fuelCosts));

            return minFuelCost;
        }

        // 1 unit of movement costs 1 fuel
        private static int GetFuelCostConstantRate(int alignedPosition, int crabSubPosition)
        {
            return Math.Abs(alignedPosition - crabSubPosition);
        }

        private static int GetFuelCostLinearRate(int alignedPosition, int crabSubPosition, int[] fuelCosts)
        {
            int constantFuelCost = GetFuelCostConstantRate(alignedPosition, crabSubPosition);
            if (fuelCosts[constantFuelCost] != -1)
            {
                return fuelCosts[constantFuelCost];
            }

            int fuelCost = constantFuelCost;
            for (int i = constantFuelCost - 1;i > 0; --i)
            {
                fuelCost += i;
            }

            fuelCosts[constantFuelCost] = fuelCost;

            return fuelCost;
        }
    }
}

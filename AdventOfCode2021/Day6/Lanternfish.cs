using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day6
{
    /*
     * Initial Laternfish timer = 3
     * 1 day passes = 2
     * 2 days passes = 1
     * 3 days passes = 0
     * 4 days passes = timer reset 6 and create a new lanternfish with a timer of 8
     * --> what is the lanternfish's timer here? -1?
     * 
     * Lanternfish that help spawn a new lantern fish's timer is now 6 starting from day 0
     * day 0 counts as 1 day
     * 
     * After 1 day passes
     * 1st lanternfish has timer of 5
     * 2nd lanternfish has timer of 7
     * 
     * Input:
     * X days to simulate
     * age of several lanternfish comma separated integer values
     * 
     * Output:
     * Calculate number of fish after X days
     * 
     * Once a fish's timer resets to 6, add a new fish to the end of the list
     * if more than 1 fish's timer resets to 6, add that amount of new fish to the end of the list with timer set to 8
     * 
     */ 

    public class Lanternfish
    {
        public static int[] ReadInputs(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);

            List<int> fishTimers = new List<int>();

            try
            {
                string rawInput = reader.ReadLine(); 
                fishTimers = rawInput.Split(',').Select(num => int.Parse(num)).ToList();
            }
            catch
            {
                Console.WriteLine("Error reading path at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }

            return fishTimers.ToArray();
        }

        // Too slow.. to answer part 2.. if starting fish population is either large or if total number of days is large or both
        // Issue 1: number of items in a list is bound to the max size of int limits (solution: use long)
        // Issue 2: as number of lantern fish multiply exponentially, the problem time complexity is O(N * some exponential function)
        // where N represents the number of total days to simulate fish population growth
        public static int GetSimulatedPopulationGrowth(int[] initialFishes, int totalDays)
        {
            var fishes = new List<int>(initialFishes);
            int newFishCount = 0;

            for (int d = 0;d < totalDays; ++d)
            {

                // Spawn new fishes for every fish whose day reset back to 6 from 0

                // count the number of fishes whose timer reaches 0
                newFishCount = fishes.Where(fishTimer => fishTimer == 0).Count();
                for (int i = 0;i < newFishCount; ++i)
                {
                    fishes.Add(9);
                }

                // skip newly added fishes at the end of the list (if any were added)
                for (int i = 0;i < fishes.Count - newFishCount; ++i)
                {
                    // set fish timer to 7 since their life count will go down to 6
                    if (fishes[i] == 0)
                    {
                        fishes[i] = 7;
                    }
                }

                // decrement each fish timer by 1 where fishTimer[i] > 0
                for (int i = 0;i < fishes.Count; ++i)
                {
                    if (fishes[i] > 0)
                    {
                        --fishes[i];
                    }
                }
            }

            return fishes.Count;
        }

        // O(N + M) time complexity where N represents the number of days and M represents the number of fish we place into the fish buckets (e.g. dictionary)
        // Iterating through the fish buckets can be argued to be O(1) since in the scope of this problem the number of buckets don't change
        public static long GetSimulatedPopulationGrowthLarge(int[] initialFishes, int totalDays)
        {
            // store each fish in a bucket where the key represents the timer a fish has and value represents number of fishes that share that same time
            int bucketLength = 10;
            long[] fishBucket = new long[bucketLength];

            // count number of fishes that share the same fishTimer
            foreach(var fishTimer in initialFishes)
            {
                fishBucket[fishTimer] += 1;
            }

            for (int d = 0;d < totalDays; ++d)
            {
                // Add X new fishes where X represents the number of original fishes whose timer reached 0
                long newFishCount = fishBucket[0];
                // Note: set new fish life to 9 here because in the loop below we are moving them to day 8
                fishBucket[bucketLength - 1] += newFishCount;

                long fishCountAtTimer0 = fishBucket[0];

                for (int i = 1;i < bucketLength; ++i)
                {
                    // Add fish's whose timer at the beginning of this day to timer 6 bucket if i - 1 == 6
                    // otherwise move fishes from day i to day i - 1
                    fishBucket[i - 1] = i - 1 == 6 ? fishBucket[i] + fishCountAtTimer0 : fishBucket[i];

                    // edge case: fish bucket 9 should be emptied once it is all transfered to fish bucket 8
                    if (i == 9)
                    {
                        fishBucket[i] = 0;
                    }
                }
            }

            // return number of fish that is in the population now
            return fishBucket.Sum();
        }
    }
}

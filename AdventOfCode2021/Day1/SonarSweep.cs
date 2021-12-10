using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2021.Day1
{
    public class SonarSweep
    {
        public static int GetDepthMeasurementIncreaseCount(int[] depthMeasurements)
        {
            int increaseCount = 0;

            for (int i = 1; i < depthMeasurements.Length; ++i)
            {
                if (depthMeasurements[i] > depthMeasurements[i - 1])
                {
                    ++increaseCount;
                }
            }
            return increaseCount;
        }

        // Count Number of times the sum of measurements in a 3 number sliding window increases
        public static int GetSlidingDepthWindowIncreaseCount(int[] depthMeasurements, int windowLength)
        {
            int increaseCount = 0;

            for (int i = 1; i < depthMeasurements.Length; ++i)
            {
                int prevWindowSum = GetSlidingWindowSum(depthMeasurements, i - 1, windowLength);
                int currentWindowSum = GetSlidingWindowSum(depthMeasurements, i, windowLength);

                if (prevWindowSum < currentWindowSum)
                {
                    ++increaseCount;
                }
            }

            return increaseCount;
        }

        private static int GetSlidingWindowSum(int[] depthMeasurements, int startOffset, int windowLength)
        {
            int sum = 0;
            for (int i = startOffset;i < startOffset + windowLength && i < depthMeasurements.Length; ++i)
            {
                sum += depthMeasurements[i];
            }

            return sum;
        }
    }
}

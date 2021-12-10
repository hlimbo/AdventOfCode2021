using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AdventOfCode2021.Day6;

namespace AdventOfCode2021Tests.Day6
{
    public class LanternFishTests
    {
        private static string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day6\\Inputs";
        private static string smallInputPath = fullPath + "\\small_input.txt";
        private static string bigInputPath = fullPath + "\\big_input.txt";

        [Test]
        public void TestSimulatedPopGrowthSmallInputPt1()
        {
            int[] fishTimers = Lanternfish.ReadInputs(smallInputPath);
            int finalFishCount = Lanternfish.GetSimulatedPopulationGrowth(fishTimers, 80);
            Assert.AreEqual(5934, finalFishCount);
        }

        [Test]
        public void TestSimulatedPopGrowthBigInputPt1()
        {
            int[] fishTimers = Lanternfish.ReadInputs(bigInputPath);
            int finalFishCount = Lanternfish.GetSimulatedPopulationGrowth(fishTimers, 80);
            Assert.AreEqual(361169, finalFishCount);
        }

        [Test]
        public void TestSimulatedPopGrowthSmallInputPt2()
        {
            int[] fishTimers = Lanternfish.ReadInputs(smallInputPath);
            long finalFishCount = Lanternfish.GetSimulatedPopulationGrowthLarge(fishTimers, 256);
            Assert.AreEqual(26984457539, finalFishCount);
        }

        [Test]
        public void TestSimulatedPopGrowthBigInputPt2()
        {
            int[] fishTimers = Lanternfish.ReadInputs(bigInputPath);
            long finalFishCount = Lanternfish.GetSimulatedPopulationGrowthLarge(fishTimers, 256);
            Assert.AreEqual(1634946868992, finalFishCount);
        }
    }
}

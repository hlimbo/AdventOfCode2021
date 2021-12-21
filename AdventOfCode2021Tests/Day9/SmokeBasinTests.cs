using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using AdventOfCode2021.Day9;

namespace AdventOfCode2021Tests.Day9
{
    public class SmokeBasinTests
    {
        private static string path = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day9\\Inputs\\";
        private string smallInput = path + "small_input.txt";
        private string bigInput = path + "big_input.txt";

        [Test]
        public void SmallInputTestPart1()
        {
            var heightMap = SmokeBasin.ReadInputs(smallInput);
            int sum = SmokeBasin.CalculateRiskValueFromLowPoints(heightMap);
            Assert.AreEqual(15, sum);
        }

        [Test]
        public void BigInputTestPart1()
        {
            var heightMap = SmokeBasin.ReadInputs(bigInput);
            int sum = SmokeBasin.CalculateRiskValueFromLowPoints(heightMap);
            Assert.AreEqual(524, sum);
        }

        [Test]
        public void SmallInputTestPart2()
        {
            var heightMap = SmokeBasin.ReadInputs(smallInput);
            int product = SmokeBasin.GetLargestBasinsProduct(heightMap);
            Assert.AreEqual(1134, product);
        }

        [Test]
        public void BigInputTestPart2()
        {
            var heightMap = SmokeBasin.ReadInputs(bigInput);
            int product = SmokeBasin.GetLargestBasinsProduct(heightMap);
            Assert.AreEqual(1235430, product);
        }

    }
}

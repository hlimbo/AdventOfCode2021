using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using AdventOfCode2021.Day11;

namespace AdventOfCode2021Tests.Day11
{
    public class DumboOctopusTests
    {
        private static string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day11\\Inputs\\";
        private string smallInput = fullPath + "small_input.txt";
        private string bigInput = fullPath + "big_input.txt";

        [Test]
        public void SmallInputTestPart1()
        {
            var octopuses = DumboOctopus.ReadInputs(smallInput);
            int flashCount = DumboOctopus.CalculateFlashCount(100, octopuses);
            Assert.AreEqual(1656, flashCount);
        }

        [Test]
        public void BigInputTestPart1()
        {
            var octopuses = DumboOctopus.ReadInputs(bigInput);
            int flashCount = DumboOctopus.CalculateFlashCount(100, octopuses);
            Assert.AreEqual(1634, flashCount);
        }

        [Test]
        public void BigInputTestPart2()
        {
            var octopuses = DumboOctopus.ReadInputs(bigInput);
            int stepNumber = DumboOctopus.GetStepNumberWhenAllOctopusesFlash(octopuses);
            Assert.AreEqual(210, stepNumber);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using AdventOfCode2021.Day8;

namespace AdventOfCode2021Tests.Day8
{
    public class SevenSegmentSearchTests
    {
        private static string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day8\\Inputs\\";
        private string smallInput2Path = fullPath + "small_input_2.txt";
        private string bigInputPath = fullPath + "big_input.txt";

        [Test]
        public void TestPart1()
        {
            List<SevenSegmentSearch.BrokenDisplay> displays = SevenSegmentSearch.ReadInputs(smallInput2Path);
            int answer = SevenSegmentSearch.GetOneFourSevenOrEightDigitCount(displays);
            Assert.AreEqual(26, answer);

            displays = SevenSegmentSearch.ReadInputs(bigInputPath);
            answer = SevenSegmentSearch.GetOneFourSevenOrEightDigitCount(displays);
            Assert.AreEqual(261, answer);
        }

        [Test]
        public void TestPart2()
        {
            List<SevenSegmentSearch.BrokenDisplay> displays = SevenSegmentSearch.ReadInputs(bigInputPath);
            int answer = SevenSegmentSearch.DecodeDisplayOutputs(displays);
            Assert.AreEqual(987553, answer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AdventOfCode2021.Day10;

namespace AdventOfCode2021Tests.Day10
{
    public class SyntaxScoringTests
    {
        private static string path = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day10\\Inputs\\";
        private string bigInput = path + "big_input.txt";

        private List<string> lines;

        [SetUp]
        public void Setup()
        {
            lines = SyntaxScoring.ReadInputs(bigInput);
        }

        [Test]
        public void TestPart1()
        {
            int totalScore = SyntaxScoring.CalculateTotalSyntaxErrorScore(lines);
            Assert.AreEqual(413733, totalScore);
        }

        [Test]
        public void TestPart2()
        {
            long middleScore = SyntaxScoring.FindMiddleAutoCompleteScore(lines);
            Assert.AreEqual(3354640192, middleScore);
        }
    }
}

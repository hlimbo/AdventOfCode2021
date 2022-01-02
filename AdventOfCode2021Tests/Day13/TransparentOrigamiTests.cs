using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using AdventOfCode2021.Day13;

namespace AdventOfCode2021Tests.Day13
{
    public class TransparentOrigamiTests
    {
        private string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day13\\Inputs\\big_input.txt";
        private TransparentOrigami.TransparentPaper paper;

        [SetUp]
        public void Setup()
        {
            var manual = TransparentOrigami.ReadInputs(fullPath);
            int[] maxXYUnits = TransparentOrigami.GetMaxXYUnits(manual.coordinates);
            paper = TransparentOrigami.DrawTransparentPaper(manual.coordinates, maxXYUnits);
        }

        [Test]
        public void TestPart1()
        {
            paper.FoldHorizontally(655);
            Assert.AreEqual(638, paper.CountHashes());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AdventOfCode2021.Day2;

namespace AdventOfCode2021Tests.Day2
{
    public class DiveTests
    {
        private string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day2\\inputs";

        [Test]
        public void TestDiveSmallInputPt1()
        {

            string path = fullPath + "\\dive_small_input.txt";
            Dive.Command[] commands = Dive.ReadInputs(path);
            int answer = Dive.FindSubmarineProductLocation(commands);
            Assert.AreEqual(150, answer);
        }

        [Test]
        public void TestDiveBigInputPt1()
        {
            string path = fullPath + "\\dive_big_input.txt";
            Dive.Command[] commands = Dive.ReadInputs(path);
            int answer = Dive.FindSubmarineProductLocation(commands);
            Assert.AreEqual(2187380, answer);
        }

        [Test]
        public void TestDiveSmallInputPt2()
        {
            string path = fullPath + "\\dive_small_input.txt";
            Dive.Command[] commands = Dive.ReadInputs(path);
            int answer = Dive.FindSubmarineProductLocationPt2(commands);
            Assert.AreEqual(900, answer);
        }

        [Test]
        public void TestDiveBigInputPt2()
        {
            string path = fullPath + "\\dive_big_input.txt";
            Dive.Command[] commands = Dive.ReadInputs(path);
            int answer = Dive.FindSubmarineProductLocationPt2(commands);
            Assert.AreEqual(2086357770, answer);
        }
    }
}

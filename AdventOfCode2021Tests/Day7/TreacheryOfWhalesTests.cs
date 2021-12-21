using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using AdventOfCode2021.Day7;

namespace AdventOfCode2021Tests.Day7
{
    public class TreacheryOfWhalesTests
    {
        private string path = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day7\\Inputs\\big_input.txt";

        [Test]
        public void BigInputTestPart2()
        {
            int[] horizontalInputs = TreacheryOfWhales.ReadInputs(path);
            int minFuelCost = TreacheryOfWhales.GetMinimumFuelCost(horizontalInputs);
            Assert.AreEqual(98231647, minFuelCost);
        }
    }
}

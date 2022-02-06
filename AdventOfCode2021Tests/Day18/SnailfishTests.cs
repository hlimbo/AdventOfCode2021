using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AdventOfCode2021.Day18;

namespace AdventOfCode2021Tests.Day18
{
    internal class SnailfishTests
    {
        [Test]
        public void MagnitudeTests()
        {
            string line = "[[1,2],[[3,4],5]]";
            Snailfish.SnailfishNumber num = Snailfish.ConstructNumber(line);
            int actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(143, actual);

            line = "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]";
            num = Snailfish.ConstructNumber(line);
            actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(1384, actual);

            line = "[[[[1,1],[2,2]],[3,3]],[4,4]]";
            num = Snailfish.ConstructNumber(line);
            num = Snailfish.ConstructNumber(line);
            actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(445, actual);

            line = "[[[[3,0],[5,3]],[4,4]],[5,5]]";
            num = Snailfish.ConstructNumber(line);
            actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(791, actual);

            line = "[[[[5,0],[7,4]],[5,5]],[6,6]]";
            num = Snailfish.ConstructNumber(line);
            actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(1137, actual);

            line = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
            num = Snailfish.ConstructNumber(line);
            actual = Snailfish.GetMagnitude(num);
            Assert.AreEqual(3488, actual);
        }

        [Test]
        public void FindNumberToExplodeTests()
        {
            string line = "[[[[[9,8],1],2],3],4]";
            var snailfishNum = Snailfish.ConstructNumber(line);
            var numToExplode = Snailfish.FindNumberToExplode(snailfishNum, 0);

            Assert.AreEqual(9, numToExplode.leftRegNumber);
            Assert.AreEqual(8, numToExplode.rightRegNumber);

            line = "[7,[6,[5,[4,[3,2]]]]]";
            snailfishNum = Snailfish.ConstructNumber(line);
            numToExplode = Snailfish.FindNumberToExplode(snailfishNum, 0);

            Assert.AreEqual(3, numToExplode.leftRegNumber);
            Assert.AreEqual(2, numToExplode.rightRegNumber);

            line = "[[6,[5,[4,[3,2]]]],1]";
            snailfishNum = Snailfish.ConstructNumber(line);
            numToExplode = Snailfish.FindNumberToExplode(snailfishNum, 0);

            Assert.AreEqual(3, numToExplode.leftRegNumber);
            Assert.AreEqual(2, numToExplode.rightRegNumber);

            line = "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
            snailfishNum = Snailfish.ConstructNumber(line);
            numToExplode = Snailfish.FindNumberToExplode(snailfishNum, 0);

            Assert.AreEqual(7, numToExplode.leftRegNumber);
            Assert.AreEqual(3, numToExplode.rightRegNumber);

            line = "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
            snailfishNum = Snailfish.ConstructNumber(line);
            numToExplode = Snailfish.FindNumberToExplode(snailfishNum, 0);

            Assert.AreEqual(3, numToExplode.leftRegNumber);
            Assert.AreEqual(2, numToExplode.rightRegNumber);
        }


        [Test]
        public void SimpleReduceTest()
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day18\\Inputs\\simple_input.txt";
            var lines = Snailfish.ReadFile(fullPath);
            List<Snailfish.SnailfishNumber> snailfishNums = Snailfish.ParseInputs(lines);
            Snailfish.SnailfishNumber snailfishNum = Snailfish.Calculate(snailfishNums);
            int num = Snailfish.GetMagnitude(snailfishNum);

            Assert.AreEqual(791, num);
        }

        [Test]
        public void SimpleReduceTest2()
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day18\\Inputs\\simple_input2.txt";
            var lines = Snailfish.ReadFile(fullPath);
            List<Snailfish.SnailfishNumber> snailfishNums = Snailfish.ParseInputs(lines);
            Snailfish.SnailfishNumber snailfishNum = Snailfish.Calculate(snailfishNums);
            int num = Snailfish.GetMagnitude(snailfishNum);

            Assert.AreEqual(1137, num);
        }

        [Test]
        public void BigReduceTest()
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day18\\Inputs\\big_input.txt";
            var lines = Snailfish.ReadFile(fullPath);
            List<Snailfish.SnailfishNumber> snailfishNums = Snailfish.ParseInputs(lines);
            Snailfish.SnailfishNumber snailfishNum = Snailfish.Calculate(snailfishNums);
            int num = Snailfish.GetMagnitude(snailfishNum);

            Assert.AreEqual(3486, num);
        }
    }
}

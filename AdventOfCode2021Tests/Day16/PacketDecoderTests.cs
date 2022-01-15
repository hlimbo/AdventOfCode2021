using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using AdventOfCode2021.Day16;

namespace AdventOfCode2021Tests.Day16
{
    internal class PacketDecoderTests
    {
        private const string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day16\\Inputs\\";

        private long GetAnswer(string file)
        {
            string path = fullPath + file;
            string hexValues = PacketDecoder.ReadInputs(path);
            string binaryValues = PacketDecoder.ConvertHexToBinary(hexValues);

            int bitIndex = 0;
            var packets = PacketDecoder.ParseBinary(binaryValues, ref bitIndex);
            return PacketDecoder.CalculatePacketAnswer(packets, -1);
        }

        [Test]
        public void LiteralOnlyTest()
        {
            long ans = GetAnswer("small_input.txt");
            Assert.AreEqual(2021, ans);
        }

        [Test]
        public void SumTest()
        {
            long ans = GetAnswer("sum_input.txt");
            Assert.AreEqual(3, ans);
        }

        [Test]
        public void ProductTest()
        {
            long ans = GetAnswer("product_input.txt");
            Assert.AreEqual(54, ans);
        }
        
        [Test]
        public void MinTest()
        {
            long ans = GetAnswer("min_input.txt");
            Assert.AreEqual(7, ans);
        }

        [Test]
        public void MaxTest()
        {
            long ans = GetAnswer("max_input.txt");
            Assert.AreEqual(9, ans);
        }

        [Test]
        public void LessThanTest()
        {
            long ans = GetAnswer("less_than_input.txt");
            Assert.AreEqual(1, ans);
        }

        [Test]
        public void GreaterThanTest()
        {
            long ans = GetAnswer("greater_than_input.txt");
            Assert.AreEqual(0, ans);
        }

        [Test]
        public void EqualToTest()
        {
            long ans = GetAnswer("equal_to_input.txt");
            Assert.AreEqual(1, ans);
        }

        [Test]
        public void IntegrationTest()
        {
            long ans = GetAnswer("big_input.txt");
            Assert.AreEqual(660797830937, ans);
        }

    }
}

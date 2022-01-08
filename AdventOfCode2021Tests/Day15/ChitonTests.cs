using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AdventOfCode2021.Day15;

namespace AdventOfCode2021Tests.Day15
{
    internal class ChitonTests
    {
        private string path = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day15\\Inputs\\big_input.txt";
        private List<List<int>> riskTile;

        [SetUp]
        public void Setup()
        {
            riskTile = Chiton.ReadInputs(path);
        }

        [Test]
        public void TestPart1()
        {
            Chiton.RiskPath riskPath = Chiton.Dijkstra(riskTile);
            Assert.AreEqual(388, riskPath.minRiskCost);
        }

        [Test]
        public void TestPart2()
        {
            int[,] caveSystem = Chiton.ExtendRiskTilesToFormCaveMap(riskTile, 5);
            List<List<int>> riskMap = Chiton.ConvertJaggedArrayToListOfLists(caveSystem);
            Chiton.RiskPath riskPath = Chiton.Dijkstra(riskMap);

            Assert.AreEqual(2819, riskPath.minRiskCost);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AdventOfCode2021.Day5;

namespace AdventOfCode2021Tests.Day5
{
    public class HydrothermalVentureTests
    {
        private string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day5\\inputs";

        [Test]
        public void TestOverlappingPointsSmallInputPt1()
        {
            string file = "\\small_input.txt";
            string path = fullPath + file;

            var lineSegments = HydrothermalVenture.ReadInputs(path);
            int[,] diagram = HydrothermalVenture.DrawDiagram(lineSegments);
            Assert.AreEqual(5, HydrothermalVenture.GetOverlappingPointsCount(diagram));
        }

        [Test]
        public void TestOverlappingPointsBigInputPt1()
        {
            string file = "\\big_input.txt";
            string path = fullPath + file;

            var lineSegments = HydrothermalVenture.ReadInputs(path);
            int[,] diagram = HydrothermalVenture.DrawDiagram(lineSegments);
            Assert.AreEqual(6841, HydrothermalVenture.GetOverlappingPointsCount(diagram));
        }

        [Test]
        public void TestOverlappingPointsSmallInputPt2()
        {
            string file = "\\small_input.txt";
            string path = fullPath + file;
            
            var lineSegments = HydrothermalVenture.ReadInputs(path);
            int[,] diagram = HydrothermalVenture.DrawDiagramWithDiagonals(lineSegments);
            Assert.AreEqual(12, HydrothermalVenture.GetOverlappingPointsCount(diagram));
        }

        [Test]
        public void TestOverlappingPointsBigInputPt2()
        {
            string file = "\\big_input.txt";
            string path = fullPath + file;

            var lineSegments = HydrothermalVenture.ReadInputs(path);
            int[,] diagram = HydrothermalVenture.DrawDiagramWithDiagonals(lineSegments);
            Assert.AreEqual(19258, HydrothermalVenture.GetOverlappingPointsCount(diagram));
        }
    }
}

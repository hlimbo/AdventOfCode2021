using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using AdventOfCode2021.Day3;

namespace AdventOfCode2021Tests.Day3
{
    public class BinaryDiagnosticTests
    {
        private string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day3\\inputs";

        [Test]
        public void TestBinaryDiagnosticSmallInputPt1()
        {
            string path = fullPath + "\\small_input.txt";
            int[] reportValues = BinaryDiagnostic.ReadReportValues(path);
            int bitLength = reportValues[0];
            reportValues = reportValues.Skip(1).ToArray();
            int powerConsumption = BinaryDiagnostic.GetSubmarinePowerConsumption(reportValues, bitLength);
            Assert.AreEqual(198, powerConsumption);
        }

        [Test]
        public void TestBinaryDiagnosticBigInputPt1()
        {
            string path = fullPath + "\\big_input.txt";
            int[] reportValues = BinaryDiagnostic.ReadReportValues(path);
            int bitLength = reportValues[0];
            reportValues = reportValues.Skip(1).ToArray();
            int powerConsumption = BinaryDiagnostic.GetSubmarinePowerConsumption(reportValues, bitLength);
            Assert.AreEqual(852500, powerConsumption);
        }

        [Test]
        public void TestBinaryDiagnosticSmallInputPt2()
        {
            string path = fullPath + "\\small_input.txt";
            string[] reportValues = BinaryDiagnostic.ReadReportValuesString(path);
            int bitLength = reportValues[0].Length;
            int lifeSupportRating = BinaryDiagnostic.GetSubmarineLifeSupportRating(reportValues, bitLength);
            Assert.AreEqual(230, lifeSupportRating);
        }

        [Test]
        public void TestBinaryDiagnosticBigInputPt2()
        {
            string path = fullPath + "\\big_input.txt";
            string[] reportValues = BinaryDiagnostic.ReadReportValuesString(path);
            int bitLength = reportValues[0].Length;
            int lifeSupportRating = BinaryDiagnostic.GetSubmarineLifeSupportRating(reportValues, bitLength);
            Assert.AreEqual(1007985, lifeSupportRating);
        }
    }
}


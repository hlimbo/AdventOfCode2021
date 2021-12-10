using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using AdventOfCode2021.Day4;

namespace AdventOfCode2021Tests.Day4
{
    public class GiantSquidTests
    {
        private string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day4\\inputs";

        [Test]
        public void TestBingoGameSmallInputPt1()
        {
            string path = fullPath + "\\small_input.txt";
            var bingoGame = GiantSquid.ReadInputs(path);
            var winningBoard = bingoGame.Play();
            Assert.IsNotNull(winningBoard);
            Assert.AreEqual(4512, bingoGame.CalculateFinalScore(winningBoard));
        }

        [Test]
        public void TestBingoGameBigInputPt1()
        {
            string path = fullPath + "\\big_input.txt";
            var bingoGame = GiantSquid.ReadInputs(path);
            var winningBoard = bingoGame.Play();
            Assert.IsNotNull(winningBoard);
            Assert.AreEqual(12796, bingoGame.CalculateFinalScore(winningBoard));
        }

        [Test]
        public void TestBingoGameSmallInputPt2()
        {
            string path = fullPath + "\\small_input.txt";
            var bingoGame = GiantSquid.ReadInputs(path);
            var lastWinningBoard = bingoGame.PlayUntilAllBoardsWin();
            Assert.IsNotNull(lastWinningBoard);
            Assert.AreEqual(1924, bingoGame.CalculateFinalScore(lastWinningBoard));
        }

        [Test]
        public void TestBingoGameBigInputPt2()
        {
            string path = fullPath + "\\big_input.txt";
            var bingoGame = GiantSquid.ReadInputs(path);
            var lastWinningBoard = bingoGame.PlayUntilAllBoardsWin();
            Assert.IsNotNull(lastWinningBoard);
            Assert.AreEqual(18063, bingoGame.CalculateFinalScore(lastWinningBoard));
        }
    }
}

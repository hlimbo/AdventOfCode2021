using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day4
{
    /*
     * 
     * Background: Play Bingo with a Squid attached to my submarine
     * 
     * Bingo Rules
     * 5x5 grid of numbers
     * Numbers on grid are picked at random
     * Numbers picked to mark on board if available are picked at random
     * A winner is found when all numbers in any row or any column of a board are marked
     * Diagonals do not count
     * 
     * 
     * Part b:
     * From the winning board, get all unmarked numbers and return is sum
     * multiply the sum by the last number called to get final score
     * 
     * To beat the gian squid, I must figure out which board will win first
     * 
     * Input format: 
     * First Line contains list of all numbers that will be picked in order
     * After second line will be all the boards separated by newlines
     */

    public class GiantSquid
    {
        public class Slot
        {
            private int value;
            public int Value => value;

            private bool isMarked;
            public bool IsMarked
            {
                get { return isMarked; }
                set { isMarked = value; }
            }
            

            public Slot(int value, bool isMarked = false)
            {
                this.value = value;
                this.isMarked = isMarked;
            }

            public override string ToString()
            {
                return value + "|" + isMarked;
            }
        }

        public class Board
        {
            public static int N = 5;

            private Slot[,] slots;

            public Slot[] UnmarkedSlots
            {
                get
                {
                    var unmarkedSlots = new List<Slot>();
                    for(int r = 0;r < N; ++r)
                    {
                        for (int c = 0;c < N; ++c)
                        {
                            if (!slots[r,c].IsMarked)
                            {
                                unmarkedSlots.Add(slots[r, c]);
                            }
                        }
                    }

                    return unmarkedSlots.ToArray();
                }
            }

            public static Board Build(int[][] numbers)
            {
                var board = new Board();
                for (int r = 0; r < N; ++r)
                {
                    for (int c = 0; c < N; ++c)
                    {
                        board.slots[r, c] = new Slot(numbers[r][c]);
                    }
                }

                return board;
            }

            public Board()
            {
                slots = new Slot[N, N];
            }

            public Slot GetSlot(int row, int col) 
            {
                if (row < 0 || row >= N || col < 0 || col >= N)
                {
                    Console.WriteLine("Row and/or column value out of bounds: " + row + ", " + col);
                    throw new Exception("Row and/or column value out of bounds: " + row + ", " + col);
                }

                return slots[row, col];
            }

            public void MarkSlot(int row, int col)
            {
                if (row < 0 || row >= N || col < 0 || col >= N)
                {
                    Console.WriteLine("Row and/or column value out of bounds: " + row + ", " + col);
                    throw new Exception("Row and/or column value out of bounds: " + row + ", " + col);
                }

                slots[row, col].IsMarked = true;
            }

            public bool IsWinningBoard()
            {
                for (int r = 0;r < N; ++r)
                {
                    if (DoesRowHaveAllNumbersMarked(r))
                    {
                        return true;
                    }
                }

                for (int c = 0;c < N; ++c)
                {
                    if (DoesColumnHaveAllNumbersMarked(c))
                    {
                        return true;
                    }
                }

                return false;
            }

            public int[] GetNumber(int number)
            {
                for (int r = 0;r < N; ++r)
                {
                    for (int c = 0;c < N; ++c)
                    {
                        if (slots[r,c].Value == number)
                        {
                            return new int[] { r, c };
                        }
                    }
                }

                return new int[] { -1, -1 };
            }

            private bool DoesRowHaveAllNumbersMarked(int rowIndex)
            {
                bool isAllMarked = true;
                for(int c = 0;c < N; ++c)
                {
                    if (!slots[rowIndex, c].IsMarked)
                    {
                        isAllMarked = false;
                        break;
                    }
                }

                return isAllMarked;
            }

            private bool DoesColumnHaveAllNumbersMarked(int colIndex)
            {
                bool isAllMarked = true;
                for (int r = 0; r < N; ++r)
                {
                    if (!slots[r, colIndex].IsMarked)
                    {
                        isAllMarked = false;
                        break;
                    }
                }

                return isAllMarked;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int r = 0;r < N; ++r)
                {
                    for (int c = 0;c < N; ++c)
                    {
                        sb.Append(slots[r, c] + " ");
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }

        public class BingoGame
        {
            public List<Board> boards;
            public List<int> selectedNumbers;

            private int lastNumberPicked = -1;

            public BingoGame(List<Board> boards, List<int> selectedNumbers)
            {
                this.boards = boards;
                this.selectedNumbers = selectedNumbers;
            }

            // returns the 1st winning board
            public Board Play()
            {
                Board winningBoard = null;

                foreach(var num in selectedNumbers)
                {
                    if (winningBoard != null)
                    {
                        break;
                    }

                    lastNumberPicked = num;
                    Console.WriteLine("Drawing: " + num);

                    // Mark all boards with the selected number if available
                    foreach(var board in boards)
                    {
                        int[] coordinates = board.GetNumber(num);
                        if (coordinates[0] != -1)
                        {
                            int row = coordinates[0];
                            int col = coordinates[1];
                            board.MarkSlot(row, col);
                        }
                    }

                    // Find 1st winning board
                    foreach(var board in boards)
                    {
                        if (board.IsWinningBoard())
                        {
                            winningBoard = board;
                            break;
                        }
                    }
                }

                return winningBoard;
            }

            // returns the last board to win
            public Board PlayUntilAllBoardsWin()
            {
                // key = board | value = winIndex
                var winningBoards = new Dictionary<Board, int>();
                var wonBoards = new Dictionary<int, Board>();
                int winIndex = 0;

                foreach (var num in selectedNumbers)
                {
                    // stop the game - all boards have won
                    if (winningBoards.Count == boards.Count)
                    {
                        break;
                    }

                    lastNumberPicked = num;
                    Console.WriteLine("Drawing: " + num);

                    // Mark all boards with the selected number if available
                    foreach (var board in boards)
                    {
                        int[] coordinates = board.GetNumber(num);
                        if (coordinates[0] != -1)
                        {
                            int row = coordinates[0];
                            int col = coordinates[1];
                            board.MarkSlot(row, col);
                        }
                    }

                    foreach (var board in boards)
                    {
                        if (board.IsWinningBoard())
                        {
                            if (!winningBoards.ContainsKey(board))
                            {
                                winningBoards[board] = winIndex;
                                wonBoards[winIndex++] = board;
                            }
                        }
                    }
                }

                // get last winning board
                return wonBoards[winningBoards.Count - 1]; 
            }

            public int CalculateFinalScore(Board winningBoard)
            {
                int[] numbers = winningBoard.UnmarkedSlots.Select(slot => slot.Value).ToArray();
                return numbers.Sum() * lastNumberPicked;
            }
        }


        public static BingoGame ReadInputs(string path)
        {
            var reader = new StreamReader(path);

            List<int> selectedNumbers = new List<int>();
            List<Board> boards = new List<Board>();

            int rowCount = 0;
            int[][] numbers = new int[5][];

            try
            {
                do
                {
                    string rawInput = reader.ReadLine();

                    // skip new lines or whitespace
                    if (rawInput == "\n" || rawInput.Trim().Length == 0)
                    {
                        rowCount = 0;
                        continue;
                    }

                    if (selectedNumbers.Count == 0)
                    {
                        selectedNumbers = rawInput.Split(',').Select(num => int.Parse(num)).ToList();    
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            numbers = new int[5][];
                        }

                        if (rowCount < 5)
                        {
                            numbers[rowCount] = rawInput
                                .Split(' ')
                                .Where(num => num.Trim().Length != 0)
                                .Select(num => int.Parse(num))
                                .ToArray();
                            ++rowCount;

                            if (rowCount == 5)
                            {
                                // store numbers into slots
                                var board = Board.Build(numbers);
                                boards.Add(board);
                            }

                        }
                        else
                        {
                            rowCount = 0;
                        }

                    }
                }
                while (reader.Peek() != -1);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error reading path at: " + path);
                Console.WriteLine("error: " + e.Message);
            }
            finally
            {
                reader.Close();
            }

            return new BingoGame(boards, selectedNumbers);
        }
    }
}

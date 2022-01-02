using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace AdventOfCode2021.Day13
{
    public class TransparentOrigami
    {
        public enum FoldAxis { X, Y };

        public class FoldInstructions
        {
            public FoldAxis foldAxis;
            public int units;

            public FoldInstructions(string fold, int units)
            {
                this.units = units;
                foldAxis = fold == "x" ? FoldAxis.X : FoldAxis.Y;
            }
        }

        public class Coordinates
        {
            public int x, y;
            public Coordinates(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class TransparentPaperInfo
        {
            public List<Coordinates> coordinates;
            public List<FoldInstructions> instructions;

            public TransparentPaperInfo(List<Coordinates> coordinates, List<FoldInstructions> instructions)
            {
                this.coordinates = coordinates;
                this.instructions = instructions;
            }
        }

        public class TransparentPaper
        {
            public char[,] source;
            public int originalMaxX, originalMaxY;
            public int currentMaxX, currentMaxY;

            public TransparentPaper(char[,] source, int maxX, int maxY)
            {
                this.source = source;
                originalMaxX = currentMaxX = maxX;
                originalMaxY = currentMaxY = maxY;
            }

            // Difference - after paper is folded, collect all points that merge onto the same coordinate space
            // aka merged points after a fold is made (merged points that will now contain a # if at least the original space or the new space has a #)

            // Assumption: fold points will always be the midpoint selected
            public void FoldVertically(int foldIndex)
            {
                Debug.Assert(0 <= foldIndex && foldIndex < currentMaxY);
                // Debug.Assert(foldIndex == currentMaxY / 2);

                int topSideRowCount = 0;
                int botSideRowCount = 0;

                for (int i = 0;i < currentMaxY; ++i)
                {
                    if (i < foldIndex)
                    {
                        ++topSideRowCount;
                    }
                    else if (i > foldIndex)
                    {
                        ++botSideRowCount;
                    }
                }

                int offset = Math.Abs(topSideRowCount - botSideRowCount);

                int start = topSideRowCount > botSideRowCount ? offset : 0;
                int end = topSideRowCount < botSideRowCount ? currentMaxY - 1 - offset : currentMaxY - 1;
                while (start < end && start != foldIndex && end != foldIndex)
                {
                    for (int c = 0; c < currentMaxX; ++c)
                    {
                        // Merge # if either the top side or bottom side of paper contain a #
                        if (source[start, c] ==  '#' || source[end, c] == '#')
                        {
                            source[start, c] = '#';
                        }
                    }

                    ++start;
                    --end;
                }

                currentMaxY = foldIndex;
            }

            public void FoldHorizontally(int foldIndex)
            {
                Debug.Assert(0 <= foldIndex && foldIndex < currentMaxX);

                // Paper not be folded evenly, so determine where the starting or ending points begin overlap
                // after a fold has been made
                int leftSideRowCount = 0;
                int rightSideRowCount = 0;

                for (int i = 0; i < currentMaxX; ++i)
                {
                    if (i < foldIndex)
                    {
                        ++leftSideRowCount;
                    }
                    else if (i > foldIndex)
                    {
                        ++rightSideRowCount;
                    }
                }

                int offset = Math.Abs(leftSideRowCount - rightSideRowCount);


                int start = leftSideRowCount > rightSideRowCount ? offset : 0;
                int end = leftSideRowCount < rightSideRowCount ? currentMaxX - 1 - offset : currentMaxX - 1;
                // here we assume the paper can be perfectly folded in half every single time and that's not true
                while (start < end && start != foldIndex && end != foldIndex)
                {
                    for (int r = 0; r < currentMaxY; ++r)
                    {
                        // Merge # if either the top side or bottom side of paper contain a #
                        if (source[r, start] == '#' || source[r , end] == '#')
                        {
                            source[r, start] = '#';
                        }
                    }

                    ++start;
                    --end;
                }

                currentMaxX = foldIndex;
            }

            // CJCKBAPB
            public void FoldPaperUsingInstructions(List<FoldInstructions> instructions)
            {
                foreach (var instruction in instructions)
                {
                    if (instruction.foldAxis == FoldAxis.X)
                    {
                        FoldHorizontally(instruction.units);
                    }
                    else
                    {
                        // Y axis fold
                        FoldVertically(instruction.units);
                    }
                }
            }

            public int CountHashes()
            {
                int result = 0;

                for (int r = 0; r < currentMaxY; ++r)
                {
                    for (int c = 0; c < currentMaxX; ++c)
                    {
                        if (source[r,c] == '#')
                        {
                            ++result;
                        }
                    }
                }

                return result;
            }
        }


        public static TransparentPaperInfo ReadInputs(string filePath)
        {
            var reader = new StreamReader(filePath);

            var coordsList = new List<Coordinates>();
            var instructionsList = new List<FoldInstructions>();
            TransparentPaperInfo paper = null;

            try
            {
                do
                {
                    string line = reader.ReadLine();

                    // coordinates
                    if (line.Contains(','))
                    {
                        string[] parsedInputs = line.Split(',');
                        var coords = new Coordinates(int.Parse(parsedInputs[0]), int.Parse(parsedInputs[1]));
                        coordsList.Add(coords);
                    }
                    else if (line.Trim().Length == 0)
                    {
                        // new line or whiteline skip
                        continue;
                    }
                    else
                    {
                        // fold instructions
                        string parsedInputs = line.Substring(line.IndexOf('g') + 1);
                        string[] foldComponents = parsedInputs.Split('=');
                        var foldInstr = new FoldInstructions(foldComponents[0].Trim(), int.Parse(foldComponents[1]));
                        instructionsList.Add(foldInstr);
                    }

                }
                while (reader.Peek() != -1);
                paper = new TransparentPaperInfo(coordsList, instructionsList);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error reading file: " + e.Message);
            }
            finally
            {
                reader.Close();
            }

            return paper;
        }

        public static int[] GetMaxXYUnits(List<Coordinates> coordinateList)
        {
            Debug.Assert(coordinateList.Count > 0);

            int maxXUnit = coordinateList[0].x;
            int maxYUnit = coordinateList[0].y;

            for(int i = 1;i < coordinateList.Count; ++i)
            {
                var coord = coordinateList[i];
                maxXUnit = Math.Max(maxXUnit, coord.x);
                maxYUnit = Math.Max(maxYUnit, coord.y);
            }

            return new int[] { maxXUnit + 1, maxYUnit + 1 };
        }

        public static TransparentPaper DrawTransparentPaper(List<Coordinates> coordList, int[] maxXYUnits)
        {
            Debug.Assert(maxXYUnits.Length == 2);

            int row = maxXYUnits[1];
            int col = maxXYUnits[0];

            var paperResult = new char[row, col];

            for(int r = 0;r < row; ++r)
            {
                for(int c = 0;c < col; ++c)
                {
                    paperResult[r, c] = '.';
                }
            }

            foreach(var coords in coordList)
            {
                paperResult[coords.y, coords.x] = '#';
            }

            return new TransparentPaper(paperResult, col, row);
        }
    }
}

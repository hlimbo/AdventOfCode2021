using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day5
{
    // Another way is to grab all line segments
    // and plot a graph of dots
    // a dot becomes a 1 if a line is on it
    // a 1 becomes a 2 if another line is on it
    // a 2 becomes a 3 if another line is on it (etc)

    // parse input:
    // split by commas for a point
    // split by -> to get each individual point
        
    // plot a graph of points
    // replace dot with 1 if part of line is drawn in that location
    // increment number by 1 if another part of a line is drawn in that location
    // scan through entire graph and count the number of characters whose int value >= 2
    // return the answer :)

    // Look at every other line segment and see if the current line segment
    // intersects with the other line segment
    // x2 > x1

    // Problem: Determine the number of times a point gets overlapped
    // by 2 or more lines
    // line format x1, y1 -> x2, y2
    // For Part 1 only consider horizontal and vertical lines
    // where x1 = x2 or y1 = y2
    public class HydrothermalVenture
    {
        public struct Point
        {
            int x, y;
            public int X => x;
            public int Y => y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override string ToString()
            {
                return "(" + x + ", " + y + ")";
            }
        }

        public struct LineSegment
        {
            Point[] points;

            public Point[] Points => points;

            public LineSegment(Point a, Point b)
            {
                points = new Point[2] { a, b };
            }

            public override string ToString()
            {
                return "Begin: " + points[0].ToString() + " | End: " + points[1].ToString();
            }
        }

        public static List<LineSegment> ReadInputs(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);

            var lineSegments = new List<LineSegment>();

            try
            {
                do
                {
                    string rawInput = reader.ReadLine();
                    string[] stringPoints = rawInput.Split("->").Where(p => p.Trim().Length > 0).ToArray();
                    int[] pointA = stringPoints[0].Split(',').Select(strNum => int.Parse(strNum)).ToArray();
                    int[] pointB = stringPoints[1].Split(',').Select(strNum => int.Parse(strNum)).ToArray();

                    var point1 = new Point(pointA[0], pointA[1]);
                    var point2 = new Point(pointB[0], pointB[1]);

                    lineSegments.Add(new LineSegment(point1, point2));
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                Console.WriteLine("Error reading path at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }


            return lineSegments;
        }

        private static bool IsLineHorizontal(LineSegment line)
        {
            return line.Points[0].Y - line.Points[1].Y == 0;
        }

        private static bool IsLineVertical(LineSegment line)
        {
            return line.Points[0].X - line.Points[1].X == 0;
        }

        public static int[,] DrawDiagram(List<LineSegment> lines)
        {
            int maxX = 0;
            int maxY = 0;

            foreach (var line in lines)
            {
                maxX = Math.Max(line.Points[1].X, Math.Max(maxX, line.Points[0].X));
                maxY = Math.Max(line.Points[1].Y, Math.Max(maxY, line.Points[0].Y));
            }

            int[,] diagram = new int[maxY + 1, maxX + 1];

            foreach (var line in lines)
            {
                if (IsLineHorizontal(line))
                {
                    int startX = Math.Min(line.Points[0].X, line.Points[1].X);
                    int endX = Math.Max(line.Points[0].X, line.Points[1].X);

                    for (int c = startX; c <= endX; ++c)
                    {
                        diagram[line.Points[0].Y, c] += 1;
                    }

                }
                else if (IsLineVertical(line))
                {
                    int startY = Math.Min(line.Points[0].Y, line.Points[1].Y);
                    int endY = Math.Max(line.Points[0].Y, line.Points[1].Y);

                    for (int r = startY; r <= endY; ++r)
                    {
                        diagram[r, line.Points[0].X] += 1;
                    }
                }
            }

            return diagram;
        }

        public static int[,] DrawDiagramWithDiagonals(List<LineSegment> lines)
        {
            int maxX = 0;
            int maxY = 0;

            foreach (var line in lines)
            {
                maxX = Math.Max(line.Points[1].X, Math.Max(maxX, line.Points[0].X));
                maxY = Math.Max(line.Points[1].Y, Math.Max(maxY, line.Points[0].Y));
            }

            int[,] diagram = new int[maxY + 1, maxX + 1];

            foreach (var line in lines)
            {
                if (IsLineHorizontal(line))
                {
                    int startX = Math.Min(line.Points[0].X, line.Points[1].X);
                    int endX = Math.Max(line.Points[0].X, line.Points[1].X);

                    for (int c = startX; c <= endX; ++c)
                    {
                        diagram[line.Points[0].Y, c] += 1;
                    }

                }
                else if (IsLineVertical(line))
                {
                    int startY = Math.Min(line.Points[0].Y, line.Points[1].Y);
                    int endY = Math.Max(line.Points[0].Y, line.Points[1].Y);

                    for (int r = startY; r <= endY; ++r)
                    {
                        diagram[r, line.Points[0].X] += 1;
                    }
                }
                else
                {
                    // if p0.y < p1.y => +y
                    // if p0.y > p1.y => -y
                    // if p0.x < p1.x => +x
                    // if p0.x > p1.x => -x

                    int xDiff = line.Points[1].X - line.Points[0].X;
                    int yDiff = line.Points[1].Y - line.Points[0].Y;
                    int xStep = xDiff / Math.Abs(xDiff);
                    int yStep = yDiff / Math.Abs(yDiff);

                    for (
                            int r = line.Points[0].Y, c = line.Points[0].X; 
                            r != line.Points[1].Y + yStep && c != line.Points[1].X + xStep; 
                            r += yStep, c += xStep
                        )
                    {
                        diagram[r, c] += 1;
                    }
                }
            }

            return diagram;
        }

        public static int GetOverlappingPointsCount(int[,] diagram)
        {
            int count = 0;
            for (int r = 0;r < diagram.GetLength(0); ++r)
            {
                for (int c = 0;c < diagram.GetLength(1); ++c)
                {
                    if (diagram[r,c] >= 2)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }
    }
}

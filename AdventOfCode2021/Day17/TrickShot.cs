using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdventOfCode2021.Day17
{
    internal class TrickShot
    {
        public class TargetArea
        {
            public int minX, maxX;
            public int minY, maxY;

            public TargetArea (int minX, int maxX, int minY, int maxY)
            {
                this.minX = minX;
                this.maxX = maxX;
                this.minY = minY;
                this.maxY = maxY;
            }
        }

        public class Point
        {
            public int x, y;
            public Point (int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class Trajectory
        {
            public List<Point> points;
            public bool willLandOnTargetArea;

            public Trajectory()
            {
                points = new List<Point>();
            }
        }

        public static TargetArea ReadFile(string path)
        {
            var reader = new StreamReader(path);
            TargetArea area = null;
            try
            {
                string line = reader.ReadLine();
                string[] data = line.Split(' ');
                string[] components = data[1].Split(',');
                var xValues = components[0].Substring(2, components[0].Length - 2).Split("..").Select(d => int.Parse(d)).ToList();
                var yValues = components[1].Substring(2, components[1].Length - 2).Split("..").Select(d => int.Parse(d)).ToList();
                area = new TargetArea(xValues[0], xValues[1], yValues[0], yValues[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            return area;
        }

        public static List<Point> ReadPossibleWays(string path)
        {
            var points = new List<Point>();
            var reader = new StreamReader(path);
            try
            {
                do
                {
                    string line = reader.ReadLine();
                    var pointStrings = line.Split(' ').Where(p => p.Trim().Length > 0).ToArray();
                    foreach (var pointString in pointStrings)
                    {
                        var pointArr = pointString.Split(',').Select(p => int.Parse(p)).ToArray();
                        points.Add(new Point(pointArr[0], pointArr[1]));
                    }
                } 
                while (reader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            return points;
        }


        // Assume probe starting position is at 0,0
        public static Trajectory WillLandInTargetArea(int initialXVel, int initialYVel, TargetArea area)
        {
            var trajectory = new Trajectory();

            int x = 0;
            int y = 0;

            int currentXVel = initialXVel;
            int currentYVel = initialYVel;

            while (IsOutOfBounds(x, y, area))
            {
                trajectory.points.Add(new Point(x, y));

                x += currentXVel;
                y += currentYVel;


                if (currentXVel > 0)
                {
                    currentXVel -= 1;
                }
                else if (currentXVel < 0)
                {
                    currentXVel += 1;
                }

                currentYVel -= 1;

                // check if current pos based on current velocities has already passed the target area
                if ( (area.maxX < x && currentXVel > 0) || 
                    (area.minX > x && currentXVel < 0) || 
                    (area.minY > y && currentYVel < 0))
                {
                    trajectory.willLandOnTargetArea = false;
                    return trajectory;
                }

                // assuming that the probe falls straight down and has no more X velocity
                if (currentXVel == 0 && (area.maxX < x || area.minX > x))
                {
                    trajectory.willLandOnTargetArea = false;
                    return trajectory;
                }
            }

            trajectory.points.Add(new Point(x, y));
            trajectory.willLandOnTargetArea = true;
            return trajectory;
        }

        // PART 1 Answer for test_input: 9730
        public static int GetHighestApex(TargetArea area)
        {
            // Assume probe starts at 0, 0
            int highest = 0;

            // find the upper bound for maxXVel possible (reduce search space)
            int xDirection = area.minX > 0 ? 1 : area.minX < 0 ? -1 : 0;
            int yDirection = -1;

            int xUpper = area.minX;

            for (int xVel = 0; xVel != xUpper; xVel += xDirection)
            {
                for (int yVel = 1000; yVel > area.minY; yVel += yDirection)
                {
                    var trajectory = WillLandInTargetArea(xVel, yVel, area);
                    if (trajectory.willLandOnTargetArea)
                    {
                        int currentHighest = CalcHighestApex(trajectory.points);
                        highest = Math.Max(highest, currentHighest);
                    }
                }
            }

            return highest;
        }

        public static int CalcHighestApex(List<Point> points)
        {
            int highest = points[0].y;
            for (int i = 1; i < points.Count; ++i)
            {
                var point = points[i];
                highest = Math.Max(highest, point.y);
            }

            return highest;
        }

        public static bool IsOutOfBounds(int xPos, int yPos, TargetArea area)
        {
            return xPos > area.maxX || xPos < area.minX || yPos > area.maxY || yPos < area.minY;
        }

        // PART 2
        public static int GetDistinctInitialVelocitiesCount(TargetArea area)
        {
            int distinctWays = 0;

            for (int yVel = area.minY; yVel <= 1000; yVel += 1)
            {
                for (int xVel = 0; xVel <= area.maxX; xVel += 1)
                {
                    var trajectory = WillLandInTargetArea(xVel, yVel, area);
                    if (trajectory.willLandOnTargetArea)
                    {
                        Console.WriteLine(xVel + "," + yVel);
                        ++distinctWays;
                    }
                }
            }

            return distinctWays;
        }

    }
}

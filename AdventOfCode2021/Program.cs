using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day17;

namespace AdventOfCode2021
{
    class Program
    {
        // upper bound x = minX and upper bound y = minY

        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day17\\Inputs\\test_input.txt";
            var area = TrickShot.ReadFile(fullPath);
            int apex = TrickShot.GetDistinctInitialVelocitiesCount(area);
            Console.WriteLine(apex);

            //string fullPath2 = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day17\\Inputs\\possible_ways_sample.txt";
            //var points = TrickShot.ReadPossibleWays(fullPath2);
            //foreach (var point in points)
            //{
            //    var t = TrickShot.WillLandInTargetArea(point.x, point.y, area);
            //    if (!t.willLandOnTargetArea)
            //    {
            //        Console.WriteLine(point.x + "," + point.y);
            //    }
            //}
        }
    }
}

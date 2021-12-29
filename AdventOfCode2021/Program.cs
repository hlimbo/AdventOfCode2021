using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day12;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day12\\Inputs\\sample4.txt";
            List<string> caveConnections = PassagePathing.ReadInputs(fullPath);
            var caveSystem = PassagePathing.ConstructCaveSystem(caveConnections);
            int count = PassagePathing.CountUniquePaths(caveSystem, true);

            Console.WriteLine(count);

            //int paths = PassagePathing.CountUniqueCavePaths(rootCave);

            //Console.WriteLine("unique paths: " + paths);

            //var queue = new Queue<PassagePathing.GraphNode>();
            //queue.Enqueue(rootCave);
            //while (queue.Count > 0)
            //{
            //    var currentCave = queue.Dequeue();
            //    Console.WriteLine("visited cave: " + currentCave.name);
            //    foreach(var adjacentCave in currentCave.adjacentNodes)
            //    {
            //        queue.Enqueue(adjacentCave);
            //    }
            //}
        }
    }
}

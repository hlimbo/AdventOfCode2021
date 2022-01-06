using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day15;

namespace AdventOfCode2021
{
    class Program
    {
        // actual
        // "NCNBNHNNCB"
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day15\\Inputs\\big_input.txt";
            var riskLevelMap = Chiton.ReadInputs(fullPath);

            // answer: 388
            //int min = Chiton.Dijkstra(riskLevelMap);
            //Console.WriteLine(min);

            var caveSystem = Chiton.ExtendRiskTilesToFormCaveMap(riskLevelMap, 5);

            int rowCount = caveSystem.GetLength(0);
            int colCount = caveSystem.GetLength(1);

            //for (int r = 0; r < rowCount; ++r)
            //{
            //    for (int c = 0; c < colCount; ++c)
            //    {
            //        Console.Write(caveSystem[r, c]);
            //    }
            //    Console.WriteLine();
            //}


            var map = Chiton.ConvertJaggedArrayToListOfLists(caveSystem);
            var debugPath = Chiton.Dijkstra(map);

            // get path starting from end vertex to start vertex
            var current = debugPath.endVertex;
            int totalRiskCost = 0;
            while (current != null)
            {
                totalRiskCost += map[current.row][current.col];
                // Console.WriteLine(map[current.row][current.col]);
                current = debugPath.prevVertex[current];
            }

            // exclude start vertex
            totalRiskCost = totalRiskCost - map[0][0];

            // answer 2819
            Console.WriteLine("cost: " + totalRiskCost);


            // 2811 low number -- bad answer
            // Console.WriteLine(debugPath.riskCost);
        }
    }
}

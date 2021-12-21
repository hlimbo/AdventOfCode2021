using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day9;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day9\\Inputs\\small_input.txt";
            List<List<int>> heightValues = SmokeBasin.ReadInputs(fullPath);
            int product = SmokeBasin.GetLargestBasinsProduct(heightValues);
            Console.WriteLine(product);
        }
    }
}

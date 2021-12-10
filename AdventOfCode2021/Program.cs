using System;
using System.IO;
using System.Linq;
using AdventOfCode2021.Day7;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // big input part1: 348996
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day7\\Inputs\\big_input.txt";
            var inputs = TreacheryOfWhales.ReadInputs(fullPath);
            int minFuelCost = TreacheryOfWhales.GetMinimumFuelCost(inputs);
            Console.WriteLine(minFuelCost);
        }
    }
}

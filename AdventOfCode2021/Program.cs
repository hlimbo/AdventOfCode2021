using System;
using System.IO;
using System.Linq;
using AdventOfCode2021.Day6;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // big input part 1: 361169 after 80 days
            // big input part 2: 1634946868992 after 256 days
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day6\\Inputs\\big_input.txt";
            var inputs = Lanternfish.ReadInputs(fullPath);
            //var fishPopulation = Lanternfish.GetSimulatedPopulationGrowth(inputs, 80);
            var fishPopulation2 = Lanternfish.GetSimulatedPopulationGrowthLarge(inputs, 256);

           // Console.WriteLine(fishPopulation);
            Console.WriteLine(fishPopulation2);
        }
    }
}

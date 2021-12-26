using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day11;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day11\\Inputs\\big_input.txt";
            var octopuses = DumboOctopus.ReadInputs(fullPath);
            int step = DumboOctopus.GetStepNumberWhenAllOctopusesFlash(octopuses);

            Console.WriteLine("step number: " + step);
        }
    }
}

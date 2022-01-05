using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day14;

namespace AdventOfCode2021
{
    class Program
    {
        // actual
        // "NCNBNHNNCB"
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day14\\Inputs\\big_input.txt";
            var polymer = ExtendedPolymerization.ReadInputs(fullPath);
            string result = ExtendedPolymerization.ExtendPolymer(polymer, 10);
            int answer = ExtendedPolymerization.CalculateDiffBetweenMostCommonAndLeastCommonElements(result);
            long answer2 = ExtendedPolymerization.BuildPolymer(polymer, 40);

            Console.WriteLine(answer);
        }
    }
}

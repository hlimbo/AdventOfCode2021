using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day13;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day13\\Inputs\\big_input.txt";
            var manual = TransparentOrigami.ReadInputs(fullPath);

            int[] maxXYUnits = TransparentOrigami.GetMaxXYUnits(manual.coordinates);
            var transparentPaper = TransparentOrigami.DrawTransparentPaper(manual.coordinates, maxXYUnits);

            // transparentPaper.FoldHorizontally(655);

            transparentPaper.FoldPaperUsingInstructions(manual.instructions);

            for (int r = 0; r < transparentPaper.currentMaxY; ++r)
            {
                for (int c = 0; c < transparentPaper.currentMaxX; ++c)
                {
                    Console.Write(transparentPaper.source[r, c]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\n");

            int result = transparentPaper.CountHashes();

            Console.WriteLine("hello world: " + result);

        }
    }
}

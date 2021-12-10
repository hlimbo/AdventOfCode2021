using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2021.Day2
{
    // submarine commands
    /*
     *  forward - 1
     *  down - 2 (decrease depth)
     *  up - 3 (increase depth)
     *  X units in a direction can be applied
     * 
     * Can you pass in X < 0?
     * 
     * input:
     *   planned course
     *   
     *  figure out where the submarine is going
     *  
     *  facts:
     *  --> start at 0 horizontal and depth of 0
     * 
     * 
     * horizontal input ->
     * depth input ->L
     * 
     * output: multiply final horizontal input by final depth
     */

    public class Dive
    {
        public struct Command
        {
            public enum Direction
            {
                FORWARD = 1,
                DOWN = 2,
                UP = 3,
                NONE = 4,
            };

            private int units;
            public int Units => units;
            private Direction dir;
            public Direction Dir => dir;

            public Command(string direction, int units)
            {

                this.units = units;

                if (direction == "forward")
                {
                    this.dir = Direction.FORWARD;
                }
                else if (direction == "down")
                {
                    this.dir = Direction.DOWN;
                }
                else if (direction == "up")
                {
                    this.dir = Direction.UP;
                }
                else
                {
                    this.dir = Direction.NONE;
                }
            }
        }

        // read from a file location
        public static Command[] ReadInputs(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);
            var commands = new List<Command>();

            try
            {
                do
                {
                    string rawInput = reader.ReadLine();
                    string[] inputs = rawInput.Split(' ');
                    var command = new Command(inputs[0], int.Parse(inputs[1]));
                    commands.Add(command);
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                Console.WriteLine("Error reading path at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }

            return commands.ToArray();
        }

        // accepts a list of commands that move the submarine at a particular location
        // returns the final horizontal input and depth input by multiplying
        // the 2 inputs together
        public static int FindSubmarineProductLocation(Command[] inputs)
        {
            int horizontalUnits = 0;
            int depthUnits = 0;

            foreach (var input in inputs)
            {
                if (input.Dir == Command.Direction.DOWN)
                {
                    depthUnits += input.Units;
                }
                else if (input.Dir == Command.Direction.UP)
                {
                    depthUnits -= input.Units;
                }
                else
                {
                    // forward
                    horizontalUnits += input.Units;
                }
            }

            return horizontalUnits * depthUnits;
        }

        // Track 3rd value aim.... starts at 0
        // new commands 
        // down X increases aim by X units
        // up X decreases aim by X units
        // forward X:
        //  increases horizontal input by X units
        //  increases your depth by your aim multiplied by X
        // down means aiming at positive direction
        public static int FindSubmarineProductLocationPt2(Command[] commands)
        {
            int horizontalUnits = 0;
            int verticalUnits = 0;
            int aim = 0;

            foreach (var command in commands)
            {
                if (command.Dir == Command.Direction.FORWARD)
                {
                    horizontalUnits += command.Units;
                    verticalUnits += aim * command.Units;
                }
                else if (command.Dir == Command.Direction.UP)
                {
                    aim -= command.Units;
                }
                else if (command.Dir == Command.Direction.DOWN)
                {
                    aim += command.Units;
                }
            }

            return horizontalUnits * verticalUnits;
        }
    }
}

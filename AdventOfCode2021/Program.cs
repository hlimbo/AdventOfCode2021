using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day18;

namespace AdventOfCode2021
{
    class Program
    {
        private static List<int> GetIntsFromSnailfishNumber(Snailfish.SnailfishNumber snailfishNum)
        {
            var queue = new Queue<Snailfish.SnailfishNumber>();
            queue.Enqueue(snailfishNum);

            var regNums = new List<int>();

            var rightHandSideNums = new Stack<int>();

            while (queue.Count > 0)
            {
                var num = queue.Dequeue();

                if (num.leftFishPair != null)
                {
                    queue.Enqueue(num.leftFishPair);
                }
                else
                {
                    regNums.Add(num.leftRegNumber);
                }

                if (num.rightFishPair != null)
                {
                    queue.Enqueue(num.rightFishPair);
                }
                else
                {
                    if (num.leftRegNumber != -1 && num.rightRegNumber != -1)
                    {
                        regNums.Add(num.rightRegNumber);
                    }
                    else
                    {
                        // add right hand side values last to preserve left to right order
                        rightHandSideNums.Push(num.rightRegNumber);
                    }
                }
            }

            while (rightHandSideNums.Count > 0)
            {
                int regNum = rightHandSideNums.Pop();
                regNums.Add(regNum);
            }

            return regNums;
        }

        static void Main(string[] args)
        {
            // string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day18\\Inputs\\small_input.txt";
            //var area = TrickShot.ReadFile(fullPath);
            //int apex = TrickShot.GetDistinctInitialVelocitiesCount(area);
            //Console.WriteLine(apex);

            // to verify all numbers are in the Snailfish object, I just need to extract all the digits from the raw string, store each digit in an int list as expected answer
            // extract all regular numbers from Snailfish object using BFS if first snailfish element is reg number, add to actual list, otherwise enqueue snailfish number for further processing
            // for last element of snailfish number, if number is reg number, push to stack (will later append the right hand side numbers to actual list after BFS process completes)

            // Complete Step 1 parse out the thing
            // Up Next create a way to add 2 snail fish numbers together [easy] -> make a new snailfish number and set the first and second snailfish numbers in a "new array"
            // After work on explode operation [hard/tricky]
            // After work on split operation [medium]
            // After work on reduce function where it will be responsible for identifying when an explode or a split operation needs to be done based on current snailfish number [hard]
            // After work on writing magnitude function [easy]


            //string line = "[[[[[1,1],[2,2]],[3,3]],[4,4]],[5,5]]";
            //var snailfishNum = Snailfish.ConstructNumber(line);
            //var numToExplode = Snailfish.FindNumberToExplode(snailfishNum);
            //bool didMutate = Snailfish.ApplyExplosion(numToExplode);
            //numToExplode = Snailfish.FindNumberToExplode(snailfishNum);
            //didMutate = Snailfish.ApplyExplosion(numToExplode);
            //Console.WriteLine("done");


            // 1.Add 2 snailfish numbers
            // 2.Reduce resulting sum if applicable
            // 3. add current running sum to the next snailfish number
            // Repeat steps 2 and 3 until no more snailfish numbers are available to add 

            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day18\\Inputs\\big_input.txt";
            var lines = Snailfish.ReadFile(fullPath);
            var snailfishNums = Snailfish.ParseInputs(lines);

            int max = Snailfish.FindMaxMagnitude(snailfishNums);
            Console.WriteLine("num: " + max);

            //var snailfishNum = Snailfish.Calculate(snailfishNums);
            //int num = Snailfish.GetMagnitude(snailfishNum);
            //Console.WriteLine("num: " + num);


            // bad split picker case
            // [[[[0,8],[7,8]],[[23,0],20]],[[[0,4],6],[8,7]]]
            //string line = "[[[[0,8],[7,8]],[[23,0],20]],[[[0,4],6],[8,7]]]";
            //string line = "[[23,0],20]";
            //string line = "[5,[23,0]]";
            //string line = "[5,[0,23]]";
            //string line = "[[4,5],[8,23]]";
            //string line = "[[4,5],[23,26]]";

            // Debugging bad explode logic on left hand side
            // string line = "[[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]],[[[[4,2],2],6],[8,7]]]";


            //string line = "[[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]],[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]]";
            //var number = Snailfish.ConstructNumber(line);
            //number = Snailfish.Reduce(number);
            //int num = Snailfish.GetMagnitude(number);
            //Console.WriteLine("num: " + number);


            //List<int> expectedOutput = line.Where(c => "0123456789".Contains(c)).Select(d => (int)Char.GetNumericValue(d)).ToList();
            //List<int> actualOutput = GetIntsFromSnailfishNumber(snailfishNum);

            // int num = Snailfish.GetMagnitude(snailfishNum);



            // Console.WriteLine("w: " + num);
        }
    }
}

/*
 * 
 * Debug bad leftmost rightmost placement
 * 
 * [[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]

[4,5] explodes

[[[[4,0],[5,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]

[4,5] explodes

[[[[4,0],[5,4]],[[0,[7,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]

[7,6] explodes

[[[[4,0],[5,4]],[[7,0],[15,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]

[3,7] explodes

[[[[4,0],[5,4]],[[7,0],[15,5]]],[10,[[0,[11,3]],[[6,3],[8,8]]]]]

[11,3] explodes

[[[[4,0],[5,4]],[[7,0],[15,5]]],[10,[[11,0],[[9,3],[8,8]]]]]]

[9,3] explodes

[[[[4,0],[5,4]],[[7,0],[15,5]]],[10,[[11,9],[0,[11,8]]]]]]

[11,8] explodes

[[[[4,0],[5,4]],[[7,0],[15,5]]],[10,[[11,9],[11,0]]]]]]

15 splits

[[[[4,0],[5,4]],[[7,0],[[7,8],5]]],[10,[[11,9],[11,0]]]]]]

[7,8] explodes

[[[[4,0],[5,4]],[[7,7],[0,13]]],[10,[[11,9],[11,0]]]]]]

13 splits

[[[[4,0],[5,4]],[[7,7],[0,[6,7]]]],[10,[[11,9],[11,0]]]]]]

[6,7] explodes

[[[[4,0],[5,4]],[[7,7],[6,0]]],[17,[[11,9],[11,0]]]]]]

17 splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,9],[[11,9],[11,0]]]]]]

11 splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,9],[[[5,6],9],[11,0]]]]]]

[5,6] explodes

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,14],[[0,15],[11,0]]]]]]

14 splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[0,15],[11,0]]]]]]

15 splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[0,[7,8]],[11,0]]]]]]

[7,8] explodes

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,0],[19,0]]]]]]

19 splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,0],[[9,10],0]]]]]]

[9,10] explodes

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[0,10]]]]]]

10  splits

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[0,[5,5]]]]]]]

[5,5] explodes

[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]]]
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2021.Day18
{
    public class Snailfish
    {
        public static List<string> ReadFile(string path)
        {
            var lines = new List<string>();

            var reader = new StreamReader(path);
            try
            {
                do
                {
                    string line = reader.ReadLine();
                    lines.Add(line);
                }
                while (reader.Peek() != -1);
            }
            catch
            {

            }
            finally
            {
                reader.Close();
            }

            return lines;
        }

        public class SnailfishNumber
        {
            public int leftRegNumber, rightRegNumber;
            public SnailfishNumber leftFishPair, rightFishPair;
            public SnailfishNumber parent = null;

            public SnailfishNumber(int leftRegNumber, int rightRegNumber)
            {
                this.leftRegNumber = leftRegNumber;
                this.rightRegNumber = rightRegNumber;
                leftFishPair = rightFishPair = null;
            }

            public SnailfishNumber(SnailfishNumber left, SnailfishNumber right)
            {
                leftFishPair = left;
                rightFishPair = right;
                leftRegNumber = rightRegNumber = -1;
            }

            public SnailfishNumber(SnailfishNumber left, SnailfishNumber right, int leftNum, int rightNum)
            {
                leftRegNumber = leftNum;
                rightRegNumber = rightNum;
                leftFishPair = left;
                rightFishPair = right;
            }

            // Assume all reg numbers are >= 0
            public bool IsRegPair => leftRegNumber >= 0 && rightRegNumber >= 0;
            public bool HasNestedPairs => leftFishPair != null || rightFishPair != null;


        }

        // Explode
        public static SnailfishNumber FindNumberToExplode(SnailfishNumber root, int level = 0)
        {
            if (root == null)
            {
                return null;
            }

            if (level >= 4)
            {
                if (root.IsRegPair)
                {
                    return root;
                }
                else
                {
                    // If its another nested snailfish number keep looking for the snailfish number comprised of 2 regular numbers
                    // to explode...
                    // The problem was unclear of whether or not we start counting the nested levels starting at level 0 from the root node
                    // or starting from any arbitrary node at level 0

                    SnailfishNumber leftPair2 = null;
                    if (root.rightRegNumber >= 0)
                    {
                        leftPair2 = FindNumberToExplode(root.leftFishPair, level + 1);
                    }

                    // Prioritize returning the left most snailfish number to explode
                    if (leftPair2 != null)
                    {
                        return leftPair2;
                    }

                    if (root.leftRegNumber >= 0)
                    {
                        var rightPair2 = FindNumberToExplode(root.rightFishPair, level + 1);
                        return rightPair2;
                    }
                }
            }

            // No pair to explode found
            if (level < 4 && root.IsRegPair)
            {
                return null;
            }

            var leftPair = FindNumberToExplode(root.leftFishPair, level + 1);

            // we want to return the left most pair first to explode...
            // pairs that are elgible to explode but are further to the right will not be returned by this function
            if (leftPair != null)
            {
                return leftPair;
            }

            var rightPair = FindNumberToExplode(root.rightFishPair, level + 1);

            return rightPair;
        }

        // Returns true if snailfish number gets mutated, return false otherwise
        public static bool FindLeftImmediateAncestorToAdd(SnailfishNumber curr, SnailfishNumber prev, int leftElement, bool isLookingForRightMostNum = false)
        {

            bool result = false;
            if (curr.parent == null)
            {
                // if coming from left pair already, then that means we've reached the left most number and 
                // there are no more numbers to the left to consider
                if (prev == curr.leftFishPair)
                {
                    return false;
                }

                result = FindLeftImmediateAncestorToAdd(curr.leftFishPair, curr, leftElement, true);
            }
            // If I previously was from the left pair child, then go up to its parent
            else if (prev == curr.leftFishPair)
            {
                result = FindLeftImmediateAncestorToAdd(curr.parent, curr, leftElement, isLookingForRightMostNum);
            }
            else if (prev == curr.rightFishPair)
            {
                // base case .. check if the left pair we have is a regular number
                if (curr.leftFishPair == null && curr.leftRegNumber != -1)
                {
                    curr.leftRegNumber = curr.leftRegNumber + leftElement;
                    return true;
                }

                // if I was previously from the right pair child...look at left pair and look at right most child starting now
                if (curr.leftFishPair != null)
                {
                    result = FindLeftImmediateAncestorToAdd(curr.leftFishPair, curr, leftElement, true);
                }
            }
            else if (isLookingForRightMostNum)
            {
                // base case
                if (curr.rightFishPair == null && curr.rightRegNumber != -1)
                {
                    curr.rightRegNumber = curr.rightRegNumber + leftElement;
                    return true;
                }

                if (curr.rightFishPair != null)
                {
                    result = FindLeftImmediateAncestorToAdd(curr.rightFishPair, curr, leftElement, isLookingForRightMostNum);
                }
            }

            return result;
        }

        // Algorithm
        /*
         *   It was wrong to assume that we could just move up the tree up to the root BEFORE traversing the right snailfish numbers because we don't know if our num to explode is a left or right child of the parent snailfish number
         *   
         *   1. If numToExplode then recursively move up to the parent snailfish number (can simplify by passing in the prev node which will start off as the num to explode [x]
         *   
         *   2. If prevNode is the left child of the current node,
         *      2a. (Base Case) If right child is a regular number,
         *              add right element to right child
         *              return true to notify that we have mutated the snailfish number
         *      2b. If right child is a nested snailfish number (Recursion):
         *              mark that we are now looking for the leftmost regular number
         *              traverse to the right snailfish number
         *      
         * 
         *  3. If prevNode is the right child of the current node
         *      traverse up to the parent node (we can do this because we are looking for the rightmost snailfish number... going back to the right child will cause infinite cycles)
         *      
         *  4. If we reached the root level (parent == null)
         *      mark that we are now looking for the leftmost regular number
         *      traverse to the right snailfish number
         *      
         *  5. If we are marked to look for the leftmost regular number 
         *      5a. (Base Case) If left child is a regular number
         *          add right element to left child
         *          return true to notify that we have mutated the snailfish number
         *      5b. If left child is a snailfish number (Recursion)
         *          traverse to the left snailfish number
         * 
         */ 
        public static bool FindRightImmediateAncestorToAdd(SnailfishNumber curr, SnailfishNumber prev, int rightElement, bool isLookingforLeftMostNum = false)
        {
            bool result = false;

            // reach root level
            if (curr.parent == null)
            {
                // if coming from right pair already, then that means we reached the right most number and there are no
                // more numbers to the right to process
                if (prev == curr.rightFishPair)
                {
                    return false;
                }

                result = FindRightImmediateAncestorToAdd(curr.rightFishPair, curr, rightElement, true);
            }
            // if prev snailfish number is the right number of the current snailfish number
            else if (curr.rightFishPair == prev)
            {
                result = FindRightImmediateAncestorToAdd(curr.parent, curr, rightElement, isLookingforLeftMostNum);
            }
            // if prev snailfish number is the left number of the current snailfish number
            else if (curr.leftFishPair == prev)
            {
                // base case
                if (curr.rightRegNumber != -1 && curr.rightFishPair == null)
                {
                    curr.rightRegNumber = curr.rightRegNumber + rightElement;
                    return true;
                }

                // Mark to look for leftmost regular number starting from the right snailfish number of this current snailfish number
                if (curr.rightFishPair != null)
                {
                    result = FindRightImmediateAncestorToAdd(curr.rightFishPair, curr, rightElement, true);
                }
            }
            // marked to look for left most number
            else if (isLookingforLeftMostNum)
            {
                // base case
                if (curr.leftRegNumber != -1 && curr.leftFishPair == null)
                {
                    curr.leftRegNumber = curr.leftRegNumber + rightElement;
                    return true;
                }

                if (curr.leftFishPair != null)
                {
                    result = FindRightImmediateAncestorToAdd(curr.leftFishPair, curr, rightElement, isLookingforLeftMostNum);
                }
            }

            return result;
        }

        public static bool Explode(SnailfishNumber number)
        {
            Debug.Assert(number.parent != null);

            var parent = number.parent;
            if (number == parent.leftFishPair)
            {
                parent.leftFishPair = null;
                number = null;
                parent.leftRegNumber = 0;
                return true;
            }
            else if (number == parent.rightFishPair)
            {
                parent.rightFishPair = null;
                number = null;
                parent.rightRegNumber = 0;
                return true;
            }

            return false;
        }

        // Returns true if any mutations occurred in the snailfish number
        public static bool ApplyExplosion(SnailfishNumber numToExplode)
        {
            bool didLeftMutate = FindLeftImmediateAncestorToAdd(numToExplode.parent, numToExplode, numToExplode.leftRegNumber, false);
            bool didRightMutate = FindRightImmediateAncestorToAdd(numToExplode.parent, numToExplode, numToExplode.rightRegNumber, false);
            bool didExplode = Explode(numToExplode);

            return didLeftMutate || didRightMutate || didExplode;
        }


        // Finds parent snailfish number that contains a regular number to split
        public static SnailfishNumber FindNumberToSplit(SnailfishNumber root)
        {
            if (root == null)
            {
                return null;
            }

            if (root.leftFishPair == null && root.leftRegNumber >= 10)
            {
                return root;
            }
            // prevent prematurely returning the incorrect node if we haven't visited the left most node yet
            else if (root.leftFishPair == null && root.rightFishPair == null && root.rightRegNumber >= 10)
            {
                return root;
            }

            // apply left splits over right splits first
            var left = FindNumberToSplit(root.leftFishPair);

            // left most snail fish number
            if (left != null)
            {
                return left;
            }

            // middle snail fish number
            if (root.rightRegNumber >= 10)
            {
                return root;
            }

            var right = FindNumberToSplit(root.rightFishPair);

            // right most snail fish number
            return right;
        }

        public static SnailfishNumber Split(int number)
        {
            Debug.Assert(number >= 10);
            return new SnailfishNumber((int)Math.Floor(number / 2.0f), (int)Math.Ceiling(number / 2.0f));
        }

        public static SnailfishNumber ConstructNumber(string line)
        {
            // bracket stack push on [ pop on ]
            // object stack that will either contain a snailfish number or a int value
            // push a snailfish number back into stack in order to use it as a child
            // element to form parent snailfish number if applicable
            // push a number if we encounter a number while processing through line string
            // once a ] is encountered... pop the matching [ and pop the first 2 
            // numbers in object stack

            // check 2 numbers types
            // if snailfish and snailfish => use snailfish constructor
            // if number and number => use snailfish number constructor
            // if (snailfish and number) or (number and snailfish) => use snailfish mixed constructor
            // push snailfish number back to object stack to ensure we can use it as a possible element to form parent snailfish number

            // skip commas while processing line
            // push digits or numbers or snailfish numbers

            var bracketStack = new Stack<char>();
            // In C# all classes implicitly inherit from object
            // A number for this problem can either be an int or a Snailfish number
            // Using 2 different stacks to hold ints and Snailfish numbers will make
            // the logic much more complex which is why I decided to unify the 2 objects together
            // to eliminate the question, should I pop from the snailfish number stack or the int stack?
            var numberStack = new Stack<object>();

            // Assume all numbers are single digit from line string
            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (c == '[')
                {
                    bracketStack.Push(c);
                }
                else if (c == ']')
                {
                    bracketStack.Pop();

                    // Assume there will always be at least 2 numbers available on the stack
                    Debug.Assert(numberStack.Count >= 2);
                    object rawNum2 = numberStack.Pop();
                    object rawNum1 = numberStack.Pop();

                    SnailfishNumber snailfishNum = null;

                    // Evaluate what the number types are in order to create new Snailfish number
                    if (rawNum1 is string && rawNum2 is string)
                    {
                        int num1 = int.Parse((string)rawNum1);
                        int num2 = int.Parse((string)rawNum2);
                        snailfishNum = new SnailfishNumber(num1, num2);
                    }
                    else if (rawNum1 is SnailfishNumber && rawNum2 is SnailfishNumber)
                    {
                        var snail1 = (SnailfishNumber)rawNum1;
                        var snail2 = (SnailfishNumber)rawNum2;

                        snailfishNum = new SnailfishNumber(snail1, snail2);

                        snail1.parent = snailfishNum;
                        snail2.parent = snailfishNum;
                    }
                    else if (rawNum1 is string && rawNum2 is SnailfishNumber)
                    {
                        int num1 = int.Parse((string)rawNum1);
                        var snail2 = (SnailfishNumber)rawNum2;

                        snailfishNum = new SnailfishNumber(null, snail2, num1, -1);

                        snail2.parent = snailfishNum;
                    }
                    else
                    {
                        // rawNum1 is SnailfishNumber and rawNum2 is char
                        var snail1 = (SnailfishNumber)rawNum1;
                        int num2 = int.Parse((string)rawNum2);
                        
                        snailfishNum = new SnailfishNumber(snail1, null, -1, num2);

                        snail1.parent = snailfishNum;
                    }

                    // Push new Snailfish number onto number stack for further processing
                    numberStack.Push(snailfishNum);
                }
                else if ("0123456789".Contains(c))
                {
                    // account for multidigit numbers
                    var sb = new StringBuilder();
                    while ("0123456789".Contains(c))
                    {
                        sb.Append(c);
                        ++i;

                        if (i >= line.Length)
                        {
                            break;
                        }

                        c = line[i];
                    }

                    // don't skip the next token that's not a digit
                    i -= 1;

                    var numString = sb.ToString();
                    numberStack.Push(numString);
                }
            }

            // Assume by the time we finish processing the line, there should be
            // 1 number left on number stack which is the Snailfish Number
            Debug.Assert(numberStack.Count == 1);
            SnailfishNumber root = (SnailfishNumber)numberStack.Pop();

            return root;
        }

        // Note: may need to convert to longs if number calculations get too big
        public static int GetMagnitude(SnailfishNumber number)
        {
            int leftProduct = number.leftFishPair == null ?
                3 * number.leftRegNumber : 3 * GetMagnitude(number.leftFishPair);
            int rightProduct = number.rightFishPair == null ?
                2 * number.rightRegNumber : 2 * GetMagnitude(number.rightFishPair);

            return leftProduct + rightProduct;

        }

        public static SnailfishNumber Add(SnailfishNumber n1, SnailfishNumber n2)
        {
            var newNumber = new SnailfishNumber(n1, n2);
            
            // set parents to be new snail fish number created
            n1.parent = newNumber;
            n2.parent = newNumber;

            return newNumber;
        }

        // when a deep copy is made, I need to deep copy all the nodes contained PLUS update parent references to point
        // to newly copied nodes
        public static SnailfishNumber DeepCopy(SnailfishNumber number)
        {
            if (number == null)
            {
                return null;
            }

            var left = DeepCopy(number.leftFishPair);
            var right = DeepCopy(number.rightFishPair);

            var newCopy = new SnailfishNumber(left, right);

            if (left == null)
            {
                Debug.Assert(number.leftRegNumber >= 0);
                newCopy.leftRegNumber = number.leftRegNumber;
            }

            if (right == null)
            {
                Debug.Assert(number.rightRegNumber >= 0);
                newCopy.rightRegNumber = number.rightRegNumber;
            }

            // update parent references if NOT regular numbers
            if (left != null)
            {
                left.parent = newCopy;
            }

            if (right != null)
            {
                right.parent = newCopy;
            }

            return newCopy;

        }


        public static List<SnailfishNumber> ParseInputs(List<string> lines)
        {
            var numbers = new List<SnailfishNumber>();

            foreach (string line in lines)
            {
                SnailfishNumber number = ConstructNumber(line);
                numbers.Add(number);
            }

            return numbers;
        }

        public static SnailfishNumber Calculate(List<SnailfishNumber> numbers)
        {
            Debug.Assert(numbers.Count >= 2);
            var sum = Add(numbers[0], numbers[1]);
            sum = Reduce(sum);

            for(int i = 2;i < numbers.Count; ++i)
            {
                sum = Add(sum, numbers[i]);
                sum = Reduce(sum);
            }

            return sum;
        }

        public static int FindMaxMagnitude(List<SnailfishNumber> numbers)
        {
            // since not commutative, we will brute force try every single possible pair of 2 snailfish numbers to determine
            // max magnitude

            // when adding 2 snailfish numbers together, I need to create DEEP COPIES of the values to prevent
            // side effects from happening

            Debug.Assert(numbers.Count >= 2);
            var sum = Add(numbers[0], numbers[1]);
            var sumCopy = DeepCopy(sum);
            sumCopy = Reduce(sumCopy);

            int maxMagnitude = GetMagnitude(sumCopy);

            var nums = new List<int>();
            nums.Add(maxMagnitude);

            for (int i = 0;i < numbers.Count; ++i)
            {
                for (int j = 0;j < numbers.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    sum = Add(numbers[i], numbers[j]);
                    sumCopy = DeepCopy(sum);
                    sumCopy = Reduce(sumCopy);
                    int currentMagnitude = GetMagnitude(sumCopy);

                    nums.Add(currentMagnitude);

                    maxMagnitude = Math.Max(maxMagnitude, currentMagnitude);
                }
            }

            nums = nums.OrderBy(num => num).ToList();

            return maxMagnitude;
        }

        public static void PrintNumbers(SnailfishNumber num)
        {
            if (num == null)
            {
                return;
            }

            if (num.leftRegNumber != -1)
            {
                Console.Write(num.leftRegNumber + "L ");
            }

            PrintNumbers(num.leftFishPair);
            PrintNumbers(num.rightFishPair);

            if (num.rightRegNumber != -1)
            {
                Console.Write(num.rightRegNumber + "R ");
            }
        }


        public static SnailfishNumber Reduce(SnailfishNumber num)
        {
            SnailfishNumber numToExplode = null;
            SnailfishNumber numToSplit = null;

            numToExplode = FindNumberToExplode(num);
            if (numToExplode == null)
            {
                numToSplit = FindNumberToSplit(num);
            }

            //PrintNumbers(num);
            //Console.WriteLine("");

            //if (numToExplode != null)
            //{
            //    Console.WriteLine("Explode: " + numToExplode.leftRegNumber + "," + numToExplode.rightRegNumber);
            //}

            //if (numToSplit != null)
            //{
            //    Console.WriteLine("Split: " + (numToSplit.leftRegNumber >= 10 ? numToSplit.leftRegNumber : numToSplit.rightRegNumber));
            //}

            //Console.WriteLine("\n");

            while (numToExplode != null || numToSplit != null)
            {

                if (numToExplode != null)
                {
                    ApplyExplosion(numToExplode);
                }
                else if (numToSplit != null)
                {
                    if (numToSplit.leftRegNumber >= 10)
                    {
                        //Console.WriteLine("left splitting");
                        numToSplit.leftFishPair = Split(numToSplit.leftRegNumber);
                        numToSplit.leftFishPair.parent = numToSplit;
                        numToSplit.leftRegNumber = -1;
                    }
                    else if (numToSplit.rightRegNumber >= 10)
                    {
                        //Console.WriteLine("right splitting");
                        numToSplit.rightFishPair = Split(numToSplit.rightRegNumber);
                        numToSplit.rightFishPair.parent = numToSplit;
                        numToSplit.rightRegNumber = -1;
                    }
                    else
                    {
                        Debug.Fail("A number should have been split to create a new snailfish number");
                    }
                }

                // reset values
                numToExplode = null;
                numToSplit = null;

                //PrintNumbers(num);
                //Console.WriteLine("");
                numToExplode = FindNumberToExplode(num);
                if (numToExplode == null)
                {
                    numToSplit = FindNumberToSplit(num);
                }

                //if (numToExplode != null)
                //{
                //    Console.WriteLine("Explode: " + numToExplode.leftRegNumber + "," + numToExplode.rightRegNumber);
                //}

                //if (numToSplit != null)
                //{
                //    Console.WriteLine("Split: " + (numToSplit.leftRegNumber >= 10 ? numToSplit.leftRegNumber : numToSplit.rightRegNumber));
                //}

                //Console.WriteLine("\n");

            }

            return num;
        }

    }
}

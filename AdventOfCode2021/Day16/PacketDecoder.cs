using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2021.Day16
{

    public class PacketDecoder
    {
        private const long LITERAL_TYPE = 4;

        private const string ADD = "add";
        private const string MULT = "multiply";
        private const string MIN = "min";
        private const string MAX = "max";
        private const string LITERAL = "literal";
        private const string GREATER_THAN = "greaterThan";
        private const string LESS_THAN = "lessThan";
        private const string EQUAL_TO = "equalTo";

        private static readonly Dictionary<long, string> operatorTypes = new Dictionary<long, string>()
        {
            { 0, "add" },
            { 1, "multiply" },
            { 2, "min" },
            { 3, "max" },
            { 4, "literal" },
            { 5, "greaterThan" },
            { 6, "lessThan" },
            { 7, "equalTo" }
        };

        public static string ReadInputs(string path)
        {
            var reader = new StreamReader(path);
            string line = "";

            try
            {
                line = reader.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            return line;
        }

        public static string ConvertHexToBinary(string hexInput)
        {
            var conversionTable = new Dictionary<char, char[]>()
            {
                {'0', new char[] { '0', '0', '0', '0'} },
                {'1', new char[] { '0', '0', '0', '1'} },
                {'2', new char[] { '0', '0', '1', '0'} },
                {'3', new char[] { '0', '0', '1', '1'} },
                {'4', new char[] { '0', '1', '0', '0'} },
                {'5', new char[] { '0', '1', '0', '1'} },
                {'6', new char[] { '0', '1', '1', '0'} },
                {'7', new char[] { '0', '1', '1', '1'} },
                {'8', new char[] { '1', '0', '0', '0'} },
                {'9', new char[] { '1', '0', '0', '1'} },
                {'A', new char[] { '1', '0', '1', '0'} },
                {'B', new char[] { '1', '0', '1', '1'} },
                {'C', new char[] { '1', '1', '0', '0'} },
                {'D', new char[] { '1', '1', '0', '1'} },
                {'E', new char[] { '1', '1', '1', '0'} },
                {'F', new char[] { '1', '1', '1', '1'} }
            };

            var binaryValues = new List<char>();
            foreach (char hexValue in hexInput)
            {
                char[] subBinaryValues = conversionTable[hexValue];
                foreach (char binaryValue in subBinaryValues)
                {
                    binaryValues.Add(binaryValue);
                }
            }

            return new string(binaryValues.ToArray());
        }

        public static long ConvertBinaryToDecimal(string source)
        {
            long result = 0;
            for (int i = source.Length - 1;i >=0; --i)
            {
                char binary = source[i];

                if (binary == '1')
                {
                    long place = source.Length - 1 - i;
                    long num = (long)Math.Pow(2, place);
                    result += num;
                }
            }

            return result;
        }

        public static long CalculatePacketVersionsSum(char[] binaryValues)
        {

            string binarySource = new string(binaryValues);

            int bitCounter = 0;
            long packetVersionsSum = 0;

            do
            {
                if (bitCounter + 3 >= binarySource.Length)
                {
                    break;
                }

                long packetVersion = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 3));
                bitCounter += 3;

                if (bitCounter + 3 >= binarySource.Length)
                {
                    break;
                }

                long packetType = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 3));
                bitCounter += 3;

                if (bitCounter >= binarySource.Length)
                {
                    break;
                }

                Console.WriteLine("packet version: " + packetVersion);

                packetVersionsSum += packetVersion;

                if (packetType == LITERAL_TYPE)
                {
                    Console.WriteLine("packet type literal found: " + packetType);

                    // Advance in multiples of 5 bits until last group prefixed with 0 is found
                    bool isFinishedProcessing = true;
                    while (bitCounter < binarySource.Length)
                    {
                        string bitsOf5 = binarySource.Substring(bitCounter, 5);
                        bitCounter += 5;
                        if (bitsOf5[0] == '0')
                        {
                            isFinishedProcessing = false;
                            break;
                        }

                    }

                    // leave loop to prevent out of bounds exception
                    if (isFinishedProcessing)
                    {
                        break;
                    }
                }
                else // OPERATOR PACKET TYPE FOUND
                {
                    Console.WriteLine("Operator packet type found: " + packetType);

                    int lengthTypeId = (int)char.GetNumericValue(binarySource[bitCounter++]);
                    Console.WriteLine("length type: " + lengthTypeId);

                    if (lengthTypeId == 0)
                    {
                        if (bitCounter + 15 >= binarySource.Length)
                        {
                            break;
                        }

                        long subPacketsBitLength = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 15));
                        Console.WriteLine("subpackets bit length: " + subPacketsBitLength);
                        bitCounter += 15;
                    }
                    else if (lengthTypeId == 1)
                    {
                        if (bitCounter + 11 >= binarySource.Length)
                        {
                            break;
                        }

                        long subPacketsCount = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 11));
                        Console.WriteLine("sub packets count: " + subPacketsCount);
                        bitCounter += 11;
                    }
                }
            }
            while (bitCounter < binarySource.Length);


            return packetVersionsSum;
        }


        /* Attempt 1 -- Issue is that for big_input, more than 2 numbers get stored to evaluate the equal to operator */
        public static long CalculateAnswer(string binarySource)
        {
            long result = 0;

            var operatorStack = new Stack<string>();
            var literalStack = new Stack<long>();

            int bitCounter = 0;

            Console.WriteLine(binarySource);

            do
            {
                if (bitCounter + 3 >= binarySource.Length)
                {
                    break;
                }

                long packetVersion = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 3));
                bitCounter += 3;

                if (bitCounter + 3 >= binarySource.Length)
                {
                    break;
                }

                long packetType = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 3));
                bitCounter += 3;

                if (bitCounter >= binarySource.Length)
                {
                    break;
                }

                // Identify what operator type it is and push onto stack
                operatorStack.Push(operatorTypes[packetType]);

                if (packetType == LITERAL_TYPE)
                {
                    Console.WriteLine("packet type literal found: " + packetType);

                    // Advance in multiples of 5 bits until last group prefixed with 0 is found
                    bool isFinishedProcessing = true;
                    var sb = new StringBuilder();
                    while (bitCounter < binarySource.Length)
                    {
                        string bitsOf5 = binarySource.Substring(bitCounter, 5);
                        bitCounter += 5;
                        // Add last 4 bits of bits of 5 group as part of literal value
                        sb.Append(bitsOf5.Substring(1, 4));

                        // last group identified
                        if (bitsOf5[0] == '0')
                        {
                            isFinishedProcessing = false;
                            break;
                        }
                    }

                    long literalValue = ConvertBinaryToDecimal(sb.ToString());
                    Console.WriteLine("literal value: " + literalValue);
                    Console.WriteLine(sb.ToString());
                    literalStack.Push(literalValue);

                    // leave loop to prevent out of bounds exception
                    if (isFinishedProcessing)
                    {
                        break;
                    }
                }
                else // OPERATOR PACKET TYPE FOUND
                {
                    Console.WriteLine("Operator packet type found: " + operatorTypes[packetType]);


                    int lengthTypeId = (int)char.GetNumericValue(binarySource[bitCounter++]);
                    Console.WriteLine("length type: " + lengthTypeId);

                    if (lengthTypeId == 0)
                    {
                        if (bitCounter + 15 >= binarySource.Length)
                        {
                            break;
                        }

                        long subPacketsBitLength = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 15));
                        Console.WriteLine("subpackets bit length: " + subPacketsBitLength);
                        bitCounter += 15;
                    }
                    else if (lengthTypeId == 1)
                    {
                        if (bitCounter + 11 >= binarySource.Length)
                        {
                            break;
                        }

                        long subPacketsCount = ConvertBinaryToDecimal(binarySource.Substring(bitCounter, 11));
                        Console.WriteLine("sub packets count: " + subPacketsCount);
                        bitCounter += 11;
                    }
                }
            }
            while (bitCounter < binarySource.Length);

            // process the stacks
            while (operatorStack.Count > 0)
            {

                // How do we know  if the next packet is an operator that compares exactly 2 numbers?

                string currentOperator = operatorStack.Pop();

                // Assume that there is 1 literal value left and is safe to assume exit out of the loop
                if (currentOperator == LITERAL && operatorStack.Count == 0)
                {
                    result = literalStack.Pop();
                    break;
                }
                else
                {
                    var numbers = new List<long>();

                    // keep adding literal values to number list until an operator type is found
                    while (currentOperator == LITERAL && operatorStack.Count > 0 && literalStack.Count > 0)
                    {
                        currentOperator = operatorStack.Pop();
                        numbers.Add(literalStack.Pop());
                    }

                    // probably peek at the next value on the stack BEFORE computing... e.g. if the next operator is either a >, <, or = sign
                    // skip adding the first number as that is for the next operator if numbers count > 2
                    // store in temp variable and wait

                    var calcOperators = new string[] { ADD, MULT, MIN, MAX };
                    var compareOperators = new string[] { LESS_THAN, GREATER_THAN, EQUAL_TO };
                    int offset = 0;
                    //if (calcOperators.Contains(currentOperator))
                    //{
                    //    if (operatorStack.TryPeek(out string nextOperator) && compareOperators.Contains(nextOperator) && numbers.Count >= 2)
                    //    {
                    //        offset = 1;
                    //    }
                    //}

                    // push extra number back to literal stack as it won't be processed yet
                    if (offset == 1)
                    {
                        operatorStack.Push(LITERAL);
                        literalStack.Push(numbers[0]);
                    }


                    // identify which operation to perform
                    if (currentOperator == ADD)
                    {
                        long sum = numbers[offset];
                        for (int i = 1 + offset; i < numbers.Count; ++i)
                        {
                            sum += numbers[i];
                        }

                        operatorStack.Push(LITERAL);
                        literalStack.Push(sum);
                    }
                    else if (currentOperator == MULT)
                    {

                        long product = numbers[offset];
                        for (int i = 1 + offset; i < numbers.Count; ++i)
                        {
                            product *= numbers[i];
                        }

                        operatorStack.Push(LITERAL);
                        literalStack.Push(product);

                    }
                    else if (currentOperator == LESS_THAN)
                    {
                        Debug.Assert(numbers.Count == 2);
                        operatorStack.Push(LITERAL);
                        // gotcha: because numbers stored in a stack are in reverse, I need to compare the values
                        // in reverse order (e.g. the order they arrive in the stack)
                        int subResult = numbers[1] < numbers[0] ? 1 : 0;
                        literalStack.Push(subResult);
                    }
                    else if (currentOperator == GREATER_THAN)
                    {
                        Debug.Assert(numbers.Count == 2);
                        operatorStack.Push(LITERAL);
                        int subResult = numbers[1] > numbers[0] ? 1 : 0;
                        literalStack.Push(subResult);
                    }
                    else if (currentOperator == EQUAL_TO)
                    {
                        Debug.Assert(numbers.Count == 2);
                        operatorStack.Push(LITERAL);
                        int subResult = numbers[1] == numbers[0] ? 1 : 0;
                        literalStack.Push(subResult);
                    }
                    else if (currentOperator == MIN)
                    {
                        Debug.Assert(numbers.Count >= 1);

                        long min = numbers[offset];
                        for (int i = 1 + offset;i < numbers.Count; ++i)
                        {
                            min = Math.Min(min, numbers[i]);
                        }

                        operatorStack.Push(LITERAL);
                        literalStack.Push(min);
                    }
                    else if (currentOperator == MAX)
                    {
                        Debug.Assert(numbers.Count >= 1);

                        long max = numbers[offset];
                        for (int i = 1 + offset; i < numbers.Count; ++i)
                        {
                            max = Math.Max(max, numbers[i]);
                        }

                        operatorStack.Push(LITERAL);
                        literalStack.Push(max);
                    }
                }
                
            }


            return result;
        }


        /* Attempt 2 -- Was able to fix my issue by comparing a correct solution and adapting my solution to it by:
         * 
         * 1. Generating a packet tree that always has 2 numbers for comparer operators
         * 2. By using the length type packet info, I was able to determine which subpackets belonged to a particular packet
         * as opposed to not using the length type packet info which was causing issues when calculating the solution
         * 
         */
        public class Packet
        {
            public long packetVersion;
            public long packetType;

            public long literalValue;

            // Operator type info
            public int lengthTypeId;
            public int totalBitsLength;
            public int subPacketsCount;

            public List<Packet> subPackets;

            public Packet (long packetVersion, long packetType, long literalValue)
            {
                SetPacketMetaData(packetVersion, packetType);
                this.literalValue = literalValue;
            }

            public Packet (long packetVersion, long packetType, int lengthTypeId, int totalBitsLength, int subPacketsCount)
            {
                SetPacketMetaData(packetVersion, packetType);
                this.lengthTypeId = lengthTypeId;
                this.totalBitsLength = totalBitsLength;
                this.subPacketsCount = subPacketsCount;

                subPackets = new List<Packet>();
            }

            private void SetPacketMetaData(long packetVersion, long packetType)
            {
                this.packetVersion = packetVersion;
                this.packetType = packetType;
            }
        }

        public static List<Packet> ParseBinary(string binarySource, ref int index)
        {
            var packets = new List<Packet>();
            
            {
                string packetVer = binarySource.Substring(index, 3);
                string packetT = binarySource.Substring(index + 3, 3);
                index += 6;

                Console.WriteLine("packet ver number: " + packetVer);
                Console.WriteLine("packet T: " + packetT);

                long packetVersion = ConvertBinaryToDecimal(packetVer);
                long packetType = ConvertBinaryToDecimal(packetT);

                if (operatorTypes[packetType] == LITERAL)
                {
                    Tuple<long, int> pair = DecodeLiteralValue(binarySource, index);
                    
                    long literalValue = pair.Item1;
                    // update bit counter based on number of bits advanced in DecodeLiteralValue();
                    index = pair.Item2;

                    var literalPacket = new Packet(packetVersion, packetType, literalValue);
                    packets.Add(literalPacket);

                    Console.WriteLine("LITERAL VALUE: " + literalValue);
                }
                else
                {
                    int lengthTypeId = int.Parse(binarySource[index].ToString());
                    index += 1;
                    int totalBitsLength = 0;
                    int subPacketsCount = 0;

                    Console.WriteLine("operator packet: " + operatorTypes[packetType]);
                    Console.WriteLine("length type id: " + lengthTypeId);

                    if (lengthTypeId == 0)
                    {
                        string buf = binarySource.Substring(index, 15);
                        totalBitsLength = (int)ConvertBinaryToDecimal(buf);
                        index += 15;
                    }
                    else if (lengthTypeId == 1)
                    {
                        string buf = binarySource.Substring(index, 11);
                        subPacketsCount = (int)ConvertBinaryToDecimal(buf);
                        index += 11;
                    }

                    var operatorPacket = new Packet(packetVersion, packetType, lengthTypeId, totalBitsLength, subPacketsCount);
                    if (lengthTypeId == 0)
                    {
                        int bitsEnd = index + totalBitsLength;
                        while (index < bitsEnd)
                        {
                            operatorPacket.subPackets = operatorPacket.subPackets.Concat(ParseBinary(binarySource, ref index)).ToList();
                        }
                    }
                    else
                    {
                        for (int i = 0;i < subPacketsCount; ++i)
                        {
                            operatorPacket.subPackets = operatorPacket.subPackets.Concat(ParseBinary(binarySource, ref index)).ToList();
                        }
                    }

                    packets.Add(operatorPacket);
                }
            }

            return packets;
        }

        public static Tuple<long, int> DecodeLiteralValue(string binarySource, int bitCounter)
        {
            var sb = new StringBuilder();
            int i = bitCounter;
            for (;i < binarySource.Length; i += 5)
            {
                bool isLastGroup = binarySource[i] == '0';
                string sub = binarySource.Substring(i + 1, 4);
                sb.Append(sub);

                if (isLastGroup)
                {
                    i += 5;
                    break;
                }
            }

            long value = ConvertBinaryToDecimal(sb.ToString());
            return new Tuple<long, int>(value, i);
        }

        public static long CalculatePacketAnswer(List<Packet> packets, long parentOperator)
        {
            var buffer = new List<long>();

            if (packets.Count == 0)
            {
                return 0;
            }

            if (packets.Count == 1 && operatorTypes[packets[0].packetType] == LITERAL)
            {
                return packets[0].literalValue;
            }

            long mainResult = 0;
            foreach (var packet in packets)
            {
                if (operatorTypes[packet.packetType] == LITERAL)
                {
                    buffer.Add(packet.literalValue);
                }
                else
                {
                    long subResult = CalculatePacketAnswer(packet.subPackets, packet.packetType);
                    buffer.Add(subResult);
                }
            }

            if (buffer.Count > 0 && parentOperator != -1)
            {
                mainResult = ExecuteOperation(buffer, parentOperator);
            }
            else if (buffer.Count == 1)
            {
                mainResult = buffer[0];
            }

            return mainResult;
        }

        public static long ExecuteOperation(List<long> numbers, long operatorType)
        {
            long answer = 0;
            switch (operatorTypes[operatorType])
            {
                case ADD:
                    answer = numbers.Sum();
                    break;
                case MULT:
                    answer = Mult(numbers);
                    break;
                case MIN:
                    answer = Min(numbers);
                    break;
                case MAX:
                    answer = Max(numbers);
                    break;
                case GREATER_THAN:
                    answer = GreaterThan(numbers);
                    break;
                case LESS_THAN:
                    answer = LessThan(numbers);
                    break;
                case EQUAL_TO:
                    answer = EqualTo(numbers);
                    break;
            }

            return answer;
        }

        private static long Mult(List<long> numbers)
        {
            Debug.Assert(numbers.Count >= 1);
            long product = numbers[0];
            for (int i = 1; i < numbers.Count; ++i)
            {
                product *= numbers[i];
            }

            return product;
        }

        private static long Min(List<long> numbers)
        {
            Debug.Assert(numbers.Count >= 1);
            long min = numbers[0];
            for (int i = 1;i < numbers.Count; ++i)
            {
                min = Math.Min(min, numbers[i]);
            }

            return min;
        }

        private static long Max(List<long> numbers)
        {
            Debug.Assert(numbers.Count >= 1);
            long max = numbers[0];
            for (int i = 1; i < numbers.Count; ++i)
            {
                max = Math.Max(max, numbers[i]);
            }

            return max;
        }

        private static long GreaterThan(List<long> numbers)
        {
            Debug.Assert(numbers.Count == 2);
            return numbers[0] > numbers[1] ? 1 : 0;
        }

        private static long LessThan(List<long> numbers)
        {
            Debug.Assert(numbers.Count == 2);
            return numbers[0] < numbers[1] ? 1 : 0;
        }

        private static long EqualTo(List<long> numbers)
        {
            Debug.Assert(numbers.Count == 2);
            return numbers[0] == numbers[1] ? 1 : 0;
        }

        /* Attempt 3  -- based on https://github.com/RiotNu/advent-of-code/blob/main/AdventOfCode/2021/Puzzle16.cpp */
        public static Packet ReadPacket(string binarySource, ref int currentBit)
        {
            Packet result = null;
            long packetVer = ConvertBinaryToDecimal(binarySource.Substring(currentBit, 3));
            long packetType = ConvertBinaryToDecimal(binarySource.Substring(currentBit + 3, 3));
            currentBit += 6;

            if (packetType == 4)
            {
                // identify all groups of 5 bits that have prefix 1 set
                var sb = new StringBuilder();

                while (binarySource[currentBit] == '1')
                {
                    ++currentBit;
                    sb.Append(binarySource.Substring(currentBit, 4));
                    currentBit += 4;
                }

                // last group of 5 bits that have prefix 0 set
                ++currentBit;
                sb.Append(binarySource.Substring(currentBit, 4));
                currentBit += 4;

                long literalValue = ConvertBinaryToDecimal(sb.ToString());
                result = new Packet(packetVer, packetType, literalValue);
            }
            else
            {
                // Operator Packet
                int lengthTypeId = int.Parse(binarySource[currentBit++].ToString());
                if (lengthTypeId == 0)
                {
                    int subPacketBitCount = (int)ConvertBinaryToDecimal(binarySource.Substring(currentBit, 15));
                    currentBit += 15;

                    result = new Packet(packetVer, packetType, lengthTypeId, subPacketBitCount, 0);

                    int subPacketsEnd = currentBit + subPacketBitCount;
                    while (currentBit < subPacketsEnd)
                    {
                        result.subPackets.Add(ReadPacket(binarySource, ref currentBit));
                    }
                }
                else
                {
                    // length type id 1
                    int subPacketsCount = (int)ConvertBinaryToDecimal(binarySource.Substring(currentBit, 11));
                    currentBit += 11;

                    result = new Packet(packetVer, packetType, lengthTypeId, 0, subPacketsCount);

                    for (int i = 0;i < subPacketsCount; ++i)
                    {
                        result.subPackets.Add(ReadPacket(binarySource, ref currentBit));
                    }
                }

            }

            return result;
        }

        public static long EvaluatePacket(Packet packet)
        {
            if (packet.packetType == 4)
            {
                return packet.literalValue;
            }

            var evaluatedSubpackets = new List<long>();
            foreach (var subPacket in packet.subPackets)
            {
                evaluatedSubpackets.Add(EvaluatePacket(subPacket));
            }

            switch (operatorTypes[packet.packetType])
            {
                case ADD:
                    return evaluatedSubpackets.Sum();
                case MULT:
                    return Mult(evaluatedSubpackets);
                case MIN:
                    return Min(evaluatedSubpackets);
                case MAX:
                    return Max(evaluatedSubpackets);
                case GREATER_THAN:
                    return GreaterThan(evaluatedSubpackets);
                case LESS_THAN:
                    return LessThan(evaluatedSubpackets);
                case EQUAL_TO:
                    return EqualTo(evaluatedSubpackets);
                default:
                    Console.WriteLine("invalid operator type: " + packet.packetType);
                    break;
            }

            return 0;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode2021.Day16;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // part 1: 947
            string fullPath = "C:\\Users\\limbo\\source\\repos\\AdventOfCode2021\\AdventOfCode2021\\day16\\Inputs\\equal_to_input.txt";
            string hexValues = PacketDecoder.ReadInputs(fullPath);
            string binaryValues = PacketDecoder.ConvertHexToBinary(hexValues);

            //long sum = PacketDecoder.CalculatePacketVersionsSum(binaryValues);
            //Console.WriteLine("sum: " + sum);

            int bitCounter = 0;
            var packet = PacketDecoder.ReadPacket(binaryValues, ref bitCounter);
            var answer = PacketDecoder.EvaluatePacket(packet);

            int bitCounter2 = 0;
            var packets = PacketDecoder.ParseBinary(binaryValues, ref bitCounter2);
            long answer2 = PacketDecoder.CalculatePacketAnswer(packets, -1);

            // 660797830937
            Console.WriteLine(answer);

            //long result = PacketDecoder.CalculateAnswer(binaryValues);
            //Console.WriteLine(result);
        }
    }
}

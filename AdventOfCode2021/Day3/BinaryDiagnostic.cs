using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Day3
{

    // 1st parameter check its power consumption using binary numbers
    // generate 2 new binary numbers -> gamma and epsilon rate
    // multiply espilon with gamma rate to obtain power consumption

    /*
     * 
     * How to find gamma rate?
     * find the most common bit in the corresponding position of all numbers in diagnostic report
     * 
     * To find most common bit, look at the ith bit for all binary numbers and count the number of 1s and 0s
     * whichever has more is equal to the most common bit for ith bit for all binary numbers
     * combine bits together to form the gamma rate!
     * 
     * How to find the epsilon rate?
     * find the least common ith bit for all binary numbers
     */

    public class BinaryDiagnostic
    {
        public static int ConvertToBase10(string input)
        {
            int value = 0;
            for (int i = input.Length - 1;i >= 0; --i)
            {
                if (int.Parse(input[i].ToString()) == 1)
                {
                    value += (int)Math.Pow(2, (input.Length - 1) - i);
                }
            }

            return value;
        }

        public static int[] ReadReportValues(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);
            var reportValues = new List<int>();

            try
            {
                do
                {
                    string input = reader.ReadLine();

                    // Add in the number of bits from first line
                    // Assumption: all inputs have equal number of bits
                    if (reportValues.Count == 0)
                    {
                        reportValues.Add(input.Length);
                    }

                    // convert to base 10 from string value
                    int value = ConvertToBase10(input);
                    reportValues.Add(value);
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                Console.WriteLine("Could not read file at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }

            return reportValues.ToArray();
        }

        public static string[] ReadReportValuesString(string pathLocation)
        {
            var reader = new StreamReader(pathLocation);
            var reportValues = new List<string>();

            try
            {
                do
                {
                    string input = reader.ReadLine();
                    reportValues.Add(input);
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                Console.WriteLine("Could not read file at: " + pathLocation);
            }
            finally
            {
                reader.Close();
            }

            return reportValues.ToArray();
        }

        public static int GetSubmarinePowerConsumption(int[] reportValues, int bitLength)
        {
            int[] frequencies = FindOneAndZeroFrequencies(reportValues, bitLength);
            return FindGammaRate(frequencies) * FindEpsilonRate(frequencies);
        }

        public static int[] FindOneAndZeroFrequencies(int[] reportValues, int bitLength)
        {
            // even indices represent 1s frequencies
            // odd indices represent 0s frequencies
            var frequencies = new int[bitLength * 2];
            for (int i = 0, k = 0; i < bitLength; ++i, k += 2)
            {
                int onesCount = 0;
                int zeroesCount = 0;

                for (int j = 0; j < reportValues.Length; ++j)
                {
                    int msb = 1 << i;
                    if ((reportValues[j] & msb) != 0)
                    {
                        ++onesCount;
                    }
                    else
                    {
                        ++zeroesCount;
                    }
                }

                frequencies[k] = onesCount;
                frequencies[k + 1] = zeroesCount;
            }

            return frequencies;
        }

        public static int FindGammaRate(int[] frequencies)
        {
            int gammaRate = 0;

            for (int i = 0;i < frequencies.Length; i += 2)
            {
                int onesCount = frequencies[i];
                int zeroesCount = frequencies[i + 1];

                if (onesCount > zeroesCount)
                {
                    // Note: each bit in binary represents a value 2 raised to the ith / 2 bit
                    gammaRate += (int)Math.Pow(2, i / 2);
                }
            }

            return gammaRate;
        }

        public static int FindEpsilonRate(int[] frequencies)
        {
            int epsilonRate = 0;
            for (int i = 0;i < frequencies.Length; i += 2)
            {
                int onesCount = frequencies[i];
                int zeroesCount = frequencies[i + 1];

                if (onesCount < zeroesCount)
                {
                    epsilonRate += (int)Math.Pow(2, i / 2);
                }
            }

            return epsilonRate;
        }

        public static int GetSubmarineLifeSupportRating(string[] reportValues, int bitLength)
        {
            Func<int, int, bool> oxygenFilterCriteria = (onesCount, zeroesCount) => onesCount >= zeroesCount;
            int oxygenRating = GetFilterCriteriaRating(reportValues, bitLength, oxygenFilterCriteria);

            Func<int, int, bool> cO2FilterCriteria = (onesCount, zeroesCount) => onesCount < zeroesCount;
            int cO2Rating = GetFilterCriteriaRating(reportValues, bitLength, cO2FilterCriteria);

            return oxygenRating * cO2Rating;
        }

        private static int[] GetBinaryFrequencyCounts(List<string> reportValues, int bitIndex)
        {
            int onesCount = 0;
            int zeroesCount;

            for (int j = 0; j < reportValues.Count; ++j)
            {
                if (reportValues[j][bitIndex] == '1')
                {
                    ++onesCount;
                }
            }

            // Clever trick: to not need to do extra work to count as we loop through the character array
            // since there is only 2 kinds of values (1 or 0)
            zeroesCount = reportValues.Count - onesCount;

            return new int[] { onesCount, zeroesCount };
        }

        private static int GetFilterCriteriaRating(string[] reportValues, int bitLength, Func<int, int, bool> filterCriteria)
        {
            var filterValues = new List<string>(reportValues);
            int rating = 0;
            for (int i = 0; i < bitLength; ++i)
            {
                var counts = GetBinaryFrequencyCounts(filterValues, i);
                int onesCount = counts[0];
                int zeroesCount = counts[1];

                if (filterCriteria(onesCount, zeroesCount) == true)
                {
                    filterValues = filterValues.Where(value => value[i] == '0').ToList();
                }
                else
                {
                    filterValues = filterValues.Where(value => value[i] == '1').ToList();
                }

                if (filterValues.Count == 1)
                {
                    rating = ConvertToBase10(filterValues[0]);
                    break;
                }
            }

            return rating;
        }

    }
}

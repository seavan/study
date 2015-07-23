using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace merge_sort_test
{
    public static class ArrayHelper
    {
        public static void PrintArray(this int[] array, string s)
        {
            Console.Write(s);
            foreach (var c in array)
            {
                Console.Write(c);
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }
    class Program
    {
        private static int[] A = new[] { 1, 2, 7, 3, 4, 5 };


        public static int[] MergeSort(int[] input, ref long inversions)
        {
            if (input.Length <= 1)
            {
                return input;
            }

            var leftCount = input.Length/2;
            var rightCount = input.Length - leftCount;
            var left = new int[leftCount];
            var right = new int[rightCount];

            int k = 0;
            for (int i = 0; i < leftCount; ++i)
            {
                left[i] = input[k++];
            }

            for (int i = 0; i < rightCount; ++i)
            {
                right[i] = input[k++];
            }

            left = MergeSort(left, ref inversions);
            right = MergeSort(right, ref inversions);

            int a = 0, b = 0;

            for (k = 0; k < input.Length; ++k)
            {
                if ( (b >= right.Length) || (a < left.Length && left[a] < right[b]))
                {
                    input[k] = left[a++];
                }
                else
                {
                    inversions += left.Length - a;

                    input[k] = right[b++];
                }
            }

            return input;
        }

        static int[] LoadFile(string fileName)
        {
            var result = new List<int>();

            var file = File.OpenText(fileName);

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    result.Add(int.Parse(line));                    
                }
                
            }

            return result.ToArray();
        }


        static void Main(string[] args)
        {
            long inv = 0;
            /*A.PrintArray("Input array: ");

            MergeSort(A, ref inv);

            A.PrintArray("Merge sort: ");
            Console.WriteLine("Inversions: {0}", inv);

            var arrayLength = 20;



            var zeroInversionsArray = new int[arrayLength];

            for (int i = 0; i < zeroInversionsArray.Length; ++i)
            {
                zeroInversionsArray[i] = i + 1;
            }

            var maxInversionsArray = new int[arrayLength];

            for (int i = 0; i < maxInversionsArray.Length; ++i)
            {
                maxInversionsArray[i] = maxInversionsArray.Length - i;
            }

            zeroInversionsArray.PrintArray("Zero inversions array:");

            inv = 0;
            MergeSort(zeroInversionsArray, ref inv);

            Console.WriteLine("Inversions: {0}", inv);

            maxInversionsArray.PrintArray("Max inversions array:");
            inv = 0;
            MergeSort(maxInversionsArray, ref inv);
            maxInversionsArray.PrintArray("Max inversions array sorted:");
            Console.WriteLine("Inversions: {0}, pre-calculated: {1}", inv, maxInversionsArray.Length *(maxInversionsArray.Length - 1) / 2);


            Console.ReadKey();
            */
            inv = 0;

            A = LoadFile("IntegerArray.txt");
            Console.WriteLine("Total lines: {0}", A.Length);
            MergeSort(A, ref inv);
            Console.WriteLine("Final Inversions: {0}", inv);
        }
    }
}

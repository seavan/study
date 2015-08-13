using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quick_sort_test
{
    public static class ArrayHelper
    {
        public static void PrintArray(this int[] array, string s)
        {
            Console.Write(s);
            foreach (var c in array.Take(20))
            {
                Console.Write(c);
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }
    class Program
    {
        private static int[] A = new[] { 7, 5, 1, 4, 8, 3, 10, 2, 6, 9 };
        private static int[] B = new[] { 8, 10, 1, 9, 7, 2, 6, 3, 5, 4 };

        public static void Swap(int[] input, int a, int b)
        {
            var c = input[a];
            input[a] = input[b];
            input[b] = c;
        }

        public static void QuickSort(int[] input, int l, int r, ref long swaps, Func<int[], int, int, int> pivotFunctor)
        {
            if (l + 1 >= r)
            {
                return;
            }
            swaps += r - l - 1;
            // choose pivot as first element
            var pivot = pivotFunctor(input, l, r);

            var i = l;

            for (int j = l + 1; j < r; ++j)
            {
                if (input[j] < pivot)
                {
                    ++i;
                    Swap(input, j, i);
                }
            }

            Swap(input, l, i);

            QuickSort(input, l, i, ref swaps, pivotFunctor);
            QuickSort(input, i + 1, r, ref swaps, pivotFunctor);

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

        public static long QuickSortFirst(int[] A)
        {
            long comparisons = 0;
            QuickSort(A, 0, A.Length, ref comparisons, (T, l, r) => { return T[l]; } );
            return comparisons;
        }

        public static long QuickSortLast(int[] A)
        {
            long comparisons = 0;
            QuickSort(A, 0, A.Length, ref comparisons, (T, l, r) => { Swap(T, r - 1, l); return T[l]; });
            return comparisons;
        }

        public static long QuickSortMedian(int[] A)
        {
            long comparisons = 0;
            QuickSort(A, 0, A.Length, ref comparisons, (T, l, r) =>
            {
                var dict = new Dictionary<int, int>();
                var indexes = new int[] {l, r - 1, l + (r - l - 1)/2};

                foreach (var i in indexes)
                {
                    dict[i] = A[i];
                }

                var medianIndex = dict.OrderBy(s => s.Value).ToArray()[1].Key;

                Swap(T, l, medianIndex);

                return T[l];
            });
            return comparisons;
        }


        public static void SortTypes(int[] input)
        {
            long comparisons = 0;
            Console.WriteLine("================");
            var firstPivotArray = (int[])input.Clone();
            Console.WriteLine("First pivot method");
            firstPivotArray.PrintArray("Initial array: ");
            comparisons = QuickSortFirst(firstPivotArray);
            firstPivotArray.PrintArray("Sorted array (first pivot): ");
            Console.WriteLine("Total comparisons: {0}", comparisons);
            Console.WriteLine();
            var secondPivotArray = (int[])input.Clone();
            Console.WriteLine("Last pivot method");
            secondPivotArray.PrintArray("Initial array: ");
            comparisons = QuickSortLast(secondPivotArray);
            secondPivotArray.PrintArray("Sorted array (last pivot): ");
            Console.WriteLine("Total comparisons: {0}", comparisons);
            Console.WriteLine();
            var thirdPivotArray = (int[])input.Clone();
            Console.WriteLine("Median pivot method");
            thirdPivotArray.PrintArray("Initial array: ");
            comparisons = QuickSortMedian(thirdPivotArray);
            thirdPivotArray.PrintArray("Sorted array (median pivot): ");
            Console.WriteLine("Total comparisons: {0}", comparisons);
            Console.WriteLine("///////////////////");
        }

        static void Main(string[] args)
        {
            SortTypes(A);
            SortTypes(B);
            A = LoadFile("QuickSort.txt");
            Console.WriteLine("Total lines: {0}", A.Length);
            SortTypes(A);
            //Console.WriteLine("Final Inversions: {0}", inv);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QuickGraph; // enables extension methods
using QuickGraph.Algorithms;

namespace graph_scc
{
    class Program
    {
        static SEdge<int>[] LoadFile(string fileName)
        {
            Console.WriteLine("Started reading file {0}", fileName);
            var result = new List<SEdge<int>>();

            var file = File.OpenText(fileName);

            while (!file.EndOfStream)
            {
                var line = file.ReadLine().Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    var edge = line.Split(' ', '\t').Select(s => int.Parse(s)).ToArray();
                    result.Add(new SEdge<int>(edge[0], edge[1]));
                }

            }

            Console.WriteLine("Read the file");
            return result.ToArray();
        }
        static void Main(string[] args)
        {
            var edges = LoadFile("big_test.txt");
            var graph = edges.ToBidirectionalGraph<int, SEdge<int>>(true);

            IDictionary<int, int> sccItems = new Dictionary<int, int>();
            var sccCount = graph.StronglyConnectedComponents(out sccItems);
            Console.WriteLine("SCC count: {0}", sccCount);

            var sccResult = new Dictionary<int, int>();
            var MIN_COUNT = 5;
            var fixedCount = Math.Max(sccCount, MIN_COUNT);
            for (int i = 0; i < fixedCount; ++i)
            {
                sccResult[i] = 0;
            }

            foreach (var i in sccItems)
            {
                //Console.WriteLine("{0}:{1}", i.Key, i.Value);
                sccResult[i.Value]++;
            }

            Console.WriteLine("Sorted SCC counts");

            var sccResultSorted = sccResult.OrderByDescending(s => s.Value).Take(MIN_COUNT);

            foreach (var i in sccResultSorted)
            {
                //Console.WriteLine("{0}: {1} vertices", i.Key, i.Value);
            }

            var answer = String.Join(",", sccResultSorted.Select(s => s.Value).OrderByDescending(s => s));
            Console.WriteLine("Answer: {0}", answer);
            

            
        }
    }
}

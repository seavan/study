using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace graph_test
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

    public class VertexNaive
    {
        public int Id;
        //public VertexNaiveList Adjacent = new VertexNaiveList();
    }

    public class VertexNaiveList : List<VertexNaive>
    {
        
    }

    public class VertexNaiveSortedList : SortedList<int, VertexNaive>
    {
        
    }

    public class EdgeNaive
    {
        public VertexNaive Tail;
        public VertexNaive Head;
    }

    public class EdgeNaiveList : List<EdgeNaive>
    {
        
    }


    public class GraphNaive
    {
        public void PrintReformat()
        {
            Console.WriteLine(VertexCount);
            for (int i = 0; i < VertexCount; ++i)
            {
                for (int j = 0; j < VertexCount; ++j)
                {
                    var v1 = VertexSorted.Values[i];
                    var v2 = VertexSorted.Values[j];
                    if (
                        Edges.Any(
                            s =>
                                (s.Head.Id == v1.Id && s.Tail.Id == v2.Id) || (s.Head.Id == v2.Id && s.Tail.Id == v1.Id)))
                    {
                        Console.Write('1');
                    }
                    else
                    {
                        Console.Write('0');
                    }
                    Console.Write(' ');
                }

                Console.WriteLine();
            }
        }
        public void Print()
        {
            foreach (var v in VertexSorted.Values)
            {
                Console.Write("{0}\t", v.Id);

                var items = Edges.Where(s => v.Id == s.Head.Id).Select(s => s.Tail.Id);

                foreach (var i in items)
                {
                    Console.Write("{0}\t", i);
                }

                items = Edges.Where(s => v.Id == s.Tail.Id).Select(s => s.Head.Id);

                foreach (var i in items)
                {
                    Console.Write("{0}\t", i);
                }

                Console.WriteLine();
            }
        }

        public VertexNaive Get(int Id)
        {
            if (!VertexSorted.ContainsKey(Id))
            {
                VertexSorted[Id] = new VertexNaive() {Id = Id};
            }

            return VertexSorted[Id];
        }

        public void Adjacent(VertexNaive vertex, int id)
        {
            bool addEdge = !VertexSorted.ContainsKey(id);
            var vertex2 = Get(id);

            if (!Edges.Any(s => s.Head == vertex && s.Tail == vertex2))
            {
                Edges.Add(new EdgeNaive() {Tail = vertex, Head = vertex2});
            }
        }

        public int EdgesCount 
        {
            get { return Edges.Count; }
        }

        public int VertexCount
        {
            get { return VertexSorted.Count; }
        }

        public GraphNaive Contract(EdgeNaive edge)
        {
            var graph = new GraphNaive();

            var oldvertexId = edge.Tail.Id;
            var newvertexId = edge.Head.Id;

            //Console.WriteLine("Mergin {0} to {1}", );

            foreach (var oldEdge in Edges)
            {
                var vertex1 = oldEdge.Tail.Id == oldvertexId ? graph.Get(newvertexId) : graph.Get(oldEdge.Tail.Id);
                var vertex2 = oldEdge.Head.Id == oldvertexId ? graph.Get(newvertexId) : graph.Get(oldEdge.Head.Id);

                if (vertex1.Id == vertex2.Id)
                {
                    continue;
                }

                graph.Adjacent(vertex1, vertex2.Id);
            }

            return graph;
        }

        public GraphNaive ContractRandom(GraphNaive graph)
        {
            Console.WriteLine("========================================: {0}", graph.VertexCount);
            graph.Print();
            if (graph.VertexCount == 2)
            {
                return graph;
            }
            var r = new Random();
            var edge = graph.Edges[r.Next(0, graph.Edges.Count)];
            return ContractRandom(graph.Contract(edge));
        }

        private EdgeNaiveList Edges = new EdgeNaiveList();
        private VertexNaiveSortedList VertexSorted = new VertexNaiveSortedList();
    }

    class Program
    {
        static GraphNaive LoadFile(string fileName)
        {
            var result = new List<int>();

            var file = File.OpenText(fileName);

            var graph = new GraphNaive();

            while (!file.EndOfStream)
            {
                var line = file.ReadLine().Trim();
                var items = line.Split(' ', '\t').Select(s => int.Parse(s)).ToArray();
                if (items.Length > 0)
                {
                    var vertex = graph.Get(items[0]);

                    for (int i = 1; i < items.Length; ++i)
                    {
                        graph.Adjacent(vertex, items[i]);
                    }

                }
            }

            return graph;
        }

        static void Main(string[] args)
        {
            var graph = LoadFile("test.txt");
            graph.PrintReformat();
            /*
            var tries = graph.VertexCount;

            Console.WriteLine("Edges count: {0}", graph.EdgesCount);
            Console.WriteLine("Vertex count: {0}", graph.VertexCount);


            */
            //for (int i = 0; i < tries; ++i)
            //{
              //  var contracted = graph.ContractRandom(graph);
              //  Console.WriteLine("Contracted edges count: {0}", contracted.EdgesCount);
            //}
            //Console.WriteLine("Contracted edges count: {0}", contracted.EdgesCount);
        }
    }
}

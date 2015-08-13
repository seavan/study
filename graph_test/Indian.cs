using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph_test
{
    public class Indian
    {
        // Adjacency list for representing graph
        Dictionary<int, List<int>> adjacencyList;

        public void Main(string fname)
        {
            int count = 1000;
            ReadGraphFromFile(fname);
            int numberOfCuts = 100000;
            int highestValueCount;
            for (int i = 0; i < 1000; i++)
            {
                highestValueCount = 0;
                while (adjacencyList.Count > 2)
                {
                    Tuple<int, int> tuple = GetRandomVertexs();
                    AppendNodes(tuple.Item1, tuple.Item2);
                    RemoveSelfLoops();
                }
                foreach (KeyValuePair<int, List<int>> data in adjacencyList)
                {
                    if (data.Value.Count > highestValueCount)
                        highestValueCount = data.Value.Count;
                }

                if (count > highestValueCount)
                    count = highestValueCount;
            }
            //adjacencyList.Dump();
            Console.WriteLine("Count {0}: ", count);

        }

        public void ReadGraphFromFile(string fname)
        {
            string[] number = File.ReadAllLines(fname);
            adjacencyList = new Dictionary<int, List<int>>();

            for (int i = 0; i < number.Length; i++)
            {
                int[] items = number[i].Split(' ').Where(x => !String.IsNullOrWhiteSpace(x)).Select(x => Convert.ToInt32(x)).ToArray();
                adjacencyList.Add(items[0], items.Skip(1).ToList<int>());
            }
        }

        public Tuple<int, int> GetRandomVertexs()
        {
            Random random = new Random();
            int randomVertex = random.Next(0, adjacencyList.Count());
            var keyArray = adjacencyList.Select(x => x.Key).ToArray();
            int item1 = keyArray[randomVertex]; //Get the main edge
            int randomEdgeVertex = random.Next(0, adjacencyList[item1].Count);
            int item2 = adjacencyList[item1].ToArray()[randomEdgeVertex];
            return new Tuple<int, int>(item1, item2);
        }

        public void AppendNodes(int parentNode, int toBeMergedNode)
        {
            try
            {
                var sourceList = adjacencyList[parentNode];
                var listOfDestinatioNodes = adjacencyList[toBeMergedNode];

                foreach (var number in listOfDestinatioNodes)
                {
                    if (!sourceList.Contains(number) && number != parentNode)
                    {
                        sourceList.Add(number);
                    }
                }

                sourceList.Remove(toBeMergedNode);

                foreach (KeyValuePair<int, List<int>> source in adjacencyList)
                {
                    if (source.Value.Contains(toBeMergedNode))
                    {
                        int count = source.Value.RemoveAll(x => x == toBeMergedNode);
                        for (int i = 0; i < count; i++)
                            source.Value.Add(parentNode);
                    }
                }

                adjacencyList[parentNode] = sourceList;
                adjacencyList.Remove(toBeMergedNode);
            }
            catch (Exception)
            {
                Console.WriteLine("ParentNode : {0} & ToBeMergedNode : {1}", parentNode, toBeMergedNode);
                //adjacencyList[parentNode].Dump();
                //adjacencyList[toBeMergedNode].Dump();
                throw;
            }
        }

        public void RemoveSelfLoops()
        {
            foreach (KeyValuePair<int, List<int>> data in adjacencyList)
            {
                int key = data.Key;
                List<int> listOfItems = adjacencyList[key];
                adjacencyList[key].RemoveAll(x => x == key);
            }
        }

        // Define other methods and classes here
    }


}

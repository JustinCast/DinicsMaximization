using System;
using System.Collections.Generic;
using System.Linq;

namespace DinicsMaximization
{
    class Program
    {
  

        //************************************

        public static List<Edge>[] createGraph(int nodes)
        {
            List<Edge>[] graph = new List<Edge>[nodes];
            for (int i = 0; i < nodes; i++)
                graph[i] = new List<Edge>();
            return graph;
        }

        public static void minimunConnected(List<Edge>[] graph)
        {
            int size = graph.Length;
            Random random = new Random();
            int capacity;
            for (int i = 0; i < graph.Length; i++)
            {
                capacity = random.Next(1, graph.Length);
                if ((i + 1) != graph.Length)
                    addEdge(graph, i, i + 1, capacity);
                else
                    addEdge(graph, i, i - 1, capacity);
            }
        }

        public static void mediumConnected(List<Edge>[] graph)
        {
            Random random = new Random();
            for (int i = 0; i < graph.Length; i++)
            {
                if (i == (graph.Length - 3))
                {
                    addEdge(graph, i, i + 1, random.Next(1, graph.Length));
                    addEdge(graph, i, i + 2, random.Next(1, graph.Length));
                    addEdge(graph, i, 0, random.Next(1, graph.Length));
                }
                else if (i == (graph.Length - 2))
                {
                    addEdge(graph, i, i + 1, random.Next(1, graph.Length));
                    addEdge(graph, i, 0, random.Next(1, graph.Length));
                    addEdge(graph, i, 1, random.Next(1, graph.Length));
                }
                else if(i == (graph.Length - 1))
                {
                    addEdge(graph, i, 0, random.Next(1, graph.Length));
                    addEdge(graph, i, 1, random.Next(1, graph.Length));
                    addEdge(graph, i, 2, random.Next(1, graph.Length));
                }
                else
                {
                    addEdge(graph, i, i + 1, random.Next(1, graph.Length));
                    addEdge(graph, i, i + 2, random.Next(1, graph.Length));
                    addEdge(graph, i, i + 3, random.Next(1, graph.Length));
                }
            }
        }

        public static void stronglyConnected(List<Edge>[] graph)
        {
            Random random = new Random();
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (j != i)
                        addEdge(graph, i, j, random.Next(1, graph.Length - 1));
                }
            }
        }

        public static void addEdge(List<Edge>[] graph, int s, int t, int cap)
        {
            //Console.WriteLine("graph[t].Count(): " + graph[t].Count());
            //Console.WriteLine("graph[s].Count() - 1: " + (graph[s].Count() - 1));
            //Console.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||");
            graph[s].Add(new Edge(t, s, cap));
            graph[t].Add(new Edge(s, t, 0));
        }

        public static void Fill<T>(T[] array, int start, int end, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (start < 0 || start >= end)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            //if (end >= array.Length)
            //{
            //    throw new ArgumentOutOfRangeException("toIndex");
            //}
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        static bool dinicBfs(List<Edge>[] graph, int src, int dest, int[] dist)
        {
            Fill(dist, 0, dist.Count(), -1);
            dist[src] = 0;
            int[] Q = new int[graph.Length];
            int sizeQ = 0;
            Q[sizeQ++] = src;
            for (int i = 0; i < sizeQ; i++)
            {
                int u = Q[i];
                foreach (Edge e in graph[u])
                {
                    if (dist[e.t] < 0 && e.f < e.cap)
                    {
                        dist[e.t] = dist[u] + 1;
                        Q[sizeQ++] = e.t;
                    }
                }
            }
            return dist[dest] >= 0;
        }

        public static int maxFlow(List<Edge>[] graph, int src, int dest)
        {
            int flow = 0;
            List<Edge>[] graph2 = graph;
            int[] dist = new int[graph2.Length];
            while (dinicBfs(graph2, src, dest, dist))
            {
                int[] ptr = new int[graph2.Length];
                while (true)
                {
                    int df = dinicDfs(graph2, ptr, dist, dest, src, Int32.MaxValue);
                    if (df == 0)
                        break;
                    flow += df;
                }
            }
            return flow;
        }

        static int dinicDfs(List<Edge>[] graph, int[] ptr, int[] dist, int dest, int u, int f)
        {
            if (u == dest)
                return f;
            for (; ptr[u] < graph[u].Count(); ++ptr[u])
            {
                Edge e = graph[u].ElementAt(ptr[u]);
                if (dist[e.t] == dist[u] + 1 && e.f < e.cap)
                {
                    int df = dinicDfs(graph, ptr, dist, dest, e.t, Math.Min(f, e.cap - e.f));
                    if (df > 0)
                    {
                        e.f += df;
                        graph[e.t].ElementAt(0).f -= df;
                        return df;
                    }
                }
            }
            return 0;
        }
        
        static void Main(string[] args)
        {
            int size = 100;
            List<Edge>[] graphD1 = createGraph(size);
            minimunConnected(graphD1);
            List<Edge>[] graphD2 = createGraph(size);
            mediumConnected(graphD2);
            List<Edge>[] graphD3 = createGraph(size);
            stronglyConnected(graphD3);

            //Random random = new Random();
            //addEdge(graph, 0, 1, 3);
            //addEdge(graph, 0, 2, 2);
            //addEdge(graph, 1, 2, 2); ;
            //Console.WriteLine(4 == maxFlow(graph, 0, 2));
            //Console.WriteLine(maxFlow(graph, 0, 3));
            //Console.WriteLine(vertexConnections);           
            //int count = 0;
            //for (int i = 0; i < graphD1.Length; i++)
            //{
            //    foreach (Edge e in graphD1[i])
            //    {
            //        if (e.cap != 0)
            //        {
            //            count++;
            //            Console.WriteLine(count);
            //            Console.WriteLine("VALOR VERTICE 2: " + e.t);
            //            Console.WriteLine("VALOR VERTICE 1: " + e.rev);
            //            Console.WriteLine("VALOR CAPACITY: " + e.cap);
            //            Console.WriteLine("VALOR FLOW: ");
            //            Console.WriteLine("---------------------");
            //        }
            //    }
            //}
            printMaxFlow(graphD2);
            new FordFulkersonAlgo(graphD1, graphD2, graphD3);
            Console.ReadKey();
        }

        static void printMaxFlow(List<Edge>[] graph)
        {
            Console.WriteLine("|||||| MAXIMUN FLOWS ||||||");            
            Console.WriteLine(maxFlow(graph, 0, graph.Length - 1));
        }

    }
}
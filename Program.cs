using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    class Program
    {
        public static List<Edge>[] createGraph(int nodes)
        {
            List<Edge>[] graph = new List<Edge>[nodes];
            for (int i = 0; i < nodes; i++)
                graph[i] = new List<Edge>();
            return graph;
        }
        public static void addEdge(List<Edge>[] graph, int s, int t, int cap)
        {
            //Console.WriteLine("graph[t].Count(): " + graph[t].Count());
            //Console.WriteLine("graph[s].Count() - 1: " + (graph[s].Count() - 1));
            //Console.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||");
            graph[s].Add(new Edge(t, graph[t].Count(), cap));
            graph[t].Add(new Edge(s, graph[s].Count() - 1, 0));
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
            int[] dist = new int[graph.Length];
            while (dinicBfs(graph, src, dest, dist))
            {
                int[] ptr = new int[graph.Length];
                while (true)
                {
                    int df = dinicDfs(graph, ptr, dist, dest, src, Int32.MaxValue);
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
                        graph[e.t].ElementAt(e.rev).f -= df;
                        return df;
                    }
                }
            }
            return 0;
        }

        static void Main(string[] args)
        {
            /**
             Escenarios para tomar en cuenta:
                - Conexión mínima
                - Conexión fuertemente conexa
                - Conexión con 3 arcos por vértice
             */
            List<Edge>[] graph = createGraph(5);
            addEdge(graph, 0, 1, 3);
            addEdge(graph, 0, 2, 2);
            addEdge(graph, 1, 2, 2);
            addEdge(graph, 0, 3, 4);
            addEdge(graph, 1, 3, 3);
            //Console.WriteLine(4 == maxFlow(graph, 0, 2));
            Console.WriteLine(maxFlow(graph, 0, 3));
            for (int i = 0; i < graph.Length; i++)
            {
                foreach (Edge e in graph[i])
                {
                    Console.WriteLine("VALOR VERTICE 2: " + e.t);
                    Console.WriteLine("VALOR VERTICE 1: " + e.rev);
                    Console.WriteLine("VALOR CAPACITY: " + e.cap);
                    Console.WriteLine("VALOR FLOW: " + e.f);
                    Console.WriteLine("---------------------");
                }
            }
            //printMaxFlow(graph);
            Console.ReadKey();
        }

        static void printMaxFlow(List<Edge>[] graph)
        {
            Console.WriteLine("MAXIMOS FLUJOS");
            Console.WriteLine("Fuente = 0, Destino = 2 | " + maxFlow(graph, 0, 2));
            Console.WriteLine("Fuente = 0, Destino = 1 | " + maxFlow(graph, 0, 1));
            Console.WriteLine("Fuente = 1, Destino = 2 | " + maxFlow(graph, 1, 2));
            Console.WriteLine("Fuente = 2, Destino = 3 | " + maxFlow(graph, 2, 3));
            Console.WriteLine("Fuente = 3, Destino = 1 | " + maxFlow(graph, 3, 1));
        }
    }
}

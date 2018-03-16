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
            if (end >= array.Length)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }
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
            List<Edge>[] graph = createGraph(3);
            addEdge(graph, 0, 1, 3);
            addEdge(graph, 0, 2, 2);
            addEdge(graph, 1, 2, 2);
            Console.WriteLine(4 == maxFlow(graph, 0, 2));
        }
    }
}

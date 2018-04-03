﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DinicsMaximization
{
    class Program
    {
        static int A = 0;
        static int C = 0;

        // FordFulkerson
        Boolean bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            Boolean[] visited = new Boolean[V];
            for (int i = 0; i < V; ++i)
            {
                visited[i] = false;
                A += 2;
                C += 1;
            }
            A += 1;
            C += 1;

            LinkedList<int> queue = new LinkedList<int>();
            queue.AddFirst(s);
            visited[s] = true;
            parent[s] = -1;
            A += 2;

            while (queue.Count != 0)
            {
                C += 1;
                int u = queue.First();
                A += 1;
                queue.Remove(u);

                for (int v = 0; v < V; v++)
                {
                    C += 1;
                    A += 1;
                    if (visited[v] == false && rGraph[u, v] > 0)
                    {
                        C += 2;
                        queue.AddFirst(v);
                        parent[v] = u;
                        visited[v] = true;
                        A += 2;
                    }
                }
                C += 1;
                A += 1;
            }
            C += 1;
            return (visited[t] == true);

        }

        // FordFulkerson
        int fordFulkerson(int[,] graph, int s, int t)
        {
            int u, v;
            int[,] rGraph = new int[V, V];

            for (u = 0; u < V; u++)
            {
                A += 1;
                C += 1;
                for (v = 0; v < V; v++)
                {
                    rGraph[u, v] = graph[u, v];
                    A += 2;
                    C += 1;
                }
                A += 1;
                C += 1;
            }
            A += 1;
            C += 1;


            int[] parent = new int[V];

            int max_flow = 0;
            A += 1;
            while (bfs(rGraph, s, t, parent))
            {
                int path_flow = int.MaxValue;
                A += 1;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                    A += 4;
                    C += 1;
                }
                A += 1;
                C += 1;

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                    A += 5;
                    C += 1;
                }
                A += 1;
                C += 1;
                max_flow += path_flow;
                A += 1;
            }
            return max_flow;
        }
        int[,] graph = new int[6, 6];
        public void crearGrafo()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {

                }
            }
        }

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
                int destVertex = random.Next(0, size - 1);
                while (isConnected(graph, i, destVertex))
                    destVertex = random.Next(0, graph.Length - 1);
                capacity = random.Next(1, graph.Length);
                addEdge(graph, i, destVertex, capacity);               
            }
        }

        public static void mediumConnected(List<Edge>[] graph)
        {
            Random random = new Random();
            for (int i = 0; i < graph.Length; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(j != i)
                    {
                        if (i <= 3)
                            addEdge(graph, i, graph.Length / 2 - (j-1), random.Next(1, graph.Length));
                        else
                            addEdge(graph, i, j, random.Next(1, graph.Length));
                    }
                    else
                        addEdge(graph, i, j+1, random.Next(1, graph.Length));
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

        public static bool isConnected(List<Edge>[] graph, int pos, int v2)
        {           
            foreach (Edge e in graph[pos])
            {
                if (e.t == v2 || pos == v2)
                    return true;                    
            }
            return false;
        }

        public static void addEdge(List<Edge>[] graph, int s, int t, int cap)
        {
            Console.WriteLine("graph[t].Count(): " + graph[t].Count());
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
            List<Edge>[] graph = createGraph(10);
            //Random random = new Random();
            //addEdge(graph, 0, 1, 3);
            //addEdge(graph, 0, 2, 2);
            //addEdge(graph, 1, 2, 2); ;
            //Console.WriteLine(4 == maxFlow(graph, 0, 2));
            //Console.WriteLine(maxFlow(graph, 0, 3));
            minimunConnected(graph);
            //Console.WriteLine(vertexConnections);
            int count = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                foreach (Edge e in graph[i])
                {
                    if(e.cap != 0)
                    {
                        count++;
                        Console.WriteLine(count);
                        Console.WriteLine("VALOR VERTICE 2: " + e.t);
                        Console.WriteLine("VALOR VERTICE 1: " + e.rev);
                        Console.WriteLine("VALOR CAPACITY: " + e.cap);
                        Console.WriteLine("VALOR FLOW: " + e.f);
                        Console.WriteLine("---------------------");
                    }
                }
            }
            printMaxFlow(graph);
            Console.ReadKey();
        }

        static void printMaxFlow(List<Edge>[] graph)
        {
            Console.WriteLine("|||||| MAXIMUN FLOWS ||||||");
            Console.WriteLine(maxFlow(graph, 0, 1));
            Console.WriteLine(maxFlow(graph, 0, 2));
            Console.WriteLine(maxFlow(graph, 0, 3));
            Console.WriteLine(maxFlow(graph, 0, 4));
            Console.WriteLine(maxFlow(graph, 0, 5));
            Console.WriteLine(maxFlow(graph, 0, 6));
            Console.WriteLine(maxFlow(graph, 0, 7));
            Console.WriteLine(maxFlow(graph, 0, 8));
            Console.WriteLine(maxFlow(graph, 0, 9));
            //for (int i = 0; i < graph.Length; i++)
            //{
            //    for (int j = 0; j < graph.Length; j++) {
            //        Console.WriteLine("IN LOOP");
            //        Console.WriteLine(maxFlow(graph, i, j));
            //    }
            //}           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace DinicsMaximization
{
    class Program
    {
        static int AF = 0;
        static int CF = 0;


        // FordFulkerson
        public static Boolean bfs(List<Edge>[] graph, int s, int t, int[] parent)
        {
            Boolean[] visited = new Boolean[graph.Count()];
            for (int i = 0; i < graph.Count(); ++i)
            {
                visited[i] = false;
                AF += 2;
                CF += 1;
            }
            AF += 1;
            CF += 1;

            LinkedList<int> queue = new LinkedList<int>();
            queue.AddFirst(s);
            visited[s] = true;
            parent[s] = -1;
            AF += 2;

            while (queue.Count != 0)
            {
                CF += 1;
                int u = queue.First();
                AF += 1;
                queue.Remove(u);

                for (int v = 0; v < graph.Count(); v++)
                {
                    CF += 1;
                    AF += 1;
                    List<Edge> list = graph[u];
                    if (visited[v] == false && list[v].cap > 0)
                    {
                        CF += 2;
                        queue.AddFirst(v);
                        parent[v] = u;
                        visited[v] = true;
                        AF += 2;
                    }
                }
                CF += 1;
                AF += 1;
            }
            CF += 1;
            return (visited[t] == true);

        }

        // FordFulkerson
        public static int fordFulkerson(List<Edge>[] graph2, int s, int t)
        {
            List<Edge>[] graph = copyGraph(graph2, graph2.Length);
            int u, v;

            int[] parent = new int[graph.Count()];

            int max_flow = 0;
            AF += 1;
            while (bfs(graph, s, t, parent))
            {
                int path_flow = int.MaxValue;
                AF += 1;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    List<Edge> list = graph[u];
                    path_flow = Math.Min(path_flow, list[v].cap);
                    AF += 4;
                    CF += 1;
                }
                AF += 1;
                CF += 1;

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    List<Edge> list = graph[u];
                    List<Edge> list2 = graph[v];
                    list[v].cap -= path_flow;
                    list[u].cap += path_flow;
                    AF += 5;
                    CF += 1;
                }
                AF += 1;
                CF += 1;
                max_flow += path_flow;
                AF += 1;
            }
            return max_flow;
        }

//**********************************************************************************************************

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
                if((i + 1) != graph.Length)
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
            Console.WriteLine("graph[s].Count() - 1: " + (graph[s].Count() - 1));
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
            List<Edge>[] graph2 = copyGraph(graph, graph.Length);
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
                        //Console.WriteLine("E.REV" + graph[e.t].ElementAt(e.rev));
                        graph[e.t].ElementAt(0).f -= df;
                        return df;
                    }
                }
            }
            return 0;
        }
        public static  List<Edge>[] copyGraph(List<Edge>[] graph, int nodes) {
            List<Edge>[] graph2 = createGraph(nodes);
            for (int i=0;i< graph.Count();i++) {
                foreach (Edge edge in graph[i]) {
                    graph2[i].Add(edge);
                }
            }
            return graph2;
        }

        static void Main(string[] args)
        { 
            /**
             Escenarios para tomar en cuenta:
                - Conexión mínima
                - Conexión fuertemente conexa
                - Conexión con 3 arcos por vértice
             */
            List<Edge>[] graphD1 = createGraph(10);
            minimunConnected(graphD1);
            List<Edge>[] graphD2 = createGraph(10);
            mediumConnected(graphD2);
            List<Edge>[] graphD3 = createGraph(10);
            stronglyConnected(graphD3);
            //Random random = new Random();
            //addEdge(graph, 0, 1, 3);
            //addEdge(graph, 0, 2, 2);
            //addEdge(graph, 1, 2, 2); ;
            //Console.WriteLine(4 == maxFlow(graph, 0, 2));
            //Console.WriteLine(maxFlow(graph, 0, 3));

            //Grafos para el metodo de Ford-Fulkerson
            List<Edge>[] graphF1 = copyGraph(graphD1, 50);
            List<Edge>[] graphF2 = copyGraph(graphD2, 10);
            List<Edge>[] graphF3 = copyGraph(graphD3, 10);
            //Console.WriteLine(vertexConnections);
            int count = 0;
            for (int i = 0; i < graphD1.Length; i++)
            {
                foreach (Edge e in graphD1[i])
                {
                    if(e.cap != 0)
                    {
                        count++;
                        Console.WriteLine(count);
                        Console.WriteLine("VALOR VERTICE 2: " + e.t);
                        Console.WriteLine("VALOR VERTICE 1: " + e.rev);
                        Console.WriteLine("VALOR CAPACITY: " + e.cap);
                        Console.WriteLine("VALOR FLOW: ");
                        Console.WriteLine("---------------------");
                    }
                }
            }

            printMaxFlow(graphD1);
            //printMaxFlowFordFulkerson(graphF1);
            Console.ReadKey();
        }

        static void printMaxFlow(List<Edge>[] graph)
        {
            //Console.WriteLine("|||||| MAXIMUN FLOWS ||||||");
            Console.WriteLine(maxFlow(graph, 0, graph.Length - 1));
            

        }
        //***************************Luis Carlos ***********************************
        static void printMaxFlowFordFulkerson(List<Edge>[] graph)
        {
            Console.WriteLine("******** MAXIMUN FLOWS WHIT FORD FULKERSON ********");
            for (int i=1;i<graph.Length;i++) {
                Console.WriteLine("Nodo 0 hasta Nodo "+i+" flujo maximo de ("+fordFulkerson(graph, 0, i)+")");
            }
        }
    }
}

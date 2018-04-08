using System;
using System.Collections.Generic;
using System.Linq;

namespace DinicsMaximization
{
    class Program
    {
        static List<Edge>[] graphD1;
        static List<Edge>[] graphD2;
        static List<Edge>[] graphD3;

        static int compD=0;
        static int asigD=0;
        static int lineasD=0;

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
            graph[s].Add(new Edge(t, s, cap));
            graph[t].Add(new Edge(s, t, 0));
        }

        public static void Fill<T>(T[] array, int start, int end, T value)
        {
            lineasD+=1;
            if (array == null){
                compD+=1;
                lineasD+=1;
                throw new ArgumentNullException("array");
            }
            lineasD+=1;
            if (start < 0 || start >= end){
                compD+=2;
                lineasD+=1;
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            compD+=2;
            for (int i = start; i < end; i++){
                array[i] = value;
                compD+=1;
                asigD+=2;
                lineasD+=2;
            }
            lineasD+=1;
            compD+=1;
            asigD+=1;
        }

        public static bool dinicBfs(List<Edge>[] graph, int src, int dest, int[] dist)
        {
            Fill(dist, 0, dist.Count(), -1);
            dist[src] = 0;
            int[] Q = new int[graph.Length];
            int sizeQ = 0;
            Q[sizeQ++] = src;
            asigD+=4;
            lineasD+=5;
            for (int i = 0; i < sizeQ; i++)
            {
                compD+=1;
                int u = Q[i];
                asigD+=2;
                lineasD+=3;
                foreach (Edge e in graph[u])
                {
                    lineasD+=1;
                    if (dist[e.t] < 0 && e.f < e.cap)
                    {
                        compD+=2;
                        dist[e.t] = dist[u] + 1;
                        Q[sizeQ++] = e.t;
                        asigD+=2;
                        lineasD+=3;
                    }
                    compD+=2;
                }
                lineasD+=1;
            }
            asigD+=1;
            compD+=2;
            lineasD+=2;
            return dist[dest] >= 0;
        }

        public static int maxFlow(List<Edge>[] graph, int src, int dest)
        {
            int flow = 0;
            List<Edge>[] graph2 = graph;
            int[] dist = new int[graph2.Length];
            asigD+=3;
            lineasD+=3;
            while (dinicBfs(graph2, src, dest, dist))
            {
                int[] ptr = new int[graph2.Length];
                asigD+=1;
                lineasD+=2;
                while (true)
                {
                    int df = dinicDfs(graph2, ptr, dist, dest, src, Int32.MaxValue);
                    asigD+=1;
                    lineasD+=2;
                    if (df == 0){
                        compD+=1;
                        lineasD+=2;
                        break;
                    }    
                    compD+=1;
                    flow += df;
                    asigD+=1;
                }
                lineasD+=1;
            }
            lineasD+=2;
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

        public static void crearGrafos(int size){  
            graphD1 = createGraph(size);
            minimunConnected(graphD1);
            graphD2 = createGraph(size);
            mediumConnected(graphD2);
            graphD3 = createGraph(size);
            stronglyConnected(graphD3);
        }
        
        static void Main(string[] args)
        {
            
            crearGrafos(10);
            printMaxFlowFordFulkerson(graphD1, graphD2, graphD3,10);
            printMaxFlowDinics(graphD1, graphD2, graphD3,10);
            crearGrafos(50);
            printMaxFlowFordFulkerson(graphD1, graphD2, graphD3,50);
            printMaxFlowDinics(graphD1, graphD2, graphD3,50);
            crearGrafos(100);
            printMaxFlowFordFulkerson(graphD1, graphD2, graphD3,100);
            printMaxFlowDinics(graphD1, graphD2, graphD3,100);
            crearGrafos(500);
            printMaxFlowFordFulkerson(graphD1, graphD2, graphD3,500);
            printMaxFlowDinics(graphD1, graphD2, graphD3,500);
            crearGrafos(1000);
            printMaxFlowFordFulkerson(graphD1, graphD2, graphD3,1000);
            printMaxFlowDinics(graphD1, graphD2, graphD3,1000);
              
            Console.ReadKey();
        }



        static void printMaxFlowDinics(List<Edge>[] graphD1,List<Edge>[] graphD2,List<Edge>[] graphD3,int num)
        {            
            Console.WriteLine("Tamaño del grafo "+num +"\t "+" Metodo DINICS \n" );
            Console.WriteLine("\t--- Conexión minima ---");
            int flujoMaximo1=maxFlow(graphD1, 0, graphD1.Length - 1);
            Console.WriteLine(" Asignaciones: "+asigD+"  Comparaciones: "+compD);            
            Console.WriteLine(" Flujo maximo: " + flujoMaximo1+"  Lineas ejecutadas: "+lineasD);
            Console.WriteLine(" TIEMPO: " +"\n");
            asigD=0; compD=0; lineasD=0;

            Console.WriteLine("\t--- Conexión media ---");
            int flujoMaximo2=maxFlow(graphD2, 0, graphD2.Length - 1);
            Console.WriteLine(" Asignaciones: "+asigD+"  Comparaciones: "+compD);
            Console.WriteLine(" Flujo maximo: " + flujoMaximo2+"  Lineas ejecutadas: "+lineasD);
            Console.WriteLine(" TIEMPO: " +"\n");
            asigD=0; compD=0; lineasD=0;

            Console.WriteLine("\t--- Conexión maxima ---");
            int flujoMaximo3=maxFlow(graphD3, 0, graphD3.Length - 1);
            Console.WriteLine(" Asignaciones: "+asigD+"  Comparaciones: "+compD);
            Console.WriteLine(" Flujo maximo: " + flujoMaximo3+"  Lineas ejecutadas: "+lineasD);
            Console.WriteLine(" TIEMPO: " +"\n");
            asigD=0; compD=0; lineasD=0;
        }

        static void printMaxFlowFordFulkerson(List<Edge>[] graphD1,List<Edge>[] graphD2,List<Edge>[] graphD3,int num){ 
            new FordFulkersonAlgo(graphD1, graphD2, graphD3,num);
        }

    }
}
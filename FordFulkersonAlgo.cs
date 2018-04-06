using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    public class FordFulkersonAlgo
    {

        List<Edge>[] graphD1, graphD2, graphD3;
        DirectedGraph g;
        int af = 0;
        int compF = 0;
        int ffLines = 0;
        /**
         * Main method just for testing
         *
         * @param args will be ignored
         */
        public FordFulkersonAlgo(List<Edge>[] graphD1, List<Edge>[] graphD2, List<Edge>[] graphD3,int num)
        {
            this.graphD1 = graphD1;
            this.graphD2 = graphD2;
            this.graphD3 = graphD3;
            ParseData(num);            
        }

        private void ParseData(int size)
        {
            int source = 0;
            int sink = size - 1;
            Console.WriteLine("************ Tamaño del grafo "+size +"************");
            for (int i = 1; i <= 3; i++)
            {
                g = new DirectedGraph();
                connections(i);
                DateTime time = DateTime.Now;
                Dictionary<EdgeF, int> flow = getMaxFlow(g, source, sink); ffLines++;

                int maxFlow = getFlowSize(flow, g, source); ffLines++;


                DateTime finalT = DateTime.Now;
                TimeSpan total = new TimeSpan(finalT.Ticks - time.Ticks);
                Console.WriteLine("Asignaciones: " + af);
                Console.WriteLine("Comparaciones: " + compF);
                Console.WriteLine("Total de lineas ejecutadas: " + ffLines);
                Console.WriteLine("Flujo maximo: " + maxFlow);
                Console.WriteLine("TIEMPO: " + total.ToString()+"\n");
                ffLines = 0;

            }

        }

        private void connections(int type)
        {

            switch (type)
            {
                case 1:
                    Console.WriteLine("***** Conexión minima *****\n");
                    simpleConexion();
                    break;
                case 2:
                    Console.WriteLine("***** Conexión triple *****\n");
                    tripleConexion();
                    break;
                case 3:
                    Console.WriteLine("***** Conexión maxima *****\n");
                    fullConexion();
                    break;
            }

        }
        // Simple Conexion with Vectors 

        private void simpleConexion()
        {
            for(int i = 0; i < graphD1.Length; i++)
                foreach(Edge e in graphD1[i])
                    if(e.cap != 0)
                        g.addEdge(e.rev, e.t, e.cap);            
        }
        // Triple Conexion with Vectors

        private void tripleConexion()
        {
            for (int i = 0; i < graphD2.Length; i++)
                foreach (Edge e in graphD2[i])
                    if (e.cap != 0)
                        g.addEdge(e.rev, e.t, e.cap);
        }

        private void fullConexion()
        {
            for (int i = 0; i < graphD3.Length; i++)
                foreach (Edge e in graphD3[i])
                    if (e.cap != 0)
                        g.addEdge(e.rev, e.t, e.cap);

        }

        /**
         * This method actually calculates the maximum flow by using the
         * Ford-Fulkerson Algorithm.
         *
         * @param g The directed graph
         * @param source The object identifying the source node of the flow
         * @param sink The object identifying the sink node of the flow
         * @return A HashMap for the edges, giving every edge in the graph a value
         * which shows the part of the edge's capacity that is used by the flow
         */
        // 47 Lineas 
        public Dictionary<EdgeF, int> getMaxFlow(DirectedGraph g, Object source,
                Object sink)
        {
            // reset asignation and comparation
            af = 0;
            compF = 0;

            // The path from source to sink that is found in each iteration
            LinkedList<EdgeF> path; ffLines++;
            // The flow, i.e. the capacity of each edge that is actually used

            //-------------------
            Dictionary<EdgeF, int> flow = new Dictionary<EdgeF, int>(); af++; ffLines++;
            //-------------------

            // Create initial empty flow.

            //-------------------
            compF++; af++;
            foreach (EdgeF e in g.getEdges())
            {
                ffLines++;
                compF++;
                flow.Add(e, 0); af++; ffLines++;
            }
            //-------------------

            // The Algorithm itself
            compF++;
            while ((path = bfs(g, source, sink, flow)) != null)
            {
                ffLines++;
                compF++;
                //-------------------

                // Activating this output will illustrate how the algorithm works
                // System.out.println(path);
                // Find out the flow that can be sent on the found path.

                //-------------------
                int minCapacity = int.MaxValue; af += 2; ffLines++;
                Object lastNode = source; ffLines++;
                //-------------------
                compF++; af++;
                foreach (EdgeF edge in path)
                {
                    ffLines++;
                    compF++;
                    int c; ffLines++;
                    //-------------------
                    // Although the edges are directed they can be used in both
                    // directions if the capacity is partially used, so this if
                    // statement is necessary to find out the edge's actual
                    // direction.

                    //-------------------
                    compF++;
                    ffLines++;
                    if (edge.getStart().Equals(lastNode))
                    {
                        c = edge.getCapacity() - flow[edge]; af++; ffLines++;
                        lastNode = edge.getTarget(); af++; ffLines++;
                    }
                    //-------------------
                    else
                    {
                        ffLines++;
                        c = flow[edge]; af++; ffLines++;
                        lastNode = edge.getStart(); af++; ffLines++;
                    }
                    //-------------------
                    compF++;
                    ffLines++;
                    if (c < minCapacity)
                    {
                        minCapacity = c; af++; ffLines++;
                    }
                }

                // Change flow of all edges of the path by the value calculated
                // above.

                //-------------------
                lastNode = source; af++; ffLines++;
                //-------------------

                compF++; af++;
                foreach (EdgeF edge in path)
                {
                    ffLines++;
                    compF++;
                    // If statement like above
                    //-------------------
                    compF++;
                    ffLines++;
                    if (edge.getStart().Equals(lastNode))
                    {
                        flow[edge] = flow[edge] + minCapacity; af++; ffLines++;
                        lastNode = edge.getTarget(); af++; ffLines++;
                    }
                    else
                    {
                        ffLines++;
                        flow.Add(edge, flow[edge] - minCapacity); af++; ffLines++;
                        lastNode = edge.getStart(); af++; ffLines++;
                    }
                }
            }
            ffLines++;
            return flow;
        }

        /**
         * This method gives the actual flow value by adding all flow values of the
         * out leading edges of the source.
         *
         * @param flow A HashMap of the form like getMaxFlow produces them
         * @param g The directed Graph
         * @param source The object identifying the source node of the flow
         * @return The value of the given flow
         */
        // 10
        public int getFlowSize(Dictionary<EdgeF, int> flow, DirectedGraph g,
                Object source)
        {
            //-------------------

            int maximumFlow = 0; af++; ffLines++;
            Node sourceNode = g.getNode(source); af++; ffLines++;
            //-------------------
            compF++; af++;
            ffLines++;
            for (int i = 0; i < sourceNode.getOutLeadingOrder(); i++)
            {
                ffLines += 2;
                compF++;
                maximumFlow += flow[sourceNode.getEdge(i)]; af++; ffLines++;
            }
            ffLines++;
            return maximumFlow;
            //-------------------
        }

        /**
         * Simple breadth first search in the directed graph
         *
         * @param g The directed Graph
         * @param start The object that identifying the start node of the search
         * @param target The object that identifying the target node of the search
         * @param flow A HashMap of the form like getMaxFlow produces them. If an
         * edge has a value > 0 in it, it will also be used in the opposite
         * direction. Also edges that have a value equal to its capacity will be
         * ignored.
         * @return A list of all edges of the found path in the order in which they
         * are used, null if there is no path. If the start node equals the target
         * node, an empty list is returned.
         */
        public LinkedList<EdgeF> bfs(DirectedGraph g, Object start, Object target,
                Dictionary<EdgeF, int> flow)
        {
            //-------------------

            // The edge by which a node was reached.
            Dictionary<Object, EdgeF> parent = new Dictionary<Object, EdgeF>(); af++; ffLines++;
            // All outer nodes of the current search iteration.
            LinkedList<Object> fringe = new LinkedList<Object>(); af++; ffLines++;
            //-------------------

            // We need to put the start node into those two.
            parent.Add(start, null); af++; ffLines++;
            fringe.AddLast(start); af++; ffLines++;
            // The actual algorithm
            bool stop = false; af++; ffLines++;
            //-------------------

            compF++; ffLines++;
            while (!fringe.Count.Equals(0))
            {
                ffLines++;
                compF++;
                // This variable is needed to prevent the JVM from having a
                // concurrent modification
                //-------------------

                LinkedList<Object> newFringe = new LinkedList<Object>(); af++; ffLines++;

                //-------------------

                // Iterate through all nodes in the fringe.
                compF++;
                ffLines++;
                foreach (Object nodeID in fringe)
                {
                    ffLines++;
                    compF++;
                    Node nodes = g.getNode(nodeID); af++; ffLines++;
                    // Iterate through all the edges of the node.
                    compF++; af++; ffLines++;
                    for (int i = 0; i < nodes.getOutLeadingOrder(); i++)
                    {
                        compF++; af++; ffLines++;
                        EdgeF e = nodes.getEdge(i); af++; ffLines++;
                        //-------------------

                        // Only add the node if the flow can be changed in an out
                        // leading direction. Also break, if the target is reached.
                        //-------------------

                        compF += 3; ffLines += 3;
                        if (e.getStart().Equals(nodeID)
                                && !parent.ContainsKey(e.getTarget())
                                && flow[e] < e.getCapacity())
                        {

                            parent.Add(e.getTarget(), e); af++; ffLines++;
                            compF++;ffLines++;
                            if (e.getTarget().Equals(target))
                            {
                                stop = true; af++; ffLines++;
                                break;
                            }
                            newFringe.AddLast(e.getTarget()); af++; ffLines++;
                        }
                        else if (e.getTarget().Equals(nodeID)
                              && !parent.ContainsKey(e.getStart())
                              && flow[e] > 0)
                        {
                            ffLines++;
                            parent.Add(e.getStart(), e); af++; ffLines++;
                            compF++;
                            ffLines++;
                            if (e.getStart().Equals(target))
                            {
                                stop = true; af++; ffLines++;
                                break;
                            }
                            newFringe.AddLast(e.getStart()); af++; ffLines++;
                        }
                        ffLines++;
                        compF += 3;
                    }
                    ffLines++;
                    compF++;
                    if (stop)
                    {
                        break;
                    }

                }
                compF++; ffLines++;
                if (stop)
                {
                    break;
                }
                // Replace the fringe by the new one.
                fringe = newFringe; af++; ffLines++;
            }
            // Return null, if no path was found.
            compF++; ffLines++;
            if (fringe.Count.Equals(0))
            {
                return null;
            }
            // If a path was found, reconstruct it.
            Object node = target; af++; ffLines++;
            LinkedList<EdgeF> path = new LinkedList<EdgeF>(); af++; ffLines++;
            compF++;
            ffLines++;
            while (!node.Equals(start))
            {
                ffLines++;
                compF++;
                EdgeF e = parent[node]; af++; ffLines++;
                path.AddFirst(e); af++; ffLines++;
                compF++;
                ffLines++;
                if (e.getStart().Equals(node))
                {

                    node = e.getTarget(); af++; ffLines++;
                }
                else
                {
                    ffLines++;
                    node = e.getStart(); af++; ffLines++;
                }
            }
            ffLines++;
            // Return the path.
            return path;
        }

    }
}

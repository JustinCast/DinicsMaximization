using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    /**
* This class is represents a directed graph for the maximum flow calculation
* 
* @author Ruben Beyer
*/
    public class DirectedGraph
    {
        private Dictionary<Object, Node> nodes = new Dictionary<Object, Node>();
        private LinkedList<EdgeF> edges = new LinkedList<EdgeF>();

        /**
         * Use this method to build the graph. It will add an edge to the graph and
         * also its nodes, if necessary. The node identifiers can be any object. Two
         * objects identify the same node, if they are equal according to their
         * equals function.
         * 
         * @param startNodeID
         *            Identifier object of the start node of the edge
         * @param endNodeID
         *            Identifier object of the end node of the edge
         * @param capacity
         *            Capacity of the edge
         */
        public void addEdge(Object startNodeID, Object endNodeID, int capacity)
        {
            Node startNode;
            Node endNode;
            if (!this.nodes.ContainsKey(startNodeID))
            {
                startNode = new Node();
                this.nodes.Add(startNodeID, startNode);
            }
            else
            {
                startNode = this.nodes[startNodeID];
            }
            if (!this.nodes.ContainsKey(endNodeID))
            {
                endNode = new Node();
                this.nodes.Add(endNodeID, endNode);
            }
            else
            {
                endNode = this.nodes[endNodeID];
            }
            EdgeF edge = new EdgeF(startNodeID, endNodeID, capacity);
            startNode.addEdge(edge);
            endNode.addEdge(edge);
            this.edges.AddLast(edge);
        }

        public Node getNode(Object nodeID)
        {
            return this.nodes[nodeID];
        }

        public LinkedList<EdgeF> getEdges()
        {
            return this.edges;
        }
    }
}

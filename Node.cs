using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    /**
* The node class that is used for DirectedGraph
* 
* @author Ruben Beyer
*/
    public class Node
    {

        private List<EdgeF> edges = new List<EdgeF>();

        public void addEdge(EdgeF edge)
        {
            this.edges.Add(edge);
        }

        public EdgeF getEdge(int number)
        {
            if (this.edges.Count <= number)
            {
                return null;
            }
            else
            {
                return this.edges[number];
            }
        }

        public int getOutLeadingOrder()
        {
            return this.edges.Count;
        }

    }
}

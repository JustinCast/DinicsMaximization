using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    /**
* The edge class that is used for DirectedGraph
* 
* @author Ruben Beyer
*/
    public class EdgeF
    {

        private Object target;
        private Object start;
        private int capacity;

        public EdgeF(Object start, Object target, int capacity)
        {
            this.capacity = capacity;
            this.target = target;
            this.start = start;
        }

        public Object getTarget()
        {
            return target;
        }

        public Object getStart()
        {
            return start;
        }

        public int getCapacity()
        {
            return capacity;
        }

        public String toString()
        {
            return this.start + "->" + this.target + "(" + this.capacity + ")";
        }
    }

}

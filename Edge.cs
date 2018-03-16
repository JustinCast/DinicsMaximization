using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicsMaximization
{
    public class Edge
    {
        public int t, rev, cap, f;

        public Edge(int t, int rev, int cap)
        {
            this.t = t;
            this.rev = rev;
            this.cap = cap;
        }
    }
}

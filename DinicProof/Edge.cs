using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicProof
{
    // An edge connects v1 to v2 with a capacity of cap, flow of flow.
    class Edge
    {
        int v1, v2, cap, flow;
        Edge rev;
        Edge(int V1, int V2, int Cap, int Flow)
        {
            v1 = V1;
            v2 = V2;
            cap = Cap;
            flow = Flow;
        }
    }
}

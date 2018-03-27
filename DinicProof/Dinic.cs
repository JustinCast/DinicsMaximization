using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinicProof
{
    public class Dinic
    {

        // Queue for the top level BFS.
        public LinkedList<int> q;

        // Stores the graph.
        private List<Edge>[] adj;
        public int n;

        // s = source, t = sink
        public int s;
        public int t;


        // For BFS.
        public bool[] blocked;
        public int[] dist;

        public static int oo = (int)1E9;

        internal List<Edge>[] Adj { get => adj; set => adj = value; }

        // Constructor.
        public Dinic(int N)
        {

            // s is the source, t is the sink, add these as last two nodes.
            n = N; s = n++; t = n++;

            // Everything else is empty.
            blocked = new bool[n];
            dist = new int[n];
            q = new LinkedList<int>();
            Adj = new List<Edge>[n];
            for (int i = 0; i < n; ++i)
                Adj[i] = new List<Edge>();
        }

        // Just adds an edge and ALSO adds it going backwards.
        public void add(int v1, int v2, int cap, int flow)
        {
            Edge e = new Edge(v1, v2, cap, flow);
            Edge rev = new Edge(v2, v1, 0, 0);
            Adj[v1].Add(rev.rev = e);
            Adj[v2].Add(e.rev = rev);
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

        // Runs other level BFS.
        public bool bfs()
        {

            // Set up BFS
            q.Clear();
            Fill(dist, 0, dist.Count(), -1);
            dist[t] = 0;
            q.AddFirst(t);

            // Go backwards from sink looking for source.
            // We just care to mark distances left to the sink.
            while (q.Count() != 0)
            {
                int node = q.ElementAt(0);
                q.RemoveFirst();
                if (node == s)
                    return true;
                for (Edge e : Adj[node])
                {
                    if (e.rev.cap > e.rev.flow && dist[e.v2] == -1)
                    {
                        dist[e.v2] = dist[node] + 1;
                        q.add(e.v2);
                    }
                }
            }

            // Augmenting paths exist iff we made it back to the source.
            return dist[s] != -1;
        }

        // Runs inner DFS in Dinic's, from node pos with a flow of min.
        public int dfs(int pos, int min)
        {

            // Made it to the sink, we're good, return this as our max flow for the augmenting path.
            if (pos == t)
                return min;
            int flow = 0;

            // Try each edge from here.
            for (Edge e : Adj[pos])
            {
                int cur = 0;

                // If our destination isn't blocked and it's 1 closer to the sink and there's flow, we
                // can go this way.
                if (!blocked[e.v2] && dist[e.v2] == dist[pos] - 1 && e.cap - e.flow > 0)
                {

                    // Recursively run dfs from here - limiting flow based on current and what's left on this edge.
                    cur = dfs(e.v2, Math.min(min - flow, e.cap - e.flow));

                    // Add the flow through this edge and subtract it from the reverse flow.
                    e.flow += cur;
                    e.rev.flow = -e.flow;

                    // Add to the total flow.
                    flow += cur;
                }

                // No more can go through, we're good.
                if (flow == min)
                    return flow;
            }

            // mark if this node is now blocked.
            blocked[pos] = flow != min;

            // This is the flow
            return flow;
        }

        public int flow()
        {
            clear();
            int ret = 0;

            // Run a top level BFS.
            while (bfs())
            {

                // Reset this.
                Arrays.fill(blocked, false);

                // Run multiple DFS's until there is no flow left to push through.
                ret += dfs(s, oo);
            }
            return ret;
        }

        // Just resets flow through all edges to be 0.
        public void clear()
        {
            for (ArrayList<Edge> edges : Adj)
                for (Edge e : edges)
                    e.flow = 0;
        }
    }
}

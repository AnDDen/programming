using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpaceExperiments
{
/*    class FlowNetwork
    {
        public class Edge
        {
            public int capacity;    //пропускная способность
            public int A, B;        //вершины

            public Edge(int A, int B, int capacity)
            {
                this.A = A;
                this.B = B;
                this.capacity = capacity;
            }
        }

        public const int infinity = int.MaxValue / 10;
        
        public int m;               //количество экспериментов
        public int n;               //количество приборов
        public int[] p;             //прибыль от экспериментов
        public int[] c;             //стоимость доставки приборов
        public SortedSet<int>[] r;  //множество приборов, необходимых для эксперимента

        //[0] - исток
        //[n + m + 1] - сток
        //[1]..[n] - приборы
        //[n + 1]..[n + m] - эксперименты

        public int count;           //n + m + 2

        public Edge[] edges;
        
        public void AddEdge(int A, int B, int capacity)
        {
            Array.Resize<Edge>(ref edges, edges.Length + 1);
            edges[edges.Length - 1] = new Edge(A, B, capacity);
        }

        public int GetEdgeNum(int A, int B)
        {
            for (int i = 0; i < edges.Length; i++)
                if ((edges[i].A == A) && (edges[i].B == B)) return i;
            return -1;            
        }

        public Edge GetEdge(int A, int B)
        {
            int i = GetEdgeNum(A, B);
            if (i != -1) return edges[i];
            else return null;
        }

        public int[] GetEdges(int A)
        {
            int[] ed = new int[0];
            for (int i = 0; i < edges.Length; i++)
            {
                Edge e = GetEdge(A, i);
                if (e != null)
                {
                    Array.Resize<int>(ref ed, ed.Length + 1);
                    ed[ed.Length - 1] = e.B;
                }
            }
            return ed;
        }

        public FlowNetwork(int m, int n, int[] p, int[] c, SortedSet<int>[] r)
        {
            this.m = m;
            this.n = n;
            this.p = p;
            this.c = c;
            this.r = r;
            this.count = n + m + 2;

            this.edges = new Edge[0];

            for (int i = 0; i < n; i++)
                AddEdge(0, i + 1, c[i]);

            for (int i = 0; i < m; i++)
                AddEdge(n + 1 + i, count - 1, p[i]);

            for (int i = 1; i <= m; i++)
            {
                foreach (int k in r[i])
                    AddEdge(k, n + i, infinity);
            }
        }

        private int min = 0;
        private bool f = false;
        private int[] minT;

        public int[] FindCut()
        {
            int[] S = new int[count];
            int[] T = new int[count];
            S[0] = 1;
            for (int i = 1; i < count; i++)
                T[i] = 1;
            MinCut(1, ref S, ref T);
            return minT;
        }

        private int CutCapacity(int[] S, int[] T)
        {
            int c = 0;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    if ((S[i] == 1) && (T[j] == 1))
                    {
                        Edge e = GetEdge(i, j);
                        if (e != null) c += e.capacity;
                        if (c > infinity) c = infinity;
                    }
            return c;
        }

        public bool CanConnect(int num, int[] S)
        {
            for (int i = 0; i < count; i++)
                if (i != num)
                {
                    Edge e = GetEdge(i, num);
                    if ((e != null) && (S[i] == 1))
                        return true;
                }
            return false;
        }

        private void MinCut(int k, ref int[] S, ref int[] T)
        {
            if (k > n)
            {
                int c = CutCapacity(S, T);
                if ((c < min) || (!f))
                {
                    min = c;
                    minT = new int[count];
                    T.CopyTo(minT, 0);
                    f = true;
                }
            }
            else
            {
                S[k] = 0; T[k] = 1;
                MinCut(k + 1, ref S, ref T);
                if (CanConnect(k, S))
                {
                    S[k] = 1; T[k] = 0;
                    int[] ed = GetEdges(k);
                    foreach (int e in ed)
                    {
                        S[e] = 1; T[e] = 0;
                    }
                    MinCut(k + 1, ref S, ref T);
                    S[k] = 0; T[k] = 1;
                    foreach (int e in ed)
                    {
                        S[e] = 0; T[e] = 1;
                    }
                }
            }
        }

        public int res;         //прибыль
        public int[] I;         //используемые приборы
        public int[] E;         //проведенные эксперименты

        public void Solve()
        {
            int[] T = FindCut();
            res = 0;
            I = new int[0];
            E = new int[0];
            for (int i = 1; i <= n; i++)
                if (T[i] == 1)
                {
                    Array.Resize<int>(ref I, I.Length + 1);
                    I[I.Length - 1] = i;
                    res -= GetEdge(0, i).capacity;
                }
            for (int i = n + 1; i <= n + m; i++)
                if (T[i] == 1)
                {
                    Array.Resize<int>(ref E, E.Length + 1);
                    E[E.Length - 1] = i - n;
                    res += GetEdge(i, count - 1).capacity;
                }
        }
    } */

    class FlowNetwork
    {
        public class Edge
        {
            public int capacity;    //пропускная способность
            public int num;         //смежная вершина

            public Edge(int num, int capacity)
            {
                this.num = num;
                this.capacity = capacity;
            }
        }

        public class Node
        {
            public Edge[] edges = new Edge[0];

            public void AddEdge(int num, int capacity)
            {
                Array.Resize<Edge>(ref edges, edges.Length + 1);
                edges[edges.Length - 1] = new Edge(num, capacity);
            }

            public int FindEdge(int num)
            {
                int L = edges.Length;
                for (int i = 0; i < L; i++)
                    if (edges[i].num == num) return i;
                return -1;
            }
        }

        public const int infinity = int.MaxValue / 10;

        public int m;               //количество экспериментов
        public int n;               //количество приборов
        public int[] p;             //прибыль от экспериментов
        public int[] c;             //стоимость доставки приборов
        public SortedSet<int>[] r;  //множество приборов, необходимых для эксперимента

        public int count;           //n + m + 2

        public Node[] nodes;
        //[0] - исток
        //[n + m + 1] - сток
        //[1]..[n] - приборы
        //[n + 1]..[n + m] - эксперименты

        public int[] GetEdges(int A)
        {
            int L = nodes[A].edges.Length;
            int[] ed = new int[L];
            for (int i = 0; i < L; i++)
                ed[i] = nodes[A].edges[i].num;
            return ed;
        }

        public FlowNetwork(int m, int n, int[] p, int[] c, SortedSet<int>[] r)
        {
            this.m = m;
            this.n = n;
            this.p = p;
            this.c = c;
            this.r = r;
            this.count = n + m + 2;

            this.nodes = new Node[count];
            for (int i = 0; i < count; i++)
                nodes[i] = new Node();

            for (int i = 0; i < n; i++)
                nodes[0].AddEdge(i + 1, c[i]);
            

            for (int i = 0; i < m; i++)
                nodes[n + 1 + i].AddEdge(count - 1, p[i]);

            for (int i = 1; i <= m; i++)
            {
                foreach (int k in r[i])
                    nodes[k].AddEdge(n + i, infinity);
            }
        }

        private int min = 0;
        private bool f = false;
        private int[] minT;

        public int[] FindCut() //поиск минимального разреза
        {
            int[] S = new int[count];
            int[] T = new int[count];
            S[0] = 1;
            for (int i = 1; i < count; i++)
                T[i] = 1;
            MinCut(1, ref S, ref T);
            return minT;
        }

        private int CutCapacity(int[] S, int[] T) //пропусная способность разрыва, делящего сеть на S и T
        {
            int c = 0;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    if ((S[i] == 1) && (T[j] == 1))
                    {
                        int e = nodes[i].FindEdge(j);
                        if (e != -1) c += nodes[i].edges[e].capacity;
                        if (c > infinity) c = infinity;
                    }
            return c;
        }

        public bool CanConnect(int num, int[] S) //можно ли добавить вершину c номером num к подмножеству вершин S
        {
            for (int i = 0; i < count; i++)
                if (i != num)
                {
                    int e = e = nodes[i].FindEdge(num);
                    if ((e != -1) && (S[i] == 1))
                        return true;
                }
            return false;
        }

        private void MinCut(int k, ref int[] S, ref int[] T) //рекурсивная процедура для поиска минимального разреза
        {
            if (k > n)
            {
                int c = CutCapacity(S, T);
                if ((c < min) || (!f))
                {
                    min = c;
                    minT = new int[count];
                    T.CopyTo(minT, 0);
                    f = true;
                }
            }
            else
            {
                S[k] = 0; T[k] = 1;
                MinCut(k + 1, ref S, ref T);
                if (CanConnect(k, S))
                {
                    S[k] = 1; T[k] = 0;
                    int[] ed = GetEdges(k);
                    foreach (int e in ed)
                    {
                        S[e] = 1; T[e] = 0;
                    }
                    MinCut(k + 1, ref S, ref T);
                    S[k] = 0; T[k] = 1;
                    foreach (int e in ed)
                    {
                        S[e] = 0; T[e] = 1;
                    }
                }
            }
        }

        public int res;         //прибыль
        public int[] I;         //используемые приборы
        public int[] E;         //проведенные эксперименты

        public void Solve()
        {
            int[] T = FindCut();
            res = 0;
            I = new int[0];
            E = new int[0];
            for (int i = 1; i <= n; i++)
                if (T[i] == 1)
                {
                    Array.Resize<int>(ref I, I.Length + 1);
                    I[I.Length - 1] = i;
                    res -= nodes[0].edges[nodes[0].FindEdge(i)].capacity;
                }
            for (int i = n + 1; i <= n + m; i++)
                if (T[i] == 1)
                {
                    Array.Resize<int>(ref E, E.Length + 1);
                    E[E.Length - 1] = i - n;
                    res += nodes[i].edges[nodes[i].FindEdge(count - 1)].capacity;
                }
        }

        public void Write(string path) //запись в файл для графвиза
        {
            FileStream f = new FileStream(path, FileMode.Create);
            StreamWriter w = new StreamWriter(f);

            w.WriteLine("digraph G {");

            for (int k = 0; k < nodes.Length; k++)
            {
                int L = nodes[k].edges.Length;
                if (minT[k] == 1)
                    w.WriteLine(Convert.ToString(k) + "[color = yellow, style = filled];");
                if (k == 0)
                    w.WriteLine(Convert.ToString(k) + "[label = \"Исток\"];");
                if ((k >= 1) && (k <= n))
                    w.WriteLine(Convert.ToString(k) + "[label = \"I" + Convert.ToString(k) + "\"];");
                if ((k >= n + 1) && (k < count - 1))
                    w.WriteLine(Convert.ToString(k) + "[label = \"E" + Convert.ToString(k-n) + "\"];");
                if (k == count - 1)
                    w.WriteLine(Convert.ToString(k) + "[label = \"Cток\"];");
                if (L != 0)
                    for (int i = 0; i < L; i++)
                    {
                        string s = (nodes[k].edges[i].capacity == infinity) ? "∞" : Convert.ToString(nodes[k].edges[i].capacity);
                        w.WriteLine(Convert.ToString(k) + " -> " + Convert.ToString(nodes[k].edges[i].num) +
                            " [label = \"" + s + "\"]");
                    }
            }

            w.WriteLine("}");
            w.Close();
            f.Close();
        }
    }
}

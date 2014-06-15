using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.IO;

namespace VK_CSF
{
    public class VK
    {        
        public class Node
        {
            public string id;
            public string fname;
            public string lname;

            public bool visit;
            public int[] edges;

            public Node(string id, string fname, string lname)
            {
                this.id = id;
                this.fname = fname;
                this.lname = lname;

                this.visit = false;
                this.edges = new int[0];
            }

            public void AddEdge(int num)
            {
                Array.Resize<int>(ref edges, edges.Length + 1);
                edges[edges.Length - 1] = num;
            }
        }

        private Node[] nodes;

        public VK() //конструктор
        {
            this.nodes = new Node[0];
        }

        //======== Выкачивание данных и формирование графа ========

        public void GetPeople() //добавить участников группы курса в граф
        {        
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load("temp/students.xml");
            }
            catch
            {
                WebClient client = new WebClient();
                client.DownloadFile("https://api.vk.com/method/groups.getMembers.xml?group_id=csf2013&fields=first_name,last_name", "temp/students.xml");
                xml.Load("temp/students.xml");
            }

            Console.WriteLine("File temp/students.xml has been loaded");

            foreach (XmlNode p in xml.SelectNodes("/response/users/user"))
            {
                string id = p.SelectSingleNode("uid").InnerText;
                string fname = p.SelectSingleNode("first_name").InnerText;
                string lname = p.SelectSingleNode("last_name").InnerText;
                
                Array.Resize<Node>(ref nodes, nodes.Length + 1);
                nodes[nodes.Length - 1] = new Node(id, fname, lname);
            }
        }

        public string[] GetFriends(string id) //друзья пользователя id
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load("temp/friends_" + id + ".xml");
            }
            catch
            {
                WebClient client = new WebClient();
                client.DownloadFile("http://api.vk.com/method/friends.get.xml?user_id=" + id, "temp/friends_" + id + ".xml");
                xml.Load("temp/friends_" + id + ".xml");
            }
            string[] list = new string[0];
            foreach (XmlNode p in xml.SelectNodes("/response/uid"))
            {
                Array.Resize<string>(ref list, list.Length + 1);
                list[list.Length - 1] = p.InnerText;
            }
            return list;
        } 

        public void SetAllConnections() //установить все связи
        {
            for (int i = 0; i < nodes.Length; i++)
                SetСonnections(i);
        }

        public void SetСonnections(int num) //установить связи узла num
        {
            string id = nodes[num].id;
            string[] friends = GetFriends(id);
            foreach (string f in friends)
            {
                for (int i = 0; i < nodes.Length; i++)
                    if (nodes[i].id == f)
                    {
                        nodes[num].AddEdge(i);
                        break;
                    }
            }
            Console.WriteLine("Connections for {0} have been set", nodes[num].id);
        }

        public void Write(string path)
        {
            FileStream f = new FileStream(path, FileMode.Create);
            StreamWriter w = new StreamWriter(f);

            w.WriteLine("digraph G {");

            for (int i = 0; i < nodes.Length; i++)
            {
                w.WriteLine(i + " [label = \"id = " + nodes[i].id + "\r\n fname = " + nodes[i].fname + "\r\n lname = " + nodes[i].lname + "\", shape=box];");
                for (int j = 0; j < nodes[i].edges.Length; j++)
                    w.WriteLine("{0} -> {1}", i, nodes[i].edges[j]);
            }

            w.WriteLine("}");
            w.Close();
            f.Close();

            Console.WriteLine("Graph saved to " + path);
        } //печать графа

        //======== Изолированные группы ========
        //Определение количества компонент связности, в которых больше 1 вершины.

        private int[] mark;

        public void Component(int x, int count)
        {
            mark[x] = count;
            foreach (int v in nodes[x].edges)
                if (mark[v] == 0)
                    Component(v, count);
        }

        public int SetComponents() //выделение компонент связности
        {
            mark = new int[nodes.Length];
            int count = 0;
            for (int i = 0; i < nodes.Length; i++)
                if ((mark[i] == 0) && (nodes[i].edges.Length != 0))
                {
                    count++;
                    Component(i, count);
                }
            return count;
        }

        public void IsolatedGroups()
        {
            int count = SetComponents();
            if (count == 1)
                Console.WriteLine("none");
            else
            {
                Console.WriteLine("{0} isolated groups: ", count);
                string[] s = new string[count];
                for (int i = 0; i < count; i++) s[i] = "";
                for (int i = 0; i < nodes.Length; i++)
                    if (mark[i] != 0)
                        if (s[mark[i] - 1] == "")
                            s[mark[i] - 1] = nodes[i].fname + " " + nodes[i].lname;
                        else
                            s[mark[i] - 1] = s[mark[i] - 1] + ", " + nodes[i].fname + " " + nodes[i].lname;
                for (int i = 0; i < count; i++)
                    Console.WriteLine("{0}: {1}", i + 1, s[i]);
            }
        }

        //======== Самая длинная цепочка ========

        public void VisitFalse()
        {
            for (int i = 0; i < nodes.Length; i++)
                nodes[i].visit = false;
        }

        const int infinity = int.MaxValue / 10;
        int[] d;
        int[,] matr;

        public void SetMatr()   //построение матрицы смежности
        {
            int n = nodes.Length;
            matr = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                Node p = nodes[i];
                for (int j = 0; j < p.edges.Length; j++)
                {
                    if (p.edges[j] > i)
                        matr[i, p.edges[j]] = 1;
                    else
                        matr[p.edges[j], i] = 1;
                }
            }
        }       

        public bool AllVisited()
        {
            for (int i = 0; i < nodes.Length; i++)
                if (!nodes[i].visit) return false;
            return true;
        }

        public int DijkstraMax(int a)
        {
            VisitFalse();
            int N = nodes.Length;
            for (int i = 0; i < N; i++)
                d[i] = infinity;
            d[a] = 0;
            while (!AllVisited())
            {
                int v = -1;
                for (int j = 0; j < N; j++)
                    if (!nodes[j].visit && ((v == -1) || (d[j] < d[v]))) v = j;
                nodes[v].visit = true;
                int L = nodes[v].edges.Length;
                for (int j = 0; j < L; j++)
                {
                    int u = nodes[v].edges[j];
                    if (!nodes[u].visit)
                        if (d[u] > d[v] + matr[v, u])
                            d[u] = d[v] + matr[v, u];
                }
            }
            int max = 0;
            for (int i = 1; i < N; i++)
                if ((d[i] > max) && (d[i] < infinity) && (matr[a, i] != 1)) max = d[i];
            return max;
        }

        public void MaxLength()
        {
            int N = nodes.Length;
            d = new int[N];
            SetMatr();
            int max = 0;
            int m;
            for (int a = 0; a < N; a++)
            {
                m = DijkstraMax(a);
                if (m > max) max = m;
            }
            Console.WriteLine("Maximum length = {0}", max);
        }
    }
}

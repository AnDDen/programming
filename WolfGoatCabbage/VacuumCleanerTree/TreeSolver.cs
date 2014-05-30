using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VacuumCleanerTree
{
    public class Inf //класс для состояния мира
    {
        const int n = 3;

        int[] b1, b2 = new int[n]; //берега
        int l; //лодка

        //0 - коза, 1 - волк, 2 - капуста. 1 не может быть с 0, а 0 не может быть с 2.

        public string name; //название действия, с помощью которого получено текущее состояние мира

        public Inf(int[] b1, int[] b2, int l, string name)
        {
            this.b1 = new int[n];
            this.b2 = new int[n];
            for (int i = 0; i < n; i++)
            {
                this.b1[i] = b1[i];
                this.b2[i] = b2[i];
            }
            this.l = l;
            this.name = name;
        }

        public int Near()
        {
            return 3 - b2[0] - b2[1] - b2[2];
        }

        public bool IsEqual(Inf p) //проверка того, что текущее положение совпадает с положением p
        {
            bool f = (l == p.l);
            for (int i = 0; i < n; i++)
                if ((b1[i] != p.b1[i]) || (b2[i] != p.b2[i])) f = false;
            return f;
        }

        bool Check()
        {
            if (((b1[0] == 1) && (b1[1] == 1) && (b1[2] == 1)) || ((b2[0] == 1) && (b2[1] == 1) && (b2[2] == 1))) return true;
            if (((b1[0] == 1) && ((b1[1] == 1) || (b1[2] == 1))) || ((b2[0] == 1) && ((b2[1] == 1) || (b2[2] == 1)))) return false;
            return true;
        }

        public Inf[] GenerateNext() //создание массива состояний мира, которые можно получить из текущего
        {
            Inf[] list = new Inf[0];

            if (l == -1) //лодка пуста, могу загружать
            {
                //первый берег
                //загружаю 0
                if (b1[0] == 1)
                {
                    b1[0] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 0, "Goat loaded from the first beach");
                    }
                    b1[0] = 1;
                }

                //загружаю 1
                if (b1[1] == 1)
                {
                    b1[1] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 1, "Wolf loaded from the first beach");
                    }
                    b1[1] = 1;
                }

                //загружаю 2
                if (b1[2] == 1)
                {
                    b1[2] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 2, "Cabbage loaded from the first beach");
                    }
                    b1[2] = 1;
                }

                //второй берег
                //загружаю 0
                if (b2[0] == 1)
                {
                    b2[0] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 0, "Goat loaded from the second beach");
                    }
                    b2[0] = 1;
                }

                //загружаю 1
                if (b2[1] == 1)
                {
                    b2[1] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 1, "Wolf loaded from the second beach");
                    }
                    b2[1] = 1;
                }

                //загружаю 2
                if (b2[2] == 1)
                {
                    b2[2] = 0;
                    if (Check())
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = new Inf(b1, b2, 2, "Cabbage loaded from the second beach");
                    }
                    b2[2] = 1;
                }
            }
            else //лодка не пуста, выгрузить с лодки
            {
                //первый берег
                b1[l] = 1;
            //    if (Check())
             //   {
                    Array.Resize(ref list, list.Length + 1);
                    list[list.Length - 1] = new Inf(b1, b2, -1, "Loaded to the first beach");
             //   }
                b1[l] = 0;

                //второй берег
                b2[l] = 1;
            //    if (Check())
             //   {
                    Array.Resize(ref list, list.Length + 1);
                    list[list.Length - 1] = new Inf(b1, b2, -1, "Loaded to the second beach");
             //   }
                b2[l] = 0; 
            }

            return list;
        }

        public string WriteInf() //запись состояния мира
        {
            string s = "[" + name + "]\r\nBeach 1: ";
            if (b1[0] == 1) s += "Goat ";
            if (b1[1] == 1) s += "Wolf ";
            if (b1[2] == 1) s += "Cabbage ";
            s += "\r\n";

            s += "Beach 2: ";
            if (b2[0] == 1) s += "Goat ";
            if (b2[1] == 1) s += "Wolf ";
            if (b2[2] == 1) s += "Cabbage ";
            s += "\r\n";

            s += "Boat: ";
            if (l == 0) s += "Goat\r\n";
            else
                if (l == 1) s += "Wolf\r\n";
                else
                    if (l == 2) s += "Cabbage\r\n";
                    else
                        s += "No One\r\n";

            return s;
        }
    }

    public class TNode //класс узла дерева
    {
        public Inf data; //информационная часть - состояние мира
        
        public TNode[] childs; //потомки узла
        public TNode parent; //родитель узла
        public int level; //уровень, на котором находится текущий узел

        public int num;

        public TNode(Inf data, TNode parent) //конструктор
        {
            this.parent = parent;
            //вычисление уровня текущего узла
            if (parent != null) this.level = parent.level + 1;
            else this.level = 0;
            this.childs = new TNode[0];
            this.data = data;
        }

        public bool IsEqual(TNode p) //проверка на равенство информационных частей текущего узла и узла p
        {
            return data.IsEqual(p.data);
        }

        public bool CheckUnique(TNode q) //проверка на то, что ранее по ветке не было узла, у которого информационная часть совпадает с информационной частью q.
        {
            bool f = true;
            TNode p = this;
            while (p != null)
            {
                f &= !p.IsEqual(q);
                p = p.parent;
            }
            return f;
        }

        public void GenerateNext() //создание потомков узла
        {
            if (data.Near() != 0) //если узел не является искомым, то генерируем новый уровень
            {
                Inf[] list = data.GenerateNext(); //создаем массив состояний, которые можно получить из текущего
                foreach (Inf f in list)
                {
                    TNode p = new TNode(f, this);
                    if (CheckUnique(p)) //если ранее не встречалось такое же состояние
                    {
                        Array.Resize(ref childs, childs.Length + 1);
                        childs[childs.Length - 1] = p;
                    }
                }
            }
        }

        public string WriteNode() //запись состояния мира узла
        {
            return data.WriteInf();
        }

        public void WriteNodeFile(StreamWriter w)
        {
            w.WriteLine(Convert.ToString(num) + "[label = \"" + WriteNode() + "\", shape=box];");
            foreach (TNode x in childs)
            {
                w.WriteLine(Convert.ToString(num) + "->" + Convert.ToString(x.num) + ";");
                x.WriteNodeFile(w);
            }
        }
    }

    public class SolverTree
    {
        public TNode start; //начальный узел
        public TNode result;

        public int d; //количество уровней, которое будет генерироваться

        //конструкторы; если d не указано, то d = 5 
        public SolverTree(TNode start)
        {
            this.start = start;
            this.result = null;
            this.d = 5;
        }
        public SolverTree(TNode start, int d)
        {
            this.start = start;
            this.result = null;
            this.d = d;
        }

        public void Generate(TNode p) //генерация d уровней
        {
            p.GenerateNext();
            if (p.level - start.level + 1 < d)
            {
                foreach (TNode q in p.childs)
                    Generate(q);
            }
        }

        private int minN = -1;

        public void FindDepth(TNode p) //поиск в глубину
        {
            //если найден узел, у которого нет потомков и ближе к искомому состоянию, чем текущий result
            if ((p.childs.Length == 0) && ((minN == -1) || ((p.data.Near() <= minN) && (p.level <= result.level))))
            {
                minN = p.data.Near();
                result = p;
            }
            else
            {
                foreach (TNode q in p.childs)
                    FindDepth(q);
            }
        }

        public void Solve() //метод для решения задачи
        {
            Generate(start); //генерация d уровней после start
            FindDepth(start); //поиском в ширину находим узел, состояние мира которого наиболее близкое к искомому
            if (result.data.Near() != 0) //если result - не искомое состояние, то создаем новый экземпляр класса для решения и запускаем для него функцию Solve()
            {
                SolverTree t = new SolverTree(result, d);
                t.Solve();
                result = t.result;
            }
        }

        public string WriteResult() //запись результата в строку
        {
            string s = "";
            if (result != null)
            {
                TNode p = result;
                while (p != null)
                {
                    s = p.WriteNode() + "\n" + s;
                    p = p.parent;
                }
            }
            else s = "No path";
            return s;
        }

        public void WriteTreeResult(string path) //запись дерева в файл
        {
            FileStream f = new FileStream(path, FileMode.Create);
            StreamWriter w = new StreamWriter(f);

            w.WriteLine("digraph G {");

            int num = 1;

            StackQueue<TNode> queue = new StackQueue<TNode>();
            queue.EnQueue(start);
            TNode x;
            while (queue.count != 0)
            {
                x = queue.Pop();
                x.num = num++;
                foreach (TNode q in x.childs)
                    queue.EnQueue(q);
            }

            x = result;
            while (x != null)
            {
                w.WriteLine(Convert.ToString(x.num) + "[color = yellow, style = filled];");
                x = x.parent;
            }

            start.WriteNodeFile(w);
            w.WriteLine("}");
            w.Close();
            f.Close();
        }
    }
}

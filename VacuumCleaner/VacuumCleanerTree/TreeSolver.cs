using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VacuumCleanerTree
{
    public struct Point
    {
        public int x;
        public int y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Inf //класс для состояния мира
    {     
        public const int n = 3, m = 3;
        public int[,] data = new int[n, m]; //массив с расположением мусора
        public Point pos; //положение пылесоса

        public string name; //название действия, с помощью которого получено текущее состояние мира

        public Inf(int[,] data, Point pos, string name) //конструктор
        {
            this.data = data;
            this.pos = pos;
            this.name = name;
        } 

        public int Near() //функция, вычисляющая, насколько близко текущее состояние к искомому (0 - если искомое состояние)
        {
            int result = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (data[i, j] == 1)
                        result += (n * m * 2) + Math.Abs(pos.x - i) + Math.Abs(pos.y - j);
            return result;
        }

        public bool IsEqual (Inf p) //проверка того, что текущее положение совпадает с положением p
        {
            bool f = (pos.x == p.pos.x) && (pos.y == p.pos.y);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (data[i, j] != p.data[i, j]) f = false;
            return f;
        }

        public Inf[] GenerateNext() //создание массива состояний мира, которые можно получить из текущего
        {
            Inf p;
            Inf[] list = new Inf[0];
            if (data[pos.x, pos.y] != 0)
            {
                //Suck
                int[,] data1 = new int[n, m];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        data1[i, j] = data[i, j];
                data1[pos.x, pos.y] = 0;
                p = new Inf(data1, pos, "Suck");
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = p;
            }
            else
            {
                //Up
                Point pos1 = new Point(pos.x - 1, pos.y);
                if (pos1.x < 0) pos1.x++;
                p = new Inf(data, pos1, "Up");
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = p;

                //Down
                pos1 = new Point(pos.x + 1, pos.y);
                if (pos1.x >= n) pos1.x--;
                p = new Inf(data, pos1, "Down");
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = p;

                //Left
                pos1 = new Point(pos.x, pos.y - 1);
                if (pos1.y < 0) pos1.y++;
                p = new Inf(data, pos1, "Left");
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = p;

                //Right
                pos1 = new Point(pos.x, pos.y + 1);
                if (pos1.y >= m) pos1.y--;
                p = new Inf(data, pos1, "Right");
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = p;
            }
            return list;
        }

        public string WriteInf() //запись состояния мира
        {
            string s = "[" + name + "]\r\nTrash:\r\n";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                    s += Convert.ToString(data[i, j]);
                s += "\r\n";
            }
            s += "Cleaner: (" + Convert.ToString(pos.x) + "; " + Convert.ToString(pos.y) + ")\r\n";
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

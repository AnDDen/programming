using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace tree
{
    class RBTree
    {
        public enum NodeColor { RED, BLACK };
        
        public class Node
        {
            public int data;
            public Node parent;
            public Node left;
            public Node right;
            public NodeColor color;
            public Node(int data, Node parent, Node left, Node right)
            {
                this.data = data;
                this.parent = parent;
                this.left = left;
                this.right = right;
                this.color = NodeColor.BLACK;
            }
        }
        
        private Node root;
        Node nil = new Node(0, null, null, null); //лист
        
        public RBTree()
        {
            root = nil;
        }
        
        //Вращения

        public void LeftRorate(Node x)
        {
            Node y = x.right;
            x.right = y.left;
            if (y.left != nil)
                y.left.parent = x;
            y.parent = x.parent;
            if (x.parent == nil)
                root = y;
            else
                if (x == x.parent.left)
                    x.parent.left = y;
                else
                    x.parent.right = y;
            y.left = x;
            x.parent = y;
        }
        public void RightRorate(Node y)
        {
            Node x = y.left;
            y.left = x.right;
            if (x.right != nil)
                x.right.parent = y;
            x.parent = y.parent;
            if (y.parent == nil)
                root = x;
            else
                if (y == y.parent.right)
                    y.parent.right = x;
                else
                    y.parent.left = x;
            x.right = y;
            y.parent = x;
        }
        
        //Вставка в дерево

        private Node SimpleInsert(int data) //обычная вставка в бинарное дерево
        {
            if (root == nil)
            {
                root = new Node(data, nil, nil, nil);
                return root;
            }
            else
            {
                Node p = root;
                bool ok = false;
                while (!ok)
                {
                    if (data > p.data)
                        if (p.right == nil)
                        {
                            p.right = new Node(data, p, nil, nil);
                            ok = true;
                            return p.right;
                        }
                        else p = p.right;
                    else
                        if (data < p.data)
                            if (p.left == nil)
                            {
                                p.left = new Node(data, p, nil, nil);
                                ok = true;
                                return p.left;
                            }
                            else p = p.left;
                        else
                        {
                            ok = true;
                            return p;
                        }
                }
                return nil;
            }
        }
        public void Insert(int data) //вставка с восстановление RB-свойств
        {
            Node x = SimpleInsert(data);
            x.color = NodeColor.RED;
            while ((x != root) && (x.parent.color == NodeColor.RED))
            {
                if (x.parent == x.parent.parent.left) //если родительская ветка левая
                {
                    Node y = x.parent.parent.right; //нахождение дяди вершины x
                    if (y.color == NodeColor.RED)
                    {
                        x.parent.color = NodeColor.BLACK;
                        y.color = NodeColor.BLACK;
                        x.parent.parent.color = NodeColor.RED;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.right) //если x - правая ветка   
                        {
                            x = x.parent;
                            LeftRorate(x);
                        }
                        x.parent.color = NodeColor.BLACK;
                        x.parent.parent.color = NodeColor.RED;
                        RightRorate(x.parent.parent);
                    }
                }
                else //если родительская ветка правая
                {
                    Node y = x.parent.parent.left;
                    if ((y != nil) && (y.color == NodeColor.RED))
                    {
                        x.parent.color = NodeColor.BLACK;
                        y.color = NodeColor.BLACK;
                        x.parent.parent.color = NodeColor.RED;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.left) //если x - левая ветка   
                        {
                            x = x.parent;
                            RightRorate(x);
                        }
                        x.parent.color = NodeColor.BLACK;
                        x.parent.parent.color = NodeColor.RED;
                        LeftRorate(x.parent.parent);
                    }
                }
            }
            root.color = NodeColor.BLACK;
        }
        
        //Печать дерева

        private void WriteNode(Node p, StreamWriter w)
        {  
            if (p != null)
            {
                if (p.color == NodeColor.RED)
                    w.WriteLine(Convert.ToString(p.data) + " [color = red, style = filled, fontcolor = white];");
                else
                    w.WriteLine(Convert.ToString(p.data) + " [color = black, style = filled, fontcolor = white];");
                if (p.left != nil)
                {
                    w.WriteLine(Convert.ToString(p.data) + "->" + Convert.ToString(p.left.data) + ";");
                    WriteNode(p.left, w);
                }
                if (p.right != nil)
                {
                    w.WriteLine(Convert.ToString(p.data) + "->" + Convert.ToString(p.right.data) + ";");
                    WriteNode(p.right, w);
                }
            }
        }
        public void WriteTree(string path)
        {
            FileStream f = new FileStream(path, FileMode.Create);
            StreamWriter w = new StreamWriter(f);
            w.WriteLine("digraph G {");
            WriteNode(root, w);
            w.WriteLine("}");
            w.Close();
            f.Close();
        }

        //Крайний левый и крайний правый потомки x

        public Node TreeMinimum(Node x)
        {
            while (x.left != nil)
                x = x.left;
            return x;
        }
        public Node TreeMaximum(Node x)
        {
            while (x.right != nil)
                x = x.right;
            return x;
        }

        //Удаление элемента из дерева

        private void RBTransplant(Node x, Node y)
        {
            if (x.parent == nil)
                root = y;
            else
                if (x == x.parent.left)
                    x.parent.left = y;
                else x.parent.right = y;
            y.parent = x.parent;
        }
        public void Delete(Node z)
        {
            Node y = z;
            Node x;
            NodeColor ycolor = y.color;
            if (z.left == nil)
            {
                x = z.right;
                RBTransplant(z, z.right);
            }
            else
                if (z.right == nil)
                {
                    x = z.left;
                    RBTransplant(z, z.left);
                }
                else
                {
                    y = TreeMinimum(z.right);
                    ycolor = y.color;
                    x = y.right;
                    if (y.parent == z)
                        x.parent = y;
                    else
                    {
                        RBTransplant(y, y.right);
                        y.right = z.right;
                        y.right.parent = y;
                    }
                    RBTransplant(z, y);
                    y.left = z.left;
                    y.left.parent = y;
                    y.color = z.color;
                }
            if (ycolor == NodeColor.BLACK)
                DeleteFix(x);
        }
        private void DeleteFix(Node x) //Восстановление RB-свойств
        {
            while ((x != root) && (x != nil) && (x.color == NodeColor.BLACK))
            {
                if (x == x.parent.left)
                {
                    Node w = x.parent.right;
                    if (w.color == NodeColor.RED)
                    {
                        w.color = NodeColor.BLACK;
                        x.parent.color = NodeColor.RED;
                        LeftRorate(x.parent);
                        w = x.parent.right;
                    }
                    if ((w.left.color == NodeColor.BLACK) && (w.right.color == NodeColor.BLACK))
                    {
                        w.color = NodeColor.RED;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.right.color == NodeColor.BLACK)
                        {
                            w.left.color = NodeColor.BLACK;
                            w.color = NodeColor.RED;
                            RightRorate(w);
                            w = x.parent.right;
                        }
                        w.color = x.parent.color;
                        x.parent.color = NodeColor.BLACK;
                        w.right.color = NodeColor.BLACK;
                        LeftRorate(x.parent);
                        x = root;
                    }
                }
                else
                {
                    Node w = x.parent.right;
                    if (w.color == NodeColor.RED)
                    {
                        w.color = NodeColor.BLACK;
                        x.parent.color = NodeColor.RED;
                        RightRorate(x.parent);
                        w = x.parent.left;
                    }
                    if ((w.left.color == NodeColor.BLACK) && (w.right.color == NodeColor.BLACK))
                    {
                        w.color = NodeColor.RED;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.left.color == NodeColor.BLACK)
                        {
                            w.right.color = NodeColor.BLACK;
                            w.color = NodeColor.RED;
                            LeftRorate(w);
                            w = x.parent.left;
                        }
                        w.color = x.parent.color;
                        x.parent.color = NodeColor.BLACK;
                        w.left.color = NodeColor.BLACK;
                        RightRorate(x.parent);
                        x = root;
                    }
                }
            }
            x.color = NodeColor.BLACK;
        }
        public void Delete(int data) //Удаление по значению
        {
            Node p = Find(data);
            if (p != nil) Delete(p);
        } 

        //Поиск элемента в дереве

        public Node Find(int data)
        {
            if (root != nil)
                return FindNode(root, data);
            else
                return nil;
        }
        private Node FindNode(Node p, int data)
        {
            if (p.data == data)
                return p;
            else
            {
                Node result = nil;
                if (p.left != nil)
                    result = FindNode(p.left, data);
                if ((result == nil) && (p.right != nil))
                    result = FindNode(p.right, data);
                return result;
            }
        }

        //Глубина дерева

        private int Depth(Node r)
        {
            if (r != nil)
                return 1 + Math.Max(Depth(r.left), Depth(r.right));
            return 0;
        }
        public int TreeDepth()
        {
            return Depth(root);
        }
    }
}

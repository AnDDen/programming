using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dictionary
{
    public class Dictionary<Key, Value> where Key : IComparable
    {
        private class Node
        {
            public Value data;
            public Key key;
            public Node parent;
            public Node left;
            public Node right;
            public Node(Value data, Key key, Node parent, Node left, Node right)
            {
                this.data = data;
                this.key = key;
                this.parent = parent;
                this.left = left;
                this.right = right;
            }
        }
        
        private Node root;
        
        public Dictionary()
        {
            root = null;
        }
        
        //Вставка пары (ключ, значение)
        public void Add(Key key, Value value)
        {
            if (root == null)
                root = new Node(value, key, null, null, null);
            else
            {
                Node p = root;
                bool ok = false;
                while (!ok)
                {
                    int t = key.CompareTo(p.key);
                    if (t > 0)
                        if (p.right == null)
                        {
                            p.right = new Node(value, key, p, null, null);
                            ok = true;
                        }
                        else p = p.right;
                    else
                        if (t < 0)
                            if (p.left == null)
                            {
                                p.left = new Node(value, key, p, null, null);
                                ok = true;
                            }
                            else p = p.left;
                        else
                        {
                            p.data = value;
                            ok = true;
                        }
                }
            }
        }

        //Удаление по ключу
        Node q;
        private void Delete(ref Node r)
        { 
            if (r.right != null)
                Delete(ref r.right);
            else 
            {
                q.data = r.data;
                q.key = r.key;
                q = r; 
                r = r.left;
            }
        }
        private void DeleteNode(Key key, ref Node p)
        {
            if (p != null)
            {
                int t = key.CompareTo(p.key);
                if (t < 0)
                    DeleteNode(key, ref p.left);
                else
                    if (t > 0)
                        DeleteNode(key, ref p.right);
                    else
                    {
                        q = p;
                        if (q.right == null)
                            p = q.left;
                        else
                            if (q.left == null)
                                p = q.right;
                            else
                                Delete(ref q.left);
                    }
            }
        }
        public void Remove(Key key)
        {
            DeleteNode(key, ref root);
        }

        //Очистка словаря
        public void Clear()
        {
            root = null;
        }

        //Поиск значения о ключу
        public Value GetValue(Key key)
        {
            return Find(root, key);
        }
        private Value Find(Node p, Key key)
        {
            if (p == null)
                throw new IndexOutOfRangeException();
            if (key.Equals(p.key))
                return p.data;
            else
            {
                int t = key.CompareTo(p.key);
                if (t > 0)
                    return Find(p.right, key);
                else
                    return Find(p.left, key);
            }
        }

        //Обращение к элементам словаря через []
        public Value this[Key key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                Add(key, value);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brackets
{
    class List
    {
        public class Node
        {
            public char inf;
            public Node next;
            public Node prev;
            public Node(char inf, Node next, Node prev)
            {
                this.inf = inf;
                this.next = next;
                this.prev = prev;
            }
        }

        public Node begin;
        public int count;
        public List()
        {
            count = 0;
            begin = null;
        }

        public bool IsEmpty()
        {
            return begin == null;
        }

        public void Add(char inf) //добавить элемент в начало списка
        {
            if (IsEmpty())
                begin = new Node(inf, null, null);
            else
            {
                Node p = new Node(inf, begin, null);
                begin.prev = p;
                begin = p;
            }
            count++;
        }

        public Node GetNode(int index)
        {
            if ((index < count) && (index >= 0))
            {
                Node p = begin;
                for (int i = 0; i < index; i++)
                    p = p.next;
                return p;
            }
            else throw new IndexOutOfRangeException();
        }

        public char GetVal(int index)
        {
            if ((index < count) && (index >= 0))
            {
                Node p = begin;
                for (int i = 0; i < index; i++)
                    p = p.next;
                return p.inf;
            }
            else throw new IndexOutOfRangeException();
        }

        public void Insert(char inf, int index) //вставить элемент в позицию index
        {
            if (index == 0)
                Add(inf);
            else
                if (index == count) //вставка элемента в конец списка
                {
                    Node tmp = GetNode(count - 1);
                    Node p = new Node(inf, null, tmp);
                    tmp.next = p;
                    count++;
                }
                else
                    if ((index > 0) && (index < count))
                    {
                        Node tmp = GetNode(index);
                        Node p = new Node(inf, tmp, tmp.prev);
                        tmp.prev.next = p;
                        tmp.prev = p;
                        count++;
                    }
                    else throw new IndexOutOfRangeException();
        }

        public void Delete(int index) //удалить элемент из позиции index
        {
            if ((index >= 0) && (index < count))
            {
                Node p = GetNode(index);
                if (p.next != null)
                    p.next.prev = p.prev;
                if (p.prev != null)
                    p.prev.next = p.next;
                else
                    begin = p.next;
                count--;
            }
            else throw new IndexOutOfRangeException();
        }
    }
}

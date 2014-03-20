using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brackets
{
    class Node
    {
        public char inf;
        public Node next;
        public Node(char inf, Node next)
        {
            this.inf = inf;
            this.next = next;
        }
    }

    class Stack
    {
        public Node top;
        public Node tail;
        public int count;
        public Stack()
        {
            count = 0;
        }
        public bool IsEmpty()
        {
            return top == null;
        }
        public void PushStack(char inf)
        {
            top = new Node(inf, top);
            count++;
        }

        public void PushQueue(char inf)
        {
            Node t = new Node(inf, null);
            if (IsEmpty())
            {
                top = t;
                tail = t;
            }
            else
            {
                tail.next = t;
                tail = t;
            }
        }

        public char PopStack()
        {
            if (!IsEmpty())
            {
                char res = top.inf;
                top = top.next;
                count--;
                return res;
            }
            else throw new InvalidOperationException();
        }
    }
}

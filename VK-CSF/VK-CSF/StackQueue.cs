using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_CSF
{
    public class StackQueue<Type>
    {
        public class Node
        {
            public Type inf;
            public Node next;
            public Node(Type inf, Node next)
            {
                this.inf = inf;
                this.next = next;
            }
        }

        public Node top;
        public Node tail;
        public int count;
        public StackQueue()
        {
            count = 0;
        }
        public bool IsEmpty()
        {
            return top == null;
        }
        public void PushStack(Type inf)
        {
            top = new Node(inf, top);
            count++;
        }
        public void EnQueue(Type inf)
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
            count++;
        }
        public Type Pop()
        {
            if (!IsEmpty())
            {
                Type res = top.inf;
                top = top.next;
                count--;
                return res;
            }
            else throw new InvalidOperationException();
        }
    }
}

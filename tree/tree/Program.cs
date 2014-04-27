using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace tree
{
    class Program
    {
        static void Main(string[] args)
        {
            RBTree t = new RBTree();
            t.Insert(26);
            t.Insert(17);
            t.Insert(41);
            t.Insert(14);
            t.Insert(21);
            t.Insert(30);
            t.Insert(47);
            t.Insert(10);
            t.Insert(16);
            t.Insert(19);
            t.Insert(23);
            t.Insert(28);
            t.Insert(38);
            t.Insert(7);
            t.Insert(12);
            t.Insert(15);
            t.Insert(20);
            t.Insert(35);
            t.Insert(39);
            t.Insert(3);
            t.WriteTree("output.txt");
            t.Delete(47);
            t.WriteTree("output1.txt");
        }
    }
}

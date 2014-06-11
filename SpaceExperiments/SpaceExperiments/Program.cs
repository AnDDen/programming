using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            const int m = 3, n = 4;
            int[] p = new int[m] {5, 6, 7};
            int[] c = new int[n] {1, 2, 3, 4};
            SortedSet<int>[] r = new SortedSet<int>[m + 1];
            for (int i = 1; i <= m; i++) 
                r[i] = new SortedSet<int>();
            r[1].Add(1); r[1].Add(2); r[1].Add(3);
            r[2].Add(4); 
            r[3].Add(4); 
            */
            
            const int m = 3, n = 4;
            int[] p = new int[m] { 8, 7, 6 };
            int[] c = new int[n] { 1, 2, 3, 4 };
            SortedSet<int>[] r = new SortedSet<int>[m + 1];
            for (int i = 1; i <= m; i++)
                r[i] = new SortedSet<int>();
            r[1].Add(1); r[1].Add(2); 
            r[2].Add(2);
            r[3].Add(3); r[3].Add(4); 
            

            FlowNetwork FN = new FlowNetwork(m, n, p, c, r);
            FN.Solve();

            Console.Write("Maximum: {0}\nExperiments: ", FN.res);
            for (int i = 0; i < FN.E.Length; i++)
                Console.Write("{0} ", FN.E[i]);
            Console.Write("\nInstruments: ");
            for (int i = 0; i < FN.I.Length; i++)
                Console.Write("{0} ", FN.I[i]);

            FN.Write("output.txt");

            Console.ReadKey();            
        }
    }
}

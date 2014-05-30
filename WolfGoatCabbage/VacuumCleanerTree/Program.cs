using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacuumCleanerTree
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] b1 = new int[3] { 1, 1, 1 }, b2 = new int[3] { 0, 0, 0 };
            TNode start = new TNode(new Inf(b1, b2, -1, "Start"), null);

            SolverTree solve = new SolverTree(start);
            solve.Solve();
            string res = solve.WriteResult();
            Console.Write(res);
            solve.WriteTreeResult("output.txt");
            Console.ReadKey();
        }
    }
}

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
            int[,] data = { { 0, 1, 1 }, { 0, 1, 0 }, { 1, 1, 1 } };
            TNode start = new TNode(new Inf(data, new Point(0, 0), "Start"), null);
            SolverTree solve = new SolverTree(start);
            solve.Solve();
            string res = solve.WriteResult();
            Console.Write(res);
            solve.WriteTreeResult("output.txt");
            Console.ReadKey();
        }
    }
}

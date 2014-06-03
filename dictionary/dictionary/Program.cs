using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict["a"] = 1;
            dict["d"] = 4;
            dict["b"] = 2;
            dict["c"] = 3;
            dict["e"] = 6;
            dict["e"] = 5;

            dict.Remove("c");

            Console.WriteLine(dict["a"]);
            Console.WriteLine(dict["b"]);
            Console.WriteLine(dict["d"]);
            Console.WriteLine(dict["e"]);
            Console.ReadKey();
        }
    }
}

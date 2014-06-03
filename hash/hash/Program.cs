using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hash
{
    class Program
    {
        public static string[] RandomStrArray(int n)
        {
            Random rnd = new Random();
            int m = rnd.Next(3, 10);
            string[] s = new string[n];
            for (int i = 0; i < n; i++)
            {
                s[i] = "";
                for (int j = 0; j < m; j++)
                    s[i] += (char)rnd.Next(80, 150);
            }
            return s;
        }
        
        static void Main(string[] args)
        {
            Hash<int> ht = new Hash<int>(19);
            string[] str = new string[50];

            Random rnd = new Random();
            str = RandomStrArray(50);

            for (int i = 0; i < 50; i++)
            {
                int k = rnd.Next(1, 100);
                ht.Insert(str[i], k);
                Console.WriteLine("{0}: {1}", str[i], k);
            }

            Console.WriteLine("\n***** Output *****");
            for (int i = 0; i < 50; i++)
                Console.WriteLine("{0}: {1}", str[i], ht.Find(str[i]));            

            Console.ReadKey();
        }
    }
}

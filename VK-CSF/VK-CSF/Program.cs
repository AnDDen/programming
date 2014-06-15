using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_CSF
{
    class Program
    {
        static void Main(string[] args)
        {
            VK vk = new VK();
            vk.GetPeople();
            vk.SetAllConnections();
            vk.Write("output.txt");
            Console.WriteLine("\n===== Isolated groups =====");
            vk.IsolatedGroups();
            Console.WriteLine("\n===== Maximum length =====");
            vk.MaxLength();
            Console.ReadKey();
        }
    }
}

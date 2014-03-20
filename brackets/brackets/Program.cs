using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace brackets
{
    class Program
    {
        static int IsOpBracket(char c)
        {
            char[] oBracket = new char[3] { '(', '{', '[' };
            for (int i = 0; i < 3; i++)
                if (c == oBracket[i]) return i;
            return -1;
        }

        static int IsClBracket(char c)
        {
            char[] cBracket = new char[3] { ')', '}', ']' };
            for (int i = 0; i < 3; i++)
                if (c == cBracket[i]) return i;
            return -1;
        }

        static int CheckStr(string s)
        {
            Stack Brackets = new Stack();
            int i = 0;
            int clCount = 0;
            while (i < s.Length)
            {
                if (IsOpBracket(s[i]) != -1)
                    Brackets.PushStack(s[i]);
                else
                {
                    int cl = IsClBracket(s[i]);
                    if (cl != -1)
                        if (Brackets.IsEmpty())
                            return i + 1;
                        else
                        {
                            char c = Brackets.PopStack();
                            clCount++;
                            if (cl != IsOpBracket(c))
                                return i + 1;
                        }
                }
                i++;
            }
            if (!Brackets.IsEmpty())
            {
                return s.Length + 1;
            }
            return -1;
        }  //со стеком

        static int CheckStrList(string s)  //с двусвязным списком
        {
            List Brackets = new List();
            int i = 0;
            int clCount = 0;
            while (i < s.Length)
            {
                if (IsOpBracket(s[i]) != -1)
                    Brackets.Insert(s[i], Brackets.count);
                else
                {
                    int cl = IsClBracket(s[i]);
                    if (cl != -1)
                        if (Brackets.IsEmpty())
                            return i + 1;
                        else
                        {
                            char c = Brackets.GetVal(Brackets.count - 1);
                            Brackets.Delete(Brackets.count - 1);
                            clCount++;
                            if (cl != IsOpBracket(c))
                                return i + 1;
                        }
                }
                i++;
            }
            if (!Brackets.IsEmpty())
            {
                return s.Length + 1;
            }
            return -1;
        }
        
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            int err = CheckStr(s);
            Console.WriteLine("Using Stack");
            if (err == -1)
                Console.WriteLine("Correct");
            else Console.WriteLine("Error in symbol number {0}", err);
            
            err = CheckStrList(s);
            Console.WriteLine("\nUsing List");
            if (err == -1)
                Console.WriteLine("Correct");
            else Console.WriteLine("Error in symbol number {0}", err);
            Console.ReadKey();
        }
    }
}

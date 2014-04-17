using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    class Program
    {        
        public struct Struct
        {
            public string Name;
            public string Surname;
            public int Age;
        }

        delegate object SortKeyDelegate(object x);
        delegate int cmpDelegate(object a, object b, SortKeyDelegate key);

        static string SortKey(object x)
        {
            return ((Struct)x).Name;
        }

        static int Compare(object a, object b, SortKeyDelegate key)
        {
            string s1 = (string)key(a);
            string s2 = (string)key(b);
            
            if (s1.Length != s2.Length)
                if (s1.Length > s2.Length) return 1;
                else return -1;
            else
                if (a == b) return 0;
                else
                {
                    int i = 0;
                    while (s1[i] == s2[i]) i++;
                    if (s1[i] > s2[i]) return 1;
                    else return -1;
                }
        }

   /*     public static int Compare(Struct a, Struct b, Key key, CompareType type, CompareOrder order)
        {
            object o1, o2;
            switch (key)
            {
                case Key.Name: 
                    o1 = a.Name; o2 =  b.Name; break;
                case Key.Surname: 
                    o1 = a.Surname; o2 = b.Surname; break;
                case Key.Age: 
                    o1 = a.Age; o2 = b.Age; break;
                default:
                    throw new System.ArgumentException();
            }
            return CompareObject(o1, o2, type, order);
        }

        public static int CompareObject(object a, object b, CompareType type, CompareOrder order)
        {
            string a1 = Convert.ToString(a);
            string b1 = Convert.ToString(b);
            return (byte)order * CompareString(a1, b1, type);
        }

        public static int CompareString(string a, string b, CompareType type)        //1: a > b; -1: a < b; 0: a = b;
        {
            if (type == CompareType.Length)
            {
                if (a.Length != b.Length)
                    if (a.Length > b.Length) return 1;
                    else return -1;
                else
                    if (a == b) return 0;
                    else
                    {
                        int i = 0;
                        while (a[i] == b[i]) i++;
                        if (a[i] > b[i]) return 1;
                        else return -1;
                    }
            }
            else
            {
                if (a == b) return 0;
                int i = 0, m = Math.Min(a.Length, b.Length) - 1;
                while ((a[i] == b[i]) && (i < m)) i++;
                if (a[i] == b[i])
                    if (a.Length > b.Length) return 1;
                    else return -1;
                else
                    if (a[i] > b[i]) return 1;
                    else return -1;
            }
        }
     */

        static void Swap(ref Struct a, ref Struct b)
        {
            Struct tmp = a;
            a = b;
            b = tmp;
        }

        static void BubbleSort(Struct[] array, SortKeyDelegate key, cmpDelegate cmp, bool reverse)
        {
            int n = array.Length - 1, t;
            if (reverse)
                t = -1;
            else t = 1;
            for (int i = 0; i < n; i++)
                for (int j = n; j > i; j--)
                    if (t * cmp(array[j - 1], array[j], key) > 0)
                        Swap(ref array[j - 1], ref array[j]);
        }

        static void ShackerSort(Struct[] array, SortKeyDelegate key, cmpDelegate cmp, bool reverse)
        {
            int left = 0, right = array.Length - 1, last = right, t;
            if (reverse)
                t = -1;
            else t = 1;
            do
            {
                for (int i = right; i > left; i--)
                    if (t * cmp(array[i - 1], array[i], key) > 0)
                    {
                        Swap(ref array[i - 1], ref array[i]);
                        last = i;
                    }
                left = last;
                for (int i = left; i < right; i++)
                    if (t * cmp(array[i + 1], array[i], key) < 0)
                    {
                        Swap(ref array[i + 1], ref array[i]);
                        last = i;
                    }
                right = last;
            }
            while (left < right);
        }

        static void QuickSort(Struct[] array, int left, int right, SortKeyDelegate key, cmpDelegate cmp, bool reverse)
        {
            int i = left, j = right, t;
            if (reverse)
                t = -1;
            else t = 1;
            Struct x = array[(i + j) / 2];
            do
            {
                while (t * Compare(array[i], x, key) < 0) i++;
                while (t * Compare(array[j], x, key) > 0) j--;
                if (i <= j)
                {
                    Swap(ref array[i], ref array[j]);
                    i++; j--;
                }
            }
            while (i <= j);
            if (j > left)
                QuickSort(array, left, j, key, cmp, reverse);
            if (i < right)
                QuickSort(array, i, right, key, cmp, reverse); 
        }

        static void WriteStruct(Struct[] array)
        {
            for (int i = 0; i < array.Length; i++)
                Console.WriteLine("{0}\t{1}\t{2}", array[i].Name, array[i].Surname, array[i].Age);
        }

        static Struct[] RandomStruct(int n)
        {
            Random rnd = new Random();
            Struct[] A = new Struct[n];
            for (int i = 0; i < n; i++)
            {
                int x = rnd.Next(1, 7);
                string s = "";
                for (int j = 0; j < x; j++)
                    s += (char)('a' + rnd.Next(25));
                A[i].Name = s;

                x = rnd.Next(1, 7);
                s = "";
                for (int j = 0; j < x; j++)
                    s += (char)('a' + rnd.Next(25));
                A[i].Surname = s;

                A[i].Age = rnd.Next(10, 80);
            }
            return A;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter count");
            int n = Convert.ToInt32(Console.ReadLine());
            Struct[] StructArr = new Struct[n];
            StructArr = RandomStruct(n);
            WriteStruct(StructArr);
            Console.WriteLine();
            Console.ReadKey();
            BubbleSort(StructArr, SortKey, Compare, true);
            WriteStruct(StructArr);
            Console.WriteLine();
            Console.ReadKey();
            ShackerSort(StructArr, SortKey, Compare, false);
            WriteStruct(StructArr);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}

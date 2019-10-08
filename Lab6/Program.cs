using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Program
    {
        static void RemoveFromListAt<T>(DynamicList<T> list, int indexToRemove)
        {
            if (list.RemoveAt(indexToRemove))
                Console.WriteLine($"[{indexToRemove}] removed successfully");
            else
                Console.WriteLine($"[{indexToRemove}] wasn't found");
        }
        static void RemoveFromList<T>(DynamicList<T> list, T valueToRemove)
        {
            if (list.Remove(valueToRemove))
                Console.WriteLine($"{valueToRemove} removed successfully");
            else
                Console.WriteLine($"{valueToRemove} wasn't found");
        }

        static void FillStringList(DynamicList<string> list)
        {
            list.Add("Hello, this is the first string.");
            string[] array = { "array of strings 1", "array of strings 2", "array of strings 3", "array of strings 4" };
            list.AddRange(ref array);
            for (int i = 0; i < 20; i++)
                list.Add(Convert.ToString(i));
        }

        static void PrintList<T>(DynamicList<T> list)
        {
            Console.WriteLine("---");
            Console.WriteLine("Foreach printing");
            int index = 0;
            foreach (T value in list)
                Console.WriteLine($"{index++} - {value}");
            Console.WriteLine("---");
        }

        static void PrintListManually<T>(DynamicList<T> list)
        {
            Console.WriteLine("---");
            Console.WriteLine("Manual printing");
            int index = 0;
            IEnumerator<T> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                Console.WriteLine($"{index++} - {enumerator.Current}");
            Console.WriteLine("---");
        }

        static void PrintListReversely<T>(DynamicList<T> list)
        {
            Console.WriteLine("---");
            Console.WriteLine("Reverse printing");
            for (int i = -1; i >= -list.Count; i--)
                Console.WriteLine($"{i} - {list[i]}");
            Console.WriteLine("---");
        }

        static void Main(string[] args)
        {
            Random rand = new Random();
            DynamicList<string> strings = new DynamicList<string>();
            FillStringList(strings);
            PrintListManually(strings);
            PrintListReversely(strings);
            RemoveFromList(strings, "10");
            RemoveFromListAt(strings, 10);
            try
            {
                Console.WriteLine($"[100] - {strings[100]}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            PrintList(strings);
            Console.WriteLine("Clearing");
            strings.Clear();
            PrintList(strings);

            Console.WriteLine("Press F to exit...");
            Console.ReadKey();
        }
    }
}

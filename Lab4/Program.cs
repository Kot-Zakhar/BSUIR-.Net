using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths;
            if (args.Length != 0)
                paths = args;
            else
            {
                Console.WriteLine("Insert path to assembly:");
                paths = new String[1];
                paths[1] = Console.ReadLine();
            }

            foreach (string path in paths)
            {
                InspectAssembly(path);
            }

            Console.WriteLine("Press F to pay respect...");
            Console.ReadKey();

        }

        static void InspectAssembly(string path)
        {
            Assembly currentAssembly = Assembly.LoadFile(path);
            Console.WriteLine(new AssemblyName(currentAssembly.FullName));
            List<Type> types = currentAssembly.GetTypes().Where(type => type.IsPublic).ToList();
            types.Sort((firstType, secondType) =>
            {
                int result = String.Compare(firstType.Namespace, secondType.Namespace);
                if (result == 0)
                    result = String.Compare(firstType.Name, secondType.Name);
                return result;
            });

            types.ForEach(t =>
            {
                Console.WriteLine(t.FullName);
            });
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}

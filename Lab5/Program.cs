using System;
using System.Collections.Generic;
using System.Threading;
using TaskQueues;

namespace Lab5
{
    class Program
    {
        static int taskAmount = 30;
        static Random rand = new Random();
        static void Main(string[] args)
        {
            List<TaskDelegate> tasks = new List<TaskDelegate>();
            for (int i = 0; i < taskAmount; i++)
            {
                tasks.Add(TaskFundtion);
            }

            Parallel.WaitAll(tasks);

            Console.WriteLine("Tasks done. Press F...");
            Console.ReadKey();
        }
        static void TaskFundtion()
        {
            Thread.Sleep(1000);
            Console.WriteLine(String.Format("Thread {0} - Started", Thread.CurrentThread.ManagedThreadId));
            int amount = rand.Next(5, 20);
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine(String.Format("Thread {0} - {1}", Thread.CurrentThread.ManagedThreadId, i));
                Thread.Sleep(2);
            }
            Console.WriteLine(String.Format("Thread {0} - Finished", Thread.CurrentThread.ManagedThreadId));
        }
    }
}

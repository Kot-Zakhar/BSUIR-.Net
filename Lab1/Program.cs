using System;
using System.Threading;
using System.Timers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        static Random random;
        static int minThreadDurationInSec = 100;
        static int maxThreadDurationInSec = 300;
        static int threadAmount = 10;
        static int taskAmount = 500;
        static int allTasksDuration;
        static List<TaskDelegate> tasks = new List<TaskDelegate>();
        static void Main(string[] args)
        {
            random = new Random();

            if (args.Length > 0)
                try
                {
                    threadAmount = Convert.ToInt32(args[0]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Not valid number entered.");
                }

            GenerateTasks(taskAmount);

            Stopwatch watch = new Stopwatch();

            watch.Start();
            Console.WriteLine("Task Queue:\n");
            ProcessTaskQueue(new TaskQueue(threadAmount));
            watch.Stop();
            TimeSpan taskQueueTime = watch.Elapsed;

            watch.Reset();
            watch.Start();
            Console.WriteLine("Stupid task queue:\n");
            ProcessTaskQueue(new StupidTaskQueue(threadAmount));
            watch.Stop();
            TimeSpan stupidTaskQueueTime = watch.Elapsed;

            Console.WriteLine("Completed.");
            Console.WriteLine("TaskQueue: " + taskQueueTime.TotalMilliseconds.ToString());

            Console.WriteLine("StupidTaskQueue: " + stupidTaskQueueTime.TotalMilliseconds.ToString());

            Console.WriteLine("Complite duration: " + allTasksDuration.ToString());


            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        static private void ProcessTaskQueue(ITaskQueue queue)
        {
            int i = 0;
            foreach (TaskDelegate task in tasks)
            {
                Console.WriteLine("Task " + Convert.ToString(i++) + " enqueued.");
                queue.EnqueueTask(task);
            }
            queue.WaitAll();
        }

        static private void GenerateTasks(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int localI = i;
                int randomTime = random.Next(minThreadDurationInSec, maxThreadDurationInSec);
                allTasksDuration += randomTime;
                tasks.Add(() => {
                    SomeHeavyFunction(Convert.ToString(localI), randomTime);
                });
            }
        }

        static private void SomeHeavyFunction(string taskName, int duration)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " - Task " + taskName + " started working. " + duration.ToString() + "ms");
            Thread.Sleep(duration);
            Console.WriteLine("- Task " + taskName + " finished working.");
        }
    }
}

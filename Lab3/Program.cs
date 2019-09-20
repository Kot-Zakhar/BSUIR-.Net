using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<Thread> threads = new Queue<Thread>();
            Mutex mutex = new Mutex();
            for (int i = 0; i < 5; i++)
            {
                Thread newThread = new Thread(new ThreadStart(() =>
                {
                    Console.WriteLine(String.Format("{0} is trying to lock mutex.", Thread.CurrentThread.ManagedThreadId));


                    //int result = mutex.Unlock();
                    //if (result == 0)
                    //    Console.WriteLine(String.Format("Mutex unlocked by {0}", Thread.CurrentThread.ManagedThreadId));
                    //else
                    //    Console.WriteLine(String.Format("{0} can't unlock mutex, which is locked by {1}", Thread.CurrentThread.ManagedThreadId, result));


                    mutex.Lock();
                    Console.WriteLine(String.Format("{0} locked mutex", Thread.CurrentThread.ManagedThreadId));
                    ThreadFunction();

                    int result = mutex.Unlock();
                    if (result == 0)
                        Console.WriteLine(String.Format("Mutex unlocked by {0}", Thread.CurrentThread.ManagedThreadId));
                    else
                        Console.WriteLine(String.Format("{0} can't unlock mutex, which is locked by {1}", Thread.CurrentThread.ManagedThreadId, result));

                }));
                threads.Enqueue(newThread);
                newThread.Start();
            }
            while(threads.Count != 0)
            {
                threads.Dequeue().Join();
            }
            Console.WriteLine("Press F to pay respect...");
            Console.ReadKey();
        }

        static void ThreadFunction()
        {
            Thread.Sleep(100);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(String.Format("{0} - {1}", Thread.CurrentThread.ManagedThreadId, i));
            }
        }
    }
}

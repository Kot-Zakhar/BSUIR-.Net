using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab1
{
    class StupidTaskQueue : ITaskQueue
    {
        private Queue<TaskDelegate> taskDelegates = new Queue<TaskDelegate>();
        private bool needToStop = false;
        private Object stopLockObject = new object();
        private int maxThreadAmount = 0;
        private Thread threadManager;

        public StupidTaskQueue(int threadAmount)
        {
            maxThreadAmount = threadAmount;
            threadManager = new Thread(new ThreadStart(ThreadManagerFunction));
            threadManager.Start();
        }

        private void ThreadFuncton()
        {
            bool stopSignal = false;
            while (!stopSignal)
            {
                TaskDelegate currentTask = null;
                lock (taskDelegates)
                {
                    if (taskDelegates.Count > 0)
                        currentTask = taskDelegates.Dequeue();
                }
                if (currentTask != null)
                    currentTask();
                else
                    lock (stopLockObject)
                    {
                        stopSignal = needToStop;
                    }
            };
        }

        private void ThreadManagerFunction()
        {
            List<Thread> workingThreads = new List<Thread>();
            int threadCounter = 0;
            bool localStop = false;
            while (!localStop || workingThreads.Count > 0)
            {
                if (workingThreads.Count < maxThreadAmount)
                {
                    TaskDelegate currentTask = null;
                    lock (taskDelegates)
                    {
                        if (taskDelegates.Count > 0)
                            currentTask = taskDelegates.Dequeue();
                    }
                    if (currentTask != null)
                    {
                        Thread newThread = new Thread(new ThreadStart(currentTask));
                        newThread.Name = "Thread " + Convert.ToString(threadCounter++);
                        newThread.Start();
                        workingThreads.Add(newThread);
                    }
                }

                workingThreads = workingThreads.Where((thread) =>
                {
                    if (!thread.IsAlive)
                    {
                        thread.Join();
                        Console.WriteLine("Joining " + thread.Name);
                    }
                    return thread.IsAlive;
                }).ToList();
                

                lock (stopLockObject)
                {
                    localStop = needToStop;
                }
            }

        }

        public void EnqueueTask(TaskDelegate task)
        {
            lock (taskDelegates)
            {
                taskDelegates.Enqueue(task);
            }
        }

        public void WaitAll()
        {
            Console.WriteLine("waiting all");
            // here is the problem
            lock (stopLockObject)
            {
                needToStop = true;
            }
            threadManager.Join();
            Console.WriteLine("Joined all");
        }
    }
}

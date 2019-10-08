using System;
using System.Collections.Generic;
using System.Threading;

namespace TaskQueues
{
    public delegate void TaskDelegate();

    public class TaskQueue
    {
        private Queue<TaskDelegate> taskDelegates = new Queue<TaskDelegate>();
        private Queue<Thread> freeThreads = new Queue<Thread>();
        private List<Thread> workingThreads = new List<Thread>();
        private bool needToStop = false;
        private Object stopLockObject = new object();

        public TaskQueue(int threadAmount)
        {
            for (int i = 0; i < threadAmount; i++)
            {
                Thread newThread = new Thread(new ThreadStart(this.ThreadFuncton));
                newThread.Name = "Thread " + Convert.ToString(i);
                //freeThreads.Enqueue(newThread);
                newThread.Start();
                workingThreads.Add(newThread);
            }
        }

        ~TaskQueue()
        {
            while (freeThreads.Count > 0)
                freeThreads.Dequeue().Abort();
            taskDelegates.Clear();
            foreach (Thread thread in workingThreads)
                thread.Abort();
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

        public void EnqueueTask(TaskDelegate task)
        {
            lock (taskDelegates)
            {
                taskDelegates.Enqueue(task);
            }
        }

        public void WaitAll()
        {
            lock (stopLockObject)
            {
                needToStop = true;
            }
            foreach (Thread thread in workingThreads)
            {
                thread.Join();
                Console.WriteLine("Joining " + thread.ManagedThreadId);
            }
        }
    }
}

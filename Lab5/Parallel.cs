using System;
using System.Collections.Generic;
using System.Text;
using TaskQueues;

namespace Lab5
{
    public static class Parallel
    {
        public static void WaitAll(List<TaskDelegate> tasks)
        {
            TaskQueue taskQueue = new TaskQueue(10);
            tasks.ForEach(task => { taskQueue.EnqueueTask(task); });
            taskQueue.WaitAll();
        }
    }
}

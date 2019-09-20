using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab3
{
    class Mutex
    {
        private int threadId = 0;

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref threadId, Thread.CurrentThread.ManagedThreadId, 0) != 0)
            {
                Thread.Yield();
            }
        }

        public int Unlock()
        // returns 0 if unlock happened successfully
        // and if mutex is locked - returns id of thread, wich locked current mutex
        {
            return Interlocked.CompareExchange(ref threadId, 0, Thread.CurrentThread.ManagedThreadId) == Thread.CurrentThread.ManagedThreadId ? 0 : threadId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Lab2
{
    class Program
    {
        static TaskQueue copyQueue;
        static int threadAmount = 10;
        static int filesCopied = 0;
        static object SyncObject = new object();
        static void Main(string[] args)
        {
            string fromDirectoryPath;
            string toDirectoryPath;
            if (args.Length > 0)
            {
                fromDirectoryPath = args[0];
                toDirectoryPath = args[1];
            }
            else
            {
                do
                {
                    Console.Write("From: ");
                    fromDirectoryPath = Console.ReadLine();
                } while (File.Exists(fromDirectoryPath));
                Console.Write("To: ");
                toDirectoryPath = Console.ReadLine();
            }

            copyQueue = new TaskQueue(threadAmount);

            DirectoryInfo fromDirectory = new DirectoryInfo(fromDirectoryPath);
            DirectoryInfo toDirectory = new DirectoryInfo(toDirectoryPath);
            if (fromDirectory.Exists)
            {
                CopyDirectory(fromDirectory, toDirectory);
            }
            else
            {
                Console.WriteLine("From directory doesn't exist.");
            }
            copyQueue.WaitAll();
            Console.WriteLine("Copied " + filesCopied.ToString() + " files successfully. Press F to pay respect...");
            Console.ReadLine();
        }

        static void CopyDirectory(DirectoryInfo fromDirectory, DirectoryInfo toDirectory)
        {
            DirectoryInfo[] directories = fromDirectory.GetDirectories();

            if (!toDirectory.Exists)
                toDirectory.Create();

            FileInfo[] files = fromDirectory.GetFiles();
            foreach(FileInfo file in files)
            {
                string newPath = Path.Combine(toDirectory.FullName, file.Name);
                    copyQueue.EnqueueTask(() =>
                    {
                        try
                        {
                            Console.WriteLine(Thread.CurrentThread.Name +  ": Copying file:" + file.FullName);
                            file.CopyTo(newPath);
                            lock (SyncObject)
                            {
                            filesCopied++;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: " + e);
                        }
                    });
            }

            foreach (DirectoryInfo directory in directories)
            {
                string newPath = Path.Combine(toDirectory.FullName, directory.Name);
                CopyDirectory(directory, new DirectoryInfo(newPath));
            }
                
        }
    }
}

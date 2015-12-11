using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dorkari.Samples.Cmd.Threads
{
    class TaskWithAwait
    {
        public Task[] StartProcessing()
        {
            var jobList = Enumerable.Range(1, 10).ToArray();
            var tasks = new Task[10];
            //[1] start 10 jobs, one-by-one
            for (int i = 0; i < jobList.Count(); i++)
            {
                tasks[i] = ProcessJob(jobList[i]);
            }
            //[4] here we have 10 awaitable Task in tasks
            //[5] do all other unrelated operations
            Thread.Sleep(1500); //assume it works for 1.5 sec
            // Task.WaitAll(tasks); //[6] wait for tasks 
            // The process is complete here
            return tasks;
        }

        public async Task ProcessJob(int jobTask)
        {
            try
            {
                //[2] start job in a ThreadPool, Background thread
                var T = Task.Factory.StartNew(() =>
                {
                    JobWorker jobWorker = new JobWorker();
                    jobWorker.Execute(jobTask);
                });
                //[3] await here will keep context of calling thread
                await T; //... and release the calling thread
            }
            catch (Exception) { /*handle*/ }
        }
    }

    public class JobWorker
    {
        static object locker = new object();
        const string _file = @"C:\Arghya\Logs\out.txt";
        public void Execute(int jobTask) //on complete, writes in file
        {
            Thread.Sleep(500); //let's assume does something for 0.5 sec
            lock (locker)
            {
                File.AppendAllText(_file,
                    Environment.NewLine + "Wrirting the value-" + jobTask);
            }
        }
    }
}

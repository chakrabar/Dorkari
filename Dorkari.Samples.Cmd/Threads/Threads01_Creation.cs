using System;
using System.IO;
using System.Threading;

namespace Dorkari.Samples.Cmd.Threads
{
    delegate int del(int i);

    class Threads01_Creation
    {
        static object _obj = new object();

        void Show()
        {
            //Main threads is always foreground, and not from threadPool. Remains constant till end of process
            Console.WriteLine("Main thread is a background therad ? " + Thread.CurrentThread.IsBackground.ToString());
            Console.WriteLine("Main thread is a ThreadPool therad ? " + Thread.CurrentThread.IsThreadPoolThread.ToString());
            Console.WriteLine("Main thread has id : " + Thread.CurrentThread.ManagedThreadId);
            //##### raw threads - start #####

            //simple thread | no input | no output
            //Thread t2 = new Thread(WriteSomething); //=> Kick off a new thread | Foreground thread by default
            //t2.IsBackground = true; //=> to make background | App doesn't wait for it | Terminates when process/main thread exits!
            //t2.Start(); //=> void | start method in new thread | takes roughly 1 millisecond to create thread and start
            //t2.Join(500); //=> bool | block main thread and wait for t2, till 500 ms

            //pass "object" input | no other types can be passed like this
            //var t1 = new Thread(MyWaitCallback); //=> Raw created threads are Foreground, non-ThreadPool
            //t1.Start("RawThread");

            //pass non-object/multiple input
            //Thread t = new Thread(delegate() { WriteIt(200, "foo"); }); 
            //t.Start(); // running WriteIt()
            //new Thread(() => WriteIt(500, "bar")).Start(); //=> Foreground thread by default

            //simple return from thread
            //int val = 0;
            //new Thread(() => val = ReturnIt(10)).Start();

            //##### raw threads - end #####


            //##### thread pool with simple void methods #####

            //Queue user work item to thread pool
            //ThreadPool.QueueUserWorkItem(MyWaitCallback, null); //=> bool | successfully queued | *Runs as background thread*
            //ThreadPool.QueueUserWorkItem(state => MyWaitCallback("lamb")); //as lambda | Hard to get return value from QUWI

            // Get return value from queued thread with asynchronous delegate
            Func<int, int> method = ReturnIt;

            var asyncResult10 = method.BeginInvoke(10, null, null); //=> starts method asynchronously, in a thread pool, background thread

            //AsyncCallback is Action<IAsyncResult>
            AsyncCallback acb = iar => Console.WriteLine("I got an IAsyncResult, who says complete!"); //=> void callback, triggered by method complete signalled by IAsyncResult
            var asyncResult11 = method.BeginInvoke(11, acb, null); //=> uses AsyncCallback, which will run on complete | This is NON-BLOCKING

            AsyncCallback acb2 = iar => Console.WriteLine("The state of IAsyncResult : " + iar.AsyncState.ToString()); //=> use random object passed in BeginInvoke
            var asyncResult13 = method.BeginInvoke(13, acb2, "SomeToken"); //=> pass an object to AsynCallback

            var asyncResult12 = method.BeginInvoke(12, null, null); //=> this will be waited by EndInvoke below
            var result = method.EndInvoke(asyncResult12); //=> wait for it to complete, and then return value | This is BLOCKING
            Console.WriteLine("Completed synchronously ? " + asyncResult12.CompletedSynchronously.ToString());
            Console.WriteLine("End invoke " + result);

            //This is an EPIC complex delegate to get return value of method in AsyncCallback | Do EndInvoke of same method by using internally passed IAsyncresult
            AsyncCallback postProcess = (iar) => { int retVal = ((Func<int, int>)iar.AsyncState).EndInvoke(iar); Console.WriteLine("Method returned : " + retVal); };
            var asyncResult14 = method.BeginInvoke(14, postProcess, method); //=> pass an object to AsynCallback

            ThreadPool.SetMaxThreads(500, 500); //=> [1] max worker threads in pool, [2] max async IO threads in pool
            ThreadPool.SetMinThreads(20, 20); //=> [1] min thread that'll be created spontaniously on-demand, without using ThreadPool's thread managing algorithm
            //=> [2] min no. of asynchronous IO threads that TP will create on demand
            //TP will start with threads equal to number of cores. Then the algo => whenever thread is blocked, create new thread per 0.5 second of any blocked thread

            Console.WriteLine("Ending thread is a background therad ? " + Thread.CurrentThread.IsBackground.ToString());
            Console.WriteLine("Ending thread is a ThreadPool therad ? " + Thread.CurrentThread.IsThreadPoolThread.ToString());
            Console.WriteLine("Ending thread has id : " + Thread.CurrentThread.ManagedThreadId);
            Console.ReadLine();
        }

        static void WriteIt(int i, string s)
        {
            for (int j = 0; j < 100; j++)
            {
                Console.Write("{0}-{1}-{2} ", i, j, s);
            }
        }

        static void WriteSingle(string s)
        {
            for (int j = 0; j < 100; j++)
            {
                Console.Write("{0}-{1} ", j, s);
            }
        }

        static void WriteSomething()
        {
            for (int j = 0; j < 100; j++)
            {
                Console.Write(j + " ");
            }
        }

        static int ReturnIt(int i)
        {
            Console.WriteLine("This is a background therad ? " + Thread.CurrentThread.IsBackground.ToString());
            Console.WriteLine("This is a ThreadPool therad ? " + Thread.CurrentThread.IsThreadPoolThread.ToString());
            Console.WriteLine("This has id : " + Thread.CurrentThread.ManagedThreadId + ", i = " + i);
            Thread.Sleep(3500);
            Console.WriteLine("Method ending...");
            return ++i;
        }

        static void MyWaitCallback(object data) //specially for ThreadPool.QueueUserWorkItem
        {
            const string _File = @"C:\Users\607850442\Desktop\Desktop\Thread.txt";
            for (int j = 0; j < 100; j++)
            {
                lock (_obj)
                {
                    var msg = string.Format(data == null ? "Null Object-{0} " : "Got Value-{0} ", j);
                    File.AppendAllText(_File, msg);
                }
            }
        }
    }
}

using Dorkari.Helpers.Core.Utilities;
using System;
using System.Threading;

namespace Dorkari.Samples.Cmd.Examples
{
    class PollerExample
    {
        public static void Show()
        {
            //basic
            new Poller()
                .Execute(() => TestMeth(10));

            //Func example
            string result = new Poller()
                            .WithException<ArgumentNullException>()
                            .WithException<DivideByZeroException>()
                            .WithRetries(3)
                            .WithWait(3000)
                            .Execute(() => GetTestString(1));

            //Action example
            new Poller()
                .StopOnException<ApplicationException>()
                .WithRetries(5)
                .WithWait(4000)
                .Execute(() => TestMeth(1));
        }

        static string GetTestString(int arg)
        {
            Thread.Sleep(200);
            if (DateTime.Now.Second % 2 == 0)
                return arg.ToString();
            throw new DivideByZeroException();
        }

        static void TestMeth(int arg)
        {
            Thread.Sleep(200);
            if (DateTime.Now.Second % 2 == 0)
                throw new InvalidOperationException("Allowed");
            throw new ApplicationException("Not Allowed");
        }
    }
}

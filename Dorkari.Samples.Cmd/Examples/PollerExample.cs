using Dorkari.Helpers.Core.Utilities;
using System;
using System.Threading;

namespace Dorkari.Samples.Cmd.Examples
{
    class PollerExample
    {
        public static void Show()
        {
            string result = new Poller()
                            .WithException<ArgumentNullException>()
                            .WithException<DivideByZeroException>()
                            .WithRetries(3)
                            .WithWait(4000)
                            .Execute(() => GetTestString(1));

            new Poller()
                .StopOnException<ApplicationException>()
                .WithRetries(3)
                .WithWait(4000)
                .Execute(() => TestMeth(1));
        }

        static string GetTestString(int arg)
        {
            Thread.Sleep(1000);
            if (DateTime.Now.Second % 2 == 0)
                return arg.ToString();
            throw new DivideByZeroException();
        }

        static void TestMeth(int arg)
        {
            Thread.Sleep(1000);
            if (DateTime.Now.Second % 2 == 0)
                return;
            throw new DivideByZeroException();
        }
    }
}

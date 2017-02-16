using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dorkari.Samples.Cmd.Fun
{
    class ThreadConsoleWriter
    {
        static object locker = new object();
        static int iCommon = 0;
        static int jCommon = 0;

        public static void Show()
        {
            Task a = Task.Run(() =>
            {
                for (int i = 0; i <= 15; i++)
                {
                    Thread.Sleep(700);
                    iCommon = i;
                    WriteText();
                }
            });

            Task b = Task.Run(() =>
            {
                for (int j = 0; j <= 10; j++)
                {
                    Thread.Sleep(1150);
                    jCommon = j;
                    WriteText();
                }
            });
        }

        static void WriteText()
        {
            lock (locker)
            {
                var text = string.Format("i = {0}{1}j = {2}", iCommon, Environment.NewLine, jCommon);
                Console.SetCursorPosition(0, Console.CursorTop == 0 ? 0 : Console.CursorTop - 1);
                Console.Write(text);
            }
        }
    }
}

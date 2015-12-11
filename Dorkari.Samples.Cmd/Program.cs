using Dorkari.Samples.Cmd.Fun;
using System;

namespace Dorkari.Samples.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadConsoleWriter.Start();
            Console.ReadLine();

            Console.WriteLine("Dorkari tests");
            Console.ReadLine();
        }
    }
}

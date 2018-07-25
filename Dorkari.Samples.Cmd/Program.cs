using Dorkari.Samples.Cmd.Examples;
using Dorkari.Samples.Cmd.Tests;
using System;

namespace Dorkari.Samples.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CollectionExamples.Show();
                ListExamples.Show();
                PollerExample.Show();
                ReflectionExamples.Show();
                StringExamples.Show();

                var counter3 = Closure.WithLambdaFunc();
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("Counter :: " + counter3());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception!! Message: {0}", ex.Message);
            }

            Console.WriteLine("Dorkari tests");
            Console.ReadLine();
        }
    }
}

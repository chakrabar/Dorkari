﻿using Dorkari.Samples.Cmd.Examples;
using System;

namespace Dorkari.Samples.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //CollectionExamples.Show();
                //PollerExample.Show();
                ReflectionExamples.Show();
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

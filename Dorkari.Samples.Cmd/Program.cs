using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dorkari.Samples.Cmd.Examples;
using Dorkari.Samples.Cmd.Models;
using Dorkari.Helpers.Core.Extensions;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;

namespace Dorkari.Samples.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CollectionExamples.Show();
                PollerExample.Show();
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

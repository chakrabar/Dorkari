using System;

namespace Dorkari.Samples.Cmd.Tests
{
    class TypeInferenceTest
    {
        public static void Start()
        {
            //Method("haha", "hihi"); //ambiguous - compile time error
        }

        public static void Method(string s, object o)
        {
            Console.WriteLine("string s, object o - called with parameter types {0}, {1}.", s.GetType().FullName, o.GetType().FullName);
        }

        public static void Method(object o, string s)
        {
            Console.WriteLine("object o, string s - called with parameter types {0}, {1}.", o.GetType().FullName, s.GetType().FullName);
        }
    }
}

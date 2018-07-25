//In essence, a closure is a block of code which can be executed at a later time, 
//but which maintains the environment in which it was first created - i.e.
//it can still use the local variables etc of the method which created it, 
//even after that method has finished executing.

//The general feature of closures is implemented in C# by anonymous methods and 
//lambda expressions. https://stackoverflow.com/questions/428617/what-are-closures-in-net

//Closures are basically a function created within another function, which captures or 
//encloses the variables from outer function. The actually have reference to that variable
//(through a compiler generated internal class) and gets the latest value of it!

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Samples.Cmd.Tests
{
    public class Closure
    {
        public static Action WithDelegate()
        {
            int counter = 0;
            return delegate
            {
                counter++;
                Console.WriteLine("Counter = " + counter);
            };
        }

        public static Action WithLambdaAction()
        {
            int counter = 0;
            return () => Console.WriteLine("Counter => " + ++counter);
        }

        public static Func<int> WithLambdaFunc()
        {
            var counter = 0;
            return () => ++counter;
        }

        //This does NOT behave as explined in https://blogs.msdn.microsoft.com/ericlippert/2009/11/12/closing-over-the-loop-variable-considered-harmful/
        public static IEnumerable<Func<int>> ClosureInLoop()
        {
            var values = new[] { 15, 25, 35 };
            var funcs = new List<Func<int>>();

            foreach (var val in values)
            {
                funcs.Add(() => val);
            }

            return funcs;
        }

        public static void LoopIterationWithLambda()
        {
            var computes = new List<Func<int>>();

            foreach (var i in Enumerable.Range(0, 10))
            {
                computes.Add(() => i * 2);
            }

            foreach (var func in computes)
            {
                Console.WriteLine(func());
            }
        }
    }
}
using System;

namespace Dorkari.Samples.Cmd.Tests
{
    interface ISomeContract
    {
        void DoSomething();
    }

    class SomeBaseClass
    {
        public int MyProperty { get; set; }
    }

    //static classes cannot derive from class or implement interface
    static class StaticAndSingleton //: SomeBaseClass //,ISomeContract
    {
        public static void DoSomething()
        {
            throw new NotImplementedException();
        }
    }
}

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

    static class StaticBaseClass
    {
        public static string MyString { get; set; }
    }

    //STATIC classes CANNOT derive from (static or instance) class or implement interface
    static class StaticAndSingleton //: SomeBaseClass //,ISomeContract //,StaticBaseClass
    {
        public static void DoSomething()
        {
            throw new NotImplementedException();
        }
    }

    //instance classes also CANNOT derive from STATIC
    class AnotherClass : ISomeContract //: StaticBaseClass
    {
        public void DoSomething() //interface implementation method CANNOT be static
        {
            throw new NotImplementedException();
        }
    }
}

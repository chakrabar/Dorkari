
namespace Dorkari.Samples.Cmd.Tests
{
    class GenericInheritance
    {
    }

    public class Base<T, U> where U : new() //to call ctor in Method()
    {
        T _field;
        U _field2;
        public void Method() //U already declared at class level
        {
            _field2 = new U();
        }
        public T Method2(T value) //just an example
        {
            _field = value;
            return _field;
        }
    }

    public class Derived : Base<int, MyClass>
    {
        public new void Method() //new to hide base Method()
        {
            base.Method(); //type string already declared at class
        }
    }

    public class MyClass
    {
        public MyClass() //parameterless constructor
        { }
    }
}

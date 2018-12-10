namespace Dorkari.Samples.Cmd.Tests
{
    public interface ISomething
    {
        int Sum(int x, int y);
    }
    public class BaseClass1
    {
        public virtual int Sum(int x, int y) => x + y;
    }

    //class can implement interface & inherit class but NOT struct
    public class DerivedClass1 : BaseClass1, ISomething //Polygon
    {
        public DerivedClass1()
        { }
    }

    public struct Polygon
    {
        //struct cannot have explicit parameterless constructor
        public Polygon(int sides)
        {
            Sides = sides;
        }

        public int Sides { get; set; } //can have methos, events & indexers
    }

    //struct can only implement interface, CANNOT inherit class or struct
    public struct Rectangle : ISomething //Polygon
    {
        public int Sum(int x, int y) => x + y;
    }
}

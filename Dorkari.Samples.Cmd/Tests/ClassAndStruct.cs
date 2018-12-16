namespace Dorkari.Samples.Cmd.Tests
{
    public interface IAnimal
    {
        string MakeNoise();
    }
    public class DogFamily
    {
        public virtual string MakeNoise() => "Aaooo";
    }

    //class can implement interface & inherit class but NOT struct
    public class Husky : DogFamily, IAnimal //Polygon
    {
        public Husky() { } //constructor

        ~Husky() { } //destructor
    }

    public struct Polygon
    {
        //struct cannot have explicit parameterless constructor
        public Polygon(int sides)
        {
            Sides = sides;
        }

        //can have properties, methods, events & indexers
        public int Sides { get; set; }

        //ONLY classes can have destructors
        //~Polygon() { }
    }

    //struct can only implement interface, CANNOT inherit class or struct
    public struct Rectangle : IAnimal //Polygon
    {
        public int Height { get; set; }
        public int Width { get; set; }

        //this is bad example, do not do in real code
        public string MakeNoise() => throw new System.NotImplementedException();

        //Struct supports Deconstructor like class (C# 7.0)
        //needs System.ValueTuple NuGet for < .NET 4.6.2, .NET Core 1.x, .NET Standard 1.x
        public void Deconstruct(out int height, out int width) 
            => (height, width) = (Height, Width);
    }
}

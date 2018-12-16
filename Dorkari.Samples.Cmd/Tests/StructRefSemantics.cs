namespace Dorkari.Samples.Cmd.Tests
{
    public class StructRefSemantics
    {
        #region In

        //Standard use of ref and out parameters
        //out parameter need not be initialized
        public void Half(ref double value, out double result)
        {
            var input = value; //read from ref parameter
            value = input / 2; //setting value of ref, not super intuitive but fine
            result = value; //setting value of out parameter, pretty clean
        }

        //Using in modifier to better represent intent
        public double Square(in double value)
        {
            //following line does not compile, as it tries to set value of in-parameter
            //value = value * value; //error: in double variable is readonly
            return value * value;
        }

        public void Test()
        {
            double x;
            //following does not work as ref parameter was not initialized
            //Half(ref x, out x);
            //following does not work either, in parameter was not set
            //var square = Square(x);

            x = 10; //now both will work
            Half(ref x, out double y); //out can also be defined inline
            var square = Square(x);
        }

        #endregion

        #region Readonly Struct

        public void TestReadonlyStruct(in Velocity velocity)
        {
            var speed = velocity.Speed;
        }

        #endregion

        #region Ref Struct

        public void TestRefStruct()
        {
            //velocity is a (readonly) struct defined below
            var readonlyStruct = new Velocity(10, "NW");
            object boxed = readonlyStruct; //struct can be boxed
            var refStruct = new Temperature();
            //ref struct CANNOT be boxed
            //boxed = refStruct; //NOT ALLOWED
        }

        class TestClass
        {
            public Velocity Velocity { get; set; }
            //NOT ALLOWED
            //ref struct CANNOT be property of a class
            //public Temperature Temperature { get; set; }
        }

        ref struct Weather
        {
            //ref struct can be used as property of
            //another ref struct ONLY
            public Temperature Temperature { get; set; }
        }

        #endregion  
    }

    public readonly struct Velocity
    {
        private readonly string _direction;
        public double Speed { get; }
        public string Unit => "m/s";

        public Velocity(double speed, string direction)
        {
            Speed = speed;
            _direction = direction;
        }
    }

    public ref struct Temperature
    {
        public int Value { get; set; }
        public int Unit { get; set; }
    }
}

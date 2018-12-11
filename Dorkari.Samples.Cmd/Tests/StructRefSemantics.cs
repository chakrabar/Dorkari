namespace Dorkari.Samples.Cmd.Tests
{
    public class StructRefSemantics
    {
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
    }
}

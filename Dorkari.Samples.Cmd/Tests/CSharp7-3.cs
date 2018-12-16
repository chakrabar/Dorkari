namespace Dorkari.Samples.Cmd.Tests
{
    //enhanced generic type constraints
    public class UsingEnum<T> where T : System.Enum { }
    public class UsingDelegate<T> where T : System.Delegate { }
    public class Multicaster<T> where T : System.MulticastDelegate { }

    //tuple equality with == and !=
    class TupleEquality
    {
        internal void TestTupleEquality()
        {
            var t1 = (A: 1, B: "hello");
            var t2 = (A: 1, B: "hello");
            //making it nullable
            (int A, string B)? t3 = t2;
            //with compatible type
            (long A, string B) t4 = (1, "hello");
            //with different names
            var t5 = (X: 1, Y: "hello");

            var test1 = t1 == t2; //true
            var test2 = t1 == t3; //true
            var test3 = t1 == t4; //true
            var test4 = t1 == t5; //true

            //this DOES NOT WORK for type mismatch
            var t6 = (B: "hello", A: 1);
        }
    }
}

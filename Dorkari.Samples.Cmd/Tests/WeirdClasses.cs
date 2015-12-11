using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Tests
{
    class WeirdClasses
    {
        static int TestIntegerIncrement() //lack of better name
        {
            int c = 5;
            c = c++;
            return c;
        }
    }

    //[1] var myClass = new MyListClass { List = { 1, 2, 3 } }; //does what???? new {...} works!    
    internal sealed class WeirdList
    {
        List<String> string1 = new List<String>();
        List<String> string2 = new List<String>();

        public List<String> GetString1()
        {
            return string1;
        }

        public List<String> GetString2()
        {
            return string2;
        }

        public List<String> List
        {
            get
            {
                return string1;
            }
            set
            {
                string2 = value;
            }
        }
    }

    //[2] myClass.Name = "whatever" StackOverflow!
    internal sealed class WeirdProperty
    {
        public string Name
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Tests
{
    class ArrayAndIList
    {
        void Show()
        {
            IList<string> strings1 = GetGoodIList();
            strings1.Add("yooo"); //works

            IList<string> strings2 = GetBaadIList();
            strings2.Add("booo"); //doesn't work! Array is an magic, manages to partially implement IList!
        }

        static IList<string> GetGoodIList()
        {
            return new List<string> { "a", "b" };
        }

        static IList<string> GetBaadIList()
        {
            var str = new string[] { "c", "d" };
            Array.Sort(str);
            return str;
        }
    }
}

using Dorkari.Helpers.Core.Extensions;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Examples
{
    public class ListExamples
    {
        public static void Show()
        {
            HasSubSequenceExample();
        }

        private static void HasSubSequenceExample()
        {
            var l1 = new List<int> { 1, 2, 3, 4, 5 };
            var l2 = new List<int> { 4, 5 };
            var l3 = new List<int> { 1, 3 };
            var l4 = new List<int> { 5, 6 };

            var l5 = new List<int> { 1, 2, 3, 2, 5, 6, 2, 4, 8, 5 };
            var l6 = new List<int> { 2, 4 };
            var l7 = new List<int> { 5, 8 };

            var test1 = l1.HasSubSequence(l2); //true
            var test2 = l1.HasSubSequence(l3); //false
            var test3 = l1.HasSubSequence(l4); //false

            var test5 = l5.HasSubSequence(l6); //true
            var test6 = l5.HasSubSequence(l7); //false
        }
    }
}

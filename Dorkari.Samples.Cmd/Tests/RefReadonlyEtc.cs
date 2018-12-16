using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dorkari.Samples.Cmd.Tests
{
    public class RefReadonlyEtc
    {
        #region In

        //can't have signatures that differ only by `ref`, `in`, or `out`
        public void M(int x) { }
        //public void M(ref int x) { }
        public void M(in int x) { }
        //public void M(out int x) => x = 10;

        //following methods do not work if parameter has ref or other modifiers
        public IEnumerable<int> GetItems(int x)
        {
            for (int i = 0; i < x; i++)
                yield return i;
        }
        public async Task<int> GetValAsync(int x)
        {
            await Task.Delay(100);
            return x * 2;
        }

        public void M1(in int x) { }
        public void M2(int x) { }
        public void M2(in int x) { }
        public void M3(in int x = 100) { }

        public void TestIn()
        {
            M1(10); //works fine
            //M1(in 30); //NOT allowed
            var n = 30;
            M1(in n); //this works

            M2(n); //calls first M2
            M2(in n); //calls second M2
        }

        #endregion

        #region Ref-Return

        ref int GetPrevious(int[] arr, int idx)
        {
            if (arr == null || idx <= 0)
                throw new ArgumentException();
            return ref arr[idx - 1];
        }

        ref int GetNext(int[] arr, int idx)
        {
            if (arr == null || idx >= arr.Length - 1)
                throw new ArgumentException();
            return ref arr[idx + 1];
        }

        void TestRefReturn()
        {
            var arr = new[] { 1, 2, 3, 4, 5 };
            ref int arrItem = ref GetPrevious(arr, 2);
            //ONLY C# 7.3 and above
            arrItem = ref GetPrevious(arr, 2);
        }

        #endregion

        public void TestRefReadonly()
        {
            var structArr = new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 4), new Coordinate(3, 6) };
            var coordinate = new Coordinate();
            coordinate.SetCoordinates(structArr);

            //without `ref` it just passes by value (a copy)
            var coByVal = coordinate.GetCoordinate(1);
            coByVal.Y = 100; //2,100
            var originalData = coordinate.GetItemsAsString(); //1,2 - 2,4 - 3,6

            //Actual use of ref readonly for readonly (immutable) reference
            ref readonly var coByRef = ref coordinate.GetCoordinate(1);
            //coByRef.Y = 100; >>>> not allowed <<<<
            var y = coByRef.Y; //2,4 - gets the value fine, as ref
        }
    }

    public struct Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
            _array = null;
        }

        private Coordinate[] _array;
        public int X { get; set; }
        public int Y { get; set; }

        public void SetCoordinates(Coordinate[] coordinates) => _array = coordinates;

        public ref readonly Coordinate GetCoordinate(int x)
        {
            if (_array == null || x < 0 || x >= _array.Length)
                throw new ArgumentException();
            return ref _array[x];
        }

        public string GetItemsAsString()
        {
            return _array == null ? string.Empty
                : string.Join(" - ", _array.Select(a => $"{a.X},{a.Y}"));
        }
    }
}

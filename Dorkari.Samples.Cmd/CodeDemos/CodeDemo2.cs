﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpLearning
{
    public class CSharp7
    {
        //dummy log methods
        void Log(string msg) { }

        //quick recap - usage of ref & out
        public void MethodWithRef(ref string str) { } //ref parameter may not be assigned inside, that argument must be initialized before call
        public void MethodWithOut(out string str) { str = "whatever"; } //out parameter must get assigned inside, that argument may not be initialized before

        public void OutRefBasic()
        {
            string str1 = "initial value";
            string str2;
            MethodWithRef(ref str1);
            MethodWithOut(out str2);
        }

        //1. Inline initialization of out parameter
        //out variables can be declared inline in the method call statement, with specific type or var. 
        public void OutInitializationDemo()
        {
            var isStringAnInteger = int.TryParse("0100", out int result); //This. out var result - also works
            if (isStringAnInteger)
                Log($"The number is {result}"); //the variable can be used normally as if it was declared before calling
            //if (int.TryParse("0100", out int result)) is a more cleaner way to write it
            //When used like this, the declaration "leaks" into outer scope of the if-statement
        }

        //2. Tuple enhancements
        //Tuples are light-weight data structures that can be declared/created inline and used to return multiple values
        //C# 7 makes tuples more of a first-class citizen with cleaner syntax, named-elements
        //This needs System.ValueTuple NuGet, which comes by default in .NET Framework 4.7, .NET Core 2.0, .NET Standard 2.0
        //Even though public methods can return tuple, it's recommended to be used with private/internal only
        public void TupleDemo()
        {
            //OLD syntax, still valid
            var tuple1 = Tuple.Create("str", 1); //this is a Tuple<string, int>
            var tuple2 = new Tuple<string, int>("str", 1);
            var str1 = tuple2.Item1; //elements are auto-named as Itemn, which cannot be changed

            //NEW syntax
            var tuple3 = ("str", 1); //Install System.ValueTuple NuGet, if required
            string item1 = tuple3.Item1; //Item1 since no name was given

            //Tuples with NAMED elements
            //IMportant - these names exist in compile time only. They cannot be fetched e.g. through Reflection in runtime
            (string Name, int Id) tuple4 = ("name", 1);
            var name = tuple4.Name;
            var tuple5 = (ObjeName: "yoyo", ObjId: 3);
            var id = tuple5.ObjId;

            //Something that might create confusion. This works, but shows warning.
            (string FirstName, string LastName) tuple6 = (Ignored1: "fname", Ignored2: "lname"); //shows green squiggly lines for warning!
            //here, the names "Ignored1" & "Ignored2" will be ignored as "FirstName" & "LastName" will get precedence as target type
            var firstName = tuple6.FirstName; //tuple6.Ignored1 does NOT exist

            //Something even less expeted!
            //DECONSTRUCTION of the tuple - see below [2.2]
            (string Name, int Id) = ("name", 1); //This is deconstruction, not really a tuple
            string copyOfName = Name; //DECONSTRUCTED variable declaration!
        }

        //2.1 Example of new syntax - method returning tuple
        public (char first, char last) GetEndCharacters(string input) //names of the return fields does not matter, they are just variable names
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"Parameter {nameof(input)} cannot be null or empty!");
            return (input.First(), input.Last());
        }

        //2.2 Tuple Deconstruction - directly assign tuple fileds to a set of variables
        //You can directly stores values of Tuple fields in separate variables by putting them inside parenthesis 
        public void TupleDeconstructionDemo()
        {
            var data = "some string data";
            (char first, char last) = GetEndCharacters(data);
            var message = $"The end characters in {data} are {first} and {last}";
            var (first1, last1) = GetEndCharacters(data); //works just like above
        }

        //DECONSTRUCTION - think of it someting opposite of construction. NOT DESTRUCTION, which wipes out the thing from memory!
        //Like CONSTRUCTOR assembles multiple pieces of data into one structure, DECONSTRUCTOR breaks a structure into multiple data elements :)

        //2.3 Deconstruction of user defined types
        //Needs to declare Deconstruct() method(s) with out parameters, for as many elments it needs to be deconstructed to
        public class CoolClass
        {
            public string Name { get; set; }
            public int Id { get; set; }

            public CoolClass(string name, int id) => (Name, Id) = (name, id); //Constructor //see [7] expression bodied fucntions enhancements

            public void Deconstruct(out string name, out int id) => (name, id) = (Name, Id); //Deconstructor

            ~CoolClass() { } //Destructor - For visual comparison only (Use in real code only to clean managed resources)
        }
        //With the Deconstructor() method present in a Type, it can be directly deconstructed (assigned to set of variables)
        public void UserDefinedTypeDeconstructorDemo()
        {
            var cool = new CoolClass("cool", 5);
            (string name, int id) = cool; //well, isn't it really cool? Basically it's being assigned to two variables
            var (coolName, coolId) = cool; //another approach for the same!
        }

        //3. Discards 
        //Discards are a variable (denoted with _) to receive all the throw-away-ble values that you do not care about.
        //Discards are write-only variable, they cannot be read from or used in any other way. They're basically a bin for collecting all unimportant values.
        //The _ variable does not need a definition and one variable can take many values, all at once. Mostly used in deconstruction, out variables etc.
        public (char fisrt, char last, int length, bool hasNumeric) GetDetails(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"Parameter {nameof(input)} cannot be null or empty!");
            return (input.First(), input.Last(), input.Length, input.Any(c => char.IsNumber(c)));
        }
        public void DiscardDemo()
        {
            var (_, _, length, _) = GetDetails("My string"); //Here I'm only interested in length, so I DISCARD other values
            var isTooLong = length > 100; //this works, obviously. BUT _ cannot be used anymore to retrieve values   
            //string something = _.ToString(); //DOES NOT work. _ is write-only & non-usable otherwise
            _ = isTooLong.ToString(); //This works fine. This is again reuse of discard
        }
        //If you have your own _ variable for real use, be careful not to confuse the two!
        public void DiscardAndLocalVariableDemo()
        {
            string _ = "my value"; //a local variable with name _ , this is not a discard
            var (_, _, length, _) = GetDetails("My string"); //these are discards
            var message = $"Value of local _ is : {_}"; //This works. Here _ is the local variable
        }

        //4. Pattern matching (or conditional algorithms, in my words :)
        //Pattern matching lets you write powerful, compact code that can work on different Type & values of data
        //At some level this is like overloading functions, but more Non-OO way. It's [1]Only one function [2]Does not need a class hierarchy
        //Basically you write (enhanced) IF & SWITCH statements that controls program flow based on "pattern" of data
        //In if-statement, pattern is checked with 'is', which works with both reference & value types
        //If the pattern matches, variables is cast and assigned to a new variable. Scope of the variable is only the associated code block.
        //pattern maching with if
        public static int PatternMatchingWithIf_Sum(IEnumerable<object> values)
        {
            var sum = 0;
            foreach (var item in values)
            {
                if (item is int val) //THIS IS BEING CAST AND REASSIGNED TO NEW VARIABLE, IF MATCH FOUND
                    sum += val;
                else if (item is IEnumerable<object> subList)
                    sum += PatternMatchingWithIf_Sum(subList);
            }
            return sum;
        }
        //pattern matching with switch-statement ('is' is implicit)
        //cases CANNOT fall through. return/break (or goto) must be present in each case.
        //ORDER of cases IS IMPORTANT. It has to be MORE SPECIFIC TO MORE GENERIC cases. Compiler checks this. Case default is always evaluated last.
        //Switch case can have addition checks with WHEN clause.
        //if no default: case is present and none of the cases match, program will simply continue after switch
        public static int PatternMatchingWithSwitch_Sum(IEnumerable<object> values)
        {
            var sum = 0;
            foreach (var item in values)
            {
                switch (item)
                {
                    case 0: //specific integer value
                        break;
                    case int val: //more general integer case //notice the variable re-assignment
                        sum += val;
                        break;
                    case CoolClass cool: //case of some other type //summation is hypothetical
                        sum += cool.Id;
                        break;
                    case IEnumerable<object> subList when subList.Any(): //specific enumrable WITH WHEN
                        sum += PatternMatchingWithSwitch_Sum(subList);
                        break;
                    case IEnumerable<object> subList: //more generic enumerable, we know it has no elements
                        break;
                    case null: //we handle null because we do not want to break on null
                        break;
                    default: //if none of the above matches, something's unexpected
                        throw new InvalidOperationException($"Unexpected type was passed to {nameof(PatternMatchingWithSwitch_Sum)} function");
                }
            }
            return sum;
        }

        //5. Ref locals and returns
        //This gives C# the ability to return reference to internal storage from a method, and store them in a variable and use in somewhat pass-by-reference way.
        //This does not involve [unsafe] code with bare pointers. The design and syntax saves developers from possible misuses.
        //The aim is to give a way to update values remotely (outside method) without the overhead of de-referencing, copying and obtaining a place holder.
        //Another way to think of 'ref locals' is as 'aliases', as if the variable and original items are one thing with two different names! (- Eric Lippert)
        //To use, simply use 'ref returnType' in declaration & 'ref return' from a method. Get the refernce outside with a ref local (e.g. ref int) variable.
        //Type of ref local (or by-reference variable) is REF <TYPE> e.g. ref int, VS inttelisense shows the vriable value, not memory address :\
        //Note: If the return is taken in a non-ref variable, simply the value will be copied to the variable. To work BOTH REF LOCAL & RETURN ARE REQUIRED.
        //First we'll see old/existing way of doing something, then we'll see the new way using ref return & local
        public (int x, int y) GetMaxValuePosition_Old(int[,] matrix)
        {
            var max = int.MinValue;
            var (cx, cy) = (0, 0);
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] > max)
                    {
                        (cx, cy) = (i, j);
                        max = matrix[i, j];
                    }
            return (cx, cy);
        }
        //using the old way
        public void UseMethod_Old()
        {
            int[,] array = new int[,] { { 1, 2, 3 }, { 3, 4, 5 }, { 5, 6, 7 }, { 7, 8, 9 } }; //a 3x4 matrix
            var (x, y) = GetMaxValuePosition_Old(array);
            //update value
            array[x, y] = int.MaxValue;
        }
        //NEW way using REF RETURN
        public ref int GetMaxValuePosition_New(int[,] matrix) //signature has changed
        {
            var max = int.MinValue;
            var (cx, cy) = (0, 0);
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] > max)
                    {
                        (cx, cy) = (i, j);
                        max = matrix[i, j];
                    }
            return ref matrix[cx, cy]; //return statement has changed
        }
        //NEW way of using this with REF LOCAL
        //The local ref variable is also called by-reference variable
        //NOTE: Ref local MUST be declared and initialized together - because ref local (memory address in a way) cannot be null
        //Note: Ref local & return does not work with async methods
        public void UseMethod_New()
        {
            int[,] array = new int[,] { { 1, 2, 3 }, { 3, 4, 5 }, { 5, 6, 7 }, { 7, 8, 9 } };
            //ref int element0; This doesn't work as ref locals must have initialization when declared
            ref var element = ref GetMaxValuePosition_New(array); //NOTICE the double ref - variable declaration & method call
            //update value
            element = int.MaxValue; //TYPE of element is REF INT //VS intellisense show value 9 (before update)
            //var element = GetMaxValuePosition_New(array); Wouldn't work this way, it'd simply copy the value
        }
        //NOTE: If reference type is returned by ref, then setting refLocal = new Stuff(); will actually update the original item!!

        //6. Local functions
        //This enables you to write functions inside functions. The inner function is available only to the enclosing function.
        //They might seem to be pretty similar to local lambda expressions, but there are bunch of differences - which makes it pretty powerful.
        //Generally a simpler syntax, better performance (no delegate, better memory handling), generics, simpler recursion, can use yield etc.
        public int SumJaggedArray(int[][] data)
        {
            var total = 0;

            //Obviously we could do using LINQ
            total = data.Sum(arr => arr.Sum());
            total = data.SelectMany(arr => arr).Sum();
            //or could have used lambda in place of local function
            Func<int[], int> summation = (arr) => arr.Sum();

            //BUT here we are learning LOCAL FUNCTION anyway, so
            for (int i = 0; i < data.Length; i++)
                total += sum(data[i]);
            return total; //Note: the function can actually be defined after return statement

            int sum(int[] arr) //this is a LOCAL FUNCTION //Note: there cannot be a access modifier
            {
                int _sum = 0;
                for (int i = 0; i < arr.Length; i++)
                    _sum += arr[i];
                return _sum;
            }
        }

        //7. More use of expression-bodied functions
        //In C# 6 it was allowed only in functions & read-only properties, now they are allowed in constructor, finalizers, get-set, indexer
        //See the constructor of CoolClass in section [2.3] for an example

        //8. Throw expression
        //throw is now an expression rather than a statement what it used to be - so they can go with other expressions!
        public void SumJaggedArray_with_ThrowAsExpression(int[][] data) 
        {
            Func<int[], int> summation = 
                (arr) => arr == null ?
                    throw new ArgumentException("null not allowed") : //throw in lambda
                    arr.Sum();

            var total = 0;
            for (int i = 0; i < data.Length; i++)
                total += 
                    data[i] == null ? 
                    throw new ArgumentException("null not allowed") : //throw in conditional expressions
                    summation(data[i]);
        }

        //9. Generic async return
        //Before C# 7, async methods could return only void, Task or Task<T>. C# 7 allowes return of other types which follow the asynchronous pattern
        //One that is provided by Framework is ValueTask<T> which is a struct (value type), and can improve performance in some cases.
        public async ValueTask<T> GenericAsyncReturn<T>() //You may need to install NuGet System.Threading.Tasks.Extensions 
        {
            await Task.Delay(1000);
            return default(T);
        }

        //10. Syntax improvements for numeric literals
        //All for readability - binary representation of numbers & digit separators
        //10.1 Binary numbers - numbers can be written as pure binary wherever it makes sense, with a 0b prefix
        //10.2 Digit separation with _ (underscore). Applies to binary, integer, float, double, decimal - can be placed between any digits
        public void NumericLiteralDemo()
        {
            int asBinary = 0b0001;
            var eight = 0b1000; //its still just 8, intellisense also shows simply 8
            var x = 10 - eight; //produces 2

            var sixteen = 0b001_0000;
            //var seventeen = 0b_0001_0001; //Not supported YET

            int million = 1_000_000;
            var thousand = 1___0_0__0; //is still valid :O

            var _double = 1_9.00_20_00_90_5;
            var _decimal = 12.000_05m;
        }
    }
}

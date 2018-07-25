namespace Dorkari.Samples.Cmd.Tests
{
    public delegate string Stringer(int data);

    class MyDelegates
    {
        string IntoToSTring(int blah)
        {
            return blah.ToString();
        }

        void DelegateNew() //C# 1+
        {
            var stringer = new Stringer(IntoToSTring);
            string five = stringer(5);
        }

        void DelegateAssign() //C# 2+
        {
            Stringer stringer = IntoToSTring;
            string five = stringer(5);
        }

        void DelegateAnonymous() //C# 3+
        {
            Stringer stringer = delegate (int i) { return i.ToString(); };
            string five = stringer(5);
        }

        void DelegateLambda() //C# 3+
        {
            Stringer stringer = (i) => i.ToString();
            string five = stringer(5);
        }
    }
}

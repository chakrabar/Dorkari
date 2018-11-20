namespace Dorkari.Samples.Cmd.Tests
{
    class BoolCondition
    {
        int TestBool(bool isGood) //these all are compiled to !isGood
        {
            int blah = 0;
            if (isGood != true)
            {
                blah++;
            }
            if (!(isGood == true))
            {
                blah += 2;
            }
            if (!isGood)
            {
                blah -= 3;
            }
            return blah;
        }

        int TestString(string name) //these all are compiled differently (as in code)
        {
            int blah = 0;
            string defaultName = "whatever";
            if (name != defaultName)
            {
                blah++;
            }
            if (!(name == defaultName))
            {
                blah += 2;
            }
            if (!name.Equals(defaultName))
            {
                blah -= 3;
            }
            return blah;
        }
    }
}

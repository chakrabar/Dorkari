using System;
using System.Collections.Generic;

namespace Dorkari.Framework.Web.TestUtils
{
    internal static class TestData
    {
        static List<Robot> _robots;

        internal static List<Robot> GetRobots()
        {
            if (_robots == null)
            {
                _robots = new List<Robot> {
                    new Robot { Id = 1, Name = "Humanoid", MfdDate = DateTime.Parse("12/31/2015") },
                    new Robot { Id = 2, Name = "ACPC", MfdDate = DateTime.Parse("01/01/2010") }
                };
            }
            return _robots;
        }
    }
}
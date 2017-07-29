using Dorkari.Framework.Web.TestUtils;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dorkari.Framework.Web.Controllers
{
    public class TestController : ApiController
    {
        // ../api/test
        public IEnumerable<Robot> Get()
        {
            return TestData.GetRobots();
        }

        // ../api/test/2
        public Human Get(int id)
        {
            return new Human { Name = "Arghya C", Age = 30 };
        }

        // ../api/test?name=hombre
        public string Get(string name)
        {
            return "Hi " + name;
        }
    }
}

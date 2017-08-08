using Dorkari.Framework.Web.Communicators;
using Dorkari.Framework.Web.Models.Enums;
using Dorkari.Framework.Web.TestUtils;
using System.Collections.Generic;
using System.Diagnostics;
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

        // ../api/test?name=hombre&format=xml
        public string Get(string name, MediaType format)
        {
            var currentBaseUri = Request.RequestUri.GetLeftPart(System.UriPartial.Authority);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = RestClient.Get<IEnumerable<Robot>>(currentBaseUri + "/api/test", format);
            watch.Stop();
            var milliSecondsTaken = watch.ElapsedMilliseconds;
            return string.Format("Hi {0}, your request with Type: {1} took {2} milliseconds, content length: {3}", name, format, milliSecondsTaken, "XXX");
        }
    }
}

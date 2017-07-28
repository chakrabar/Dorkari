using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dorkari.Framework.Web.Controllers
{
    public class TestController : ApiController
    {
        public IEnumerable<Robot> Get()
        {
            return new List<Robot> {
                new Robot { Id = 1, Name = "Humanoid", MfdDate = DateTime.Parse("12/31/2015") },
                new Robot { Id = 2, Name = "ACPC", MfdDate = DateTime.Parse("01/01/2010") }
            };
        }

        public Human Get(int id)
        {
            return new Human { Name = "Arghya C", Age = 30 };
        }

        public string Get(string name)
        {
            return "Hi " + name;
        }
    }

    [ProtoContract]
    public class Robot
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public DateTime MfdDate { get; set; }
    }

    public class Human
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

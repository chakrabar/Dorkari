using ProtoBuf;
using System;

namespace Dorkari.Framework.Web.TestUtils
{
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
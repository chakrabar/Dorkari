using System;

namespace Dorkari.Framework.Web.Models.Enums
{
    public enum MediaType
    {
        [AcceptHeader("application/json")]
        JSON = 0,
        [AcceptHeader("application/xml")]
        XML,
        [AcceptHeader("application/protobuf")]
        ProtoBuf
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AcceptHeaderAttribute : Attribute
    {
        private string _acceptType;

        public AcceptHeaderAttribute(string value)
        {
            _acceptType = value;
        }

        public string Value
        {
            get { return _acceptType; }
        }
    }
}
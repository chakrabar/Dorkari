using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Dorkari.Framework.Web.MediaTypeFormatters
{
    public class ProtobufMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        public ProtobufMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/protobuf"));
        }

        public override bool CanReadType(Type type)
        {
            return IsTargetTypeProtobufEnabled(type);
        }

        public override bool CanWriteType(Type type)
        {
            return IsTargetTypeProtobufEnabled(type);
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            Serializer.Serialize(writeStream, value);
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Serializer.NonGeneric.Deserialize(type, readStream);
        }

        private bool IsTargetTypeProtobufEnabled(Type type)
        {
            if (type.IsPrimitive ||
                new Type[] { 
				    typeof(String),
				    typeof(Decimal),
				    typeof(DateTime),
				    typeof(DayOfWeek),
				    typeof(Guid)
			    }.Contains(type))
                return true;
            Type targetType = type;
            if ((typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType) //is IEnumerable<t>
                || type.GetType().GetInterfaces().Any(t => t.IsGenericType == true && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))) //is derived from IEnumerable<T>
            {
                targetType = type.GetGenericArguments()[0];
            }
            return Attribute.GetCustomAttribute(targetType, typeof(ProtoContractAttribute)) != null;
        }
    }
}
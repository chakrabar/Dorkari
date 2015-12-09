using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dorkari.Helpers.Core.Xml
{
    public static class XmlLinqExtensions
    {
        public static XElement GetElementByLocalName(this XElement xe, string nodeName)
        {
            return xe == null ? null : xe.Elements().Where(e => e.Name.LocalName == nodeName).FirstOrDefault();
        }

        public static IEnumerable<XElement> GetDescendantsByLocalName(this XContainer xc, string nodeName) //TODO: check
        {
            return xc == null ? null : xc.Descendants().Where(d => d.Name.LocalName == nodeName);
        }

        public static string ToNullSafeString(this XElement xe)
        {
            return xe == null ? string.Empty : xe.ToString();
        }

        public static string GetStringValue(this XElement xe)
        {
            return xe == null ? string.Empty : xe.Value;
        }

        public static string GetNullOrStringValue(this XElement xe)
        {
            return xe == null ? null : xe.Value;
        }

        public static double GetDoubleValue(this XElement xe)
        {
            var value = xe == null ? string.Empty : xe.Value;
            double doubleValue = 0;
            double.TryParse(value, out doubleValue);
            return doubleValue;
        }

        public static int GetIntegerValue(this XElement xe)
        {
            var value = xe == null ? string.Empty : xe.Value;
            int intValue = 0;
            int.TryParse(value, out intValue);
            return intValue;
        }

        public static DateTime ToDateTime(this XElement xe)
        {
            var value = xe == null ? string.Empty : xe.Value;
            DateTime date;
            return DateTime.TryParse(value, out date) ? date : DateTime.MinValue;
        }
    }
}

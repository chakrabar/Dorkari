using System.Collections.Generic;
using System.Dynamic;

namespace Dorkari.Helpers.Core.Utilities
{
    public class DynamicHelper
    {
        public static ExpandoObject CreateExpando(IEnumerable<KeyValuePair<string, object>> propertyValues)
        {
            IDictionary<string, object> dynamicObject = new ExpandoObject();
            foreach (var item in propertyValues)
            {
                dynamicObject[item.Key] = item.Value;
            }
            return (ExpandoObject)dynamicObject;
        }
    }
}

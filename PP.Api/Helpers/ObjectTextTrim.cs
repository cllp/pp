using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Helpers
{
    public static class ObjectTextTrim
    {
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    if (stringProperty.SetMethod != null)
                        stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            return input;
        }
    }
}
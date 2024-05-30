using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace UI.Extensions
{
    public static class EnumDescriptionExtension
    {        public static string GetEnumDescription(this Enum enumValue)
        {
            var x = enumValue.GetType().GetField(enumValue.ToString());
            if(x == null)
                return string.Empty;
            else if(Attribute.GetCustomAttribute(x, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                return attribute.Description;
            
            throw new ArgumentException("Item not found.", nameof(enumValue));
        }

        public static T GetEnumValueByDescription<T>(this string description) where T : Enum
        {
            foreach(Enum item in Enum.GetValues(typeof(T)))
                if(item.GetEnumDescription() == description)
                    return (T)item;
            throw new ArgumentException("Item not found.", nameof(description));   
        }
    }
}
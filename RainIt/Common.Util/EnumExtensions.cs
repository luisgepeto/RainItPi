using System;
using System.ComponentModel;


namespace Common.Util
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return (descAttribute == null) ? value.ToString() : descAttribute.Description;
        }
    }
}

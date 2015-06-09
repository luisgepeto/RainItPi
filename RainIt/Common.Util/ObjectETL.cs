

using System;
using System.Reflection;

namespace Common.Util
{
    public static class ObjectETL
    {
        public static TTarget ConvertTo<TSource, TTarget>(this TSource sourceObject, TTarget targetObject)
        {
            var targetObjectProperties = targetObject.GetType().GetProperties();
            foreach (var property in targetObjectProperties)
            {
                var propertyName = property.Name;
                if (!IsPropertyMappable(property) || !CanMapProperty(sourceObject, propertyName)) continue;

                var propertyValue = sourceObject.GetType().GetProperty(propertyName).GetValue(sourceObject);
                targetObject.GetType().GetProperty(propertyName).SetValue(targetObject, propertyValue);
            }
            return targetObject;
        }

        private static bool IsPropertyMappable(PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            return propertyType.IsPrimitive ||
                   propertyType == typeof (Boolean) ||
                   propertyType == typeof (Byte) ||
                   propertyType == typeof (SByte) ||
                   propertyType == typeof (Int16) ||
                   propertyType == typeof (UInt16) ||
                   propertyType == typeof (Int32) ||
                   propertyType == typeof (UInt32) ||
                   propertyType == typeof (Int64) ||
                   propertyType == typeof (UInt64) ||
                   propertyType == typeof (UIntPtr) ||
                   propertyType == typeof (IntPtr) ||
                   propertyType == typeof (Char) ||
                   propertyType == typeof (Double) ||
                   propertyType == typeof (Single) ||
                   propertyType == typeof (DateTime) ||
                   propertyType == typeof (Decimal) ||
                   propertyType == typeof (String);
        }

        public static bool CanMapProperty<TSource>(TSource someObject, string propertyName)
        {
            var property =  someObject.GetType().GetProperty(propertyName);
            return property != null && IsPropertyMappable(property);
        }
    }
}

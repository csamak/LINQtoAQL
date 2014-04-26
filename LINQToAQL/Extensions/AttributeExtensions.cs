using System;
using System.Linq;
using System.Reflection;

namespace LINQToAQL.Extensions
{
    internal static class AttributeExtensions
    {
        /// <remarks>See http://stackoverflow.com/questions/2656189 </remarks>
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof (TAttribute), true).FirstOrDefault() as TAttribute;
            return att != null ? valueSelector(att) : default(TValue);
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this MemberInfo type,
            Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return att != null ? valueSelector(att) : default(TValue);
        }
    }
}
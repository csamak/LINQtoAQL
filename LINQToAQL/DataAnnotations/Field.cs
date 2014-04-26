using System;

namespace LINQToAQL.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
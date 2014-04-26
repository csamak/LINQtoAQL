using System;

namespace LINQToAQL.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Field : Attribute
    {
        public string Name { get; set; }
    }
}
using System;

namespace LINQToAQL.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasetAttribute : Attribute
    {
        public DatasetAttribute(string name)
        {
            Name = name;
            Open = false;
        }

        public string Name { get; set; }
        public bool Open { get; set; }
    }
}
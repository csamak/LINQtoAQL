using System;

namespace LINQToAQL.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Dataset : Attribute
    {
        public Dataset(string name)
        {
            Open = false;
        }

        public string Name { get; set; }
        public bool Open { get; set; }
    }
}
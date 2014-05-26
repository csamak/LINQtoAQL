using System;

namespace LINQToAQL.DataAnnotations
{
    /// <summary>
    ///     An <see cref="Attribute"/> that describes how to handle a field when generating AQL
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasetAttribute : Attribute
    {
        /// <summary>
        ///     The dataset name to use when generating the AQL query
        /// </summary>
        /// <returns>The dataset name</returns>
        public string Name { get; set; }

        /// <summary>
        ///     Whether the type associated with the dataset is an open type
        /// </summary>
        /// <returns>Whether the type is open</returns>
        public bool Open { get; set; }
    }
}
using System;

namespace LINQToAQL.DataAnnotations
{
    /// <summary>
    ///     An <see cref="Attribute"/> that describes how to handle a field when generating AQL
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        ///     The field name to use when generating the AQL query
        /// </summary>
        /// <returns>The dataset name</returns>
        public string Name { get; set; }
    }
}
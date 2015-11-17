using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL.DataAnnotations
{
    /// <summary>
    ///     An <see cref="Attribute"/> that describes how to handle a dataverse when generating AQL
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataverseAttribute : Attribute
    {
        /// <summary>
        ///     The dataverse name to use when generating an AQL query
        /// </summary>
        public string Name { get; set; }
    }
}

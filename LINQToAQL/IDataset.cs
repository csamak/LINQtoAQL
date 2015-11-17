using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL
{
    /// <summary>
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //This interface allows consuming code to use a Dataset<T> without including a direct dependency on Remotion.Linq
    public interface IDataset<out T> : IQueryable<T>
    {
        /// <summary>
        ///     The <see cref="AsterixContext"/> associated with the <see cref="IDataset{T}"/>
        /// </summary>
        AsterixContext Context { get; }
    }
}

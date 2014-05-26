using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL.Similarity
{
    public static class JaccardExtensions
    {
        public static double Jaccard<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            throw new AsterixRemoteOnlyException();
        }

        //should this be a Tuple? How would it be used elsewhere in the query then?
        public static object[] JaccardCheck<T>(this IEnumerable<T> first, IEnumerable<T> second, double threshold)
        {
            throw new AsterixRemoteOnlyException();
        }
    }
}

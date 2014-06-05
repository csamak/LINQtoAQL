using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQToAQL.Similarity;
using LINQToAQL.Spatial;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    class Spatial : QueryBuildingBase
    {
        [Test]
        public void EditDistance()
        {
            var query = dv.FacebookUsers.Select(u => new Point(u.id, 3.2));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers return create-point($u.id, 3.2)",
                GetQueryString(query.Expression));
        }
    }
}

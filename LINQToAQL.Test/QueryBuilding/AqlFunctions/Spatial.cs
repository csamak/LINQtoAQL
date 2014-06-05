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
        public void CreatePoint()
        {
            var query = dv.FacebookUsers.Select(u => new Point(u.id, 3.2));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers return create-point($u.id, 3.2)",
                GetQueryString(query.Expression));
        }

        [Test]
        public void CreateLine()
        {
            //todo: offline point
            var query = dv.FacebookUsers.Select(u => new Line(new Point(u.id, 1.9), new Point(2.4, u.id)));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers return create-line(create-point($u.id, 1.9), create-point(2.4, $u.id))",
                GetQueryString(query.Expression));
        }
    }
}

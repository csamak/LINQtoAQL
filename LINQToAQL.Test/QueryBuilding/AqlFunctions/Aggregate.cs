using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    class Aggregate : QueryBuildingBase
    {
        [Test]
        public void Count()
        {
            Expression<Func<int>> query = (() => dv.FacebookMessages.Count());
            Assert.AreEqual("count(for $generated_1 in dataset FacebookMessages return $generated_1)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Avg()
        {
            Expression<Func<double>> query = (() => dv.FacebookMessages.Select(m => m.Id).Average());
            Assert.AreEqual("avg(for $m in dataset FacebookMessages return $m.message-id)",
                GetQueryString(query.Body));
        }
    }
}

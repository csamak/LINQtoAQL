using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class Aggregate : QueryBuildingBase
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
            Expression<Func<double>> query = (() => dv.FacebookMessages.Average(m => m.Id));
            Assert.AreEqual("avg(for $m in dataset FacebookMessages return $m.message-id)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Sum()
        {
            Expression<Func<int>> query = (() => dv.TwitterUsers.Sum(i => i.friends_count));
            Assert.AreEqual("sum(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Max()
        {
            Expression<Func<int>> query = (() => dv.TwitterUsers.Max(i => i.friends_count));
            Assert.AreEqual("max(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Min()
        {
            Expression<Func<int>> query = (() => dv.TwitterUsers.Min(i => i.friends_count));
            Assert.AreEqual("min(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }
    }
}
using System;
using System.Linq;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class Numeric : QueryBuildingBase
    {
        [Test]
        public void NumericAbs()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => Math.Abs(u.id) > 32);
            Assert.AreEqual("for $u in dataset FacebookUsers where (numeric-abs($u.id) > 32) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void NumericCeiling()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => Math.Ceiling((decimal) u.id) > 32);
            Assert.AreEqual("for $u in dataset FacebookUsers where (numeric-ceiling($u.id) > 32) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void NumericFloor()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => Math.Floor((decimal) u.id) > 32);
            Assert.AreEqual("for $u in dataset FacebookUsers where (numeric-floor($u.id) > 32) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void NumericRoundHalfToEven()
        {
            IQueryable<FacebookUser> query =
                dv.FacebookUsers.Where(u => Math.Round((decimal) u.id, MidpointRounding.ToEven) > 32);
            Assert.AreEqual(GetQueryString(query.Expression),
                GetQueryString(
                    dv.FacebookUsers.Where(u => Math.Round((decimal) u.id) > 32).Expression));
            Assert.AreEqual("for $u in dataset FacebookUsers where (numeric-round-half-to-even($u.id) > 32) return $u",
                GetQueryString(query.Expression));
        }
    }
}
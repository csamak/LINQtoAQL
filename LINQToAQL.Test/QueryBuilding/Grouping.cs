using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding
{
    internal class Grouping : QueryBuildingBase
    {
        [Test]
        public void SingleReturnKey()
        {
            IQueryable<int> query = from u in dv.FacebookUsers
                group u by u.id
                into g
                select g.Key;
            Assert.AreEqual(
                "for $g in (for $u in dataset FacebookUsers group by $u.id with $u return $u) return $g[0].id",
                GetQueryString(query.Expression));
        }

        [Test]
        public void ReturnCount()
        {
            IQueryable<int> query = from u in dv.FacebookUsers
                group u by u.id
                into g
                select g.Count();
            Assert.AreEqual(
                "for $g in (for $u in dataset FacebookUsers group by $u.id with $u return $u) return count((for $generated_uservar_1 in $g return $generated_uservar_1))",
                GetQueryString(query.Expression));
        }
    }
}
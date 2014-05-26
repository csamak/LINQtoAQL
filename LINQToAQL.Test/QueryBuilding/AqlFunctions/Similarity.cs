using System;
using System.Linq;
using LINQToAQL.Similarity;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class Similarity : QueryBuildingBase
    {
        [Test]
        public void EditDistance()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => u.name.EditDistance("Suzanna Tilson") <= 2);
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where (edit-distance($u.name, \"Suzanna Tilson\") <= 2) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void EditDistanceCheck()
        {
            var query = dv.FacebookUsers.Where(u => u.name.EditDistanceCheck("Suzanna Tilson", 2));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where edit-distance-check($u.name, \"Suzanna Tilson\", 2) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void EditDistanceOnlyRemote()
        {
            //AsterixRemoteOnlyException is internal, so we can't use Assert.Throws<AsterixRemoteOnlyException>
            try
            {
                "".EditDistance("hi");
            }
            catch (Exception ex)
            {
                Assert.True(ex.GetType().Name == "AsterixRemoteOnlyException");
                return;
            }
            Assert.Fail("AsterixRemoteOnlyException has not been thrown.");
        }
    }
}
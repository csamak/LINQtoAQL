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
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => u.FriendIds.EditDistance(new[] {1, 5, 9}) <= 2);
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where (edit-distance($u.friend-ids, [1,5,9]) <= 2) return $u",
                GetQueryString(query.Expression));
            OnlyRemote(() => "".EditDistance("other"));
        }

        [Test]
        public void EditDistanceCheck()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => u.name.EditDistanceCheck("Suzanna Tilson", 2));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where edit-distance-check($u.name, \"Suzanna Tilson\", 2) return $u",
                GetQueryString(query.Expression));
            OnlyRemote(() => "".EditDistanceCheck("other", 32));
        }

        [Test]
        public void Jaccard()
        {
            IQueryable<FacebookUser> query = dv.FacebookUsers.Where(u => u.FriendIds.Jaccard(new[] {1, 5, 9}) >= 0.6);
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where (similarity-jaccard($u.friend-ids, [1,5,9]) >= 0.6) return $u",
                GetQueryString(query.Expression));
        }

        [Test]
        public void JaccardCheck()
        {
            //note how let is handled
            IQueryable<object> query = from u in dv.FacebookUsers
                let sim = u.FriendIds.JaccardCheck(new[] {1, 5, 9}, 0.6)
                where (bool) sim[0]
                select sim[1];
            Assert.AreEqual(
                "for $u in dataset FacebookUsers where similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[0] return similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[1]",
                GetQueryString(query.Expression));
        }

        private static void OnlyRemote(Action x)
        {
            //AsterixRemoteOnlyException is internal, so we can't use Assert.Throws<AsterixRemoteOnlyException>
            try
            {
                x();
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
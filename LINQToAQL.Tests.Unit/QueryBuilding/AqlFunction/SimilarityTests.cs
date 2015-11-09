using System;
using LINQToAQL.Similarity;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding.AqlFunction
{
    internal class SimilarityTests : QueryBuildingBase
    {
        [Test]
        public void OnlyAllowsRemoteExecution()
        {
            OnlyRemote(() => "".EditDistance("other"));
            OnlyRemote(() => "".EditDistanceCheck("other", 32));
            OnlyRemote(() => new[] {1, 5, 9}.Jaccard(new [] {1, 7, 9}));
            OnlyRemote(() => new[] {1, 5, 9}.JaccardCheck(new [] {1, 7, 9}, 0.6));
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
using System;
using System.Linq;
using LINQToAQL.Tests.Common.Model;
using NUnit.Framework;

namespace LINQToAQL.Test
{
    internal class AsterixDbIntegration
    {
        private readonly TinySocial _dv = new TinySocial(new Uri("http://localhost:19002"));

        [Test]
        public void SingleString()
        {
            CollectionAssert.AreEquivalent(from user in _dv.FacebookUsers where user.id == 8 select user.name, new[] { "NilaMilliron" });
        }

        [Test]
        public void MultipleStrings()
        {
            var res = from user in _dv.FacebookUsers select user.name;
            CollectionAssert.AreEquivalent(res,
                new[]
                {
                    "MargaritaStoddard", "IsbelDull", "EmoryUnk", "NicholasStroh", "VonKemble", "WillisWynne",
                    "SuzannaTillson", "NilaMilliron", "WoodrowNehling", "BramHatch"
                });
        }

        [Test]
        public void CountScalar()
        {
            //TODO: support LongCount()
            Assert.AreEqual(_dv.FacebookUsers.Count(), 10);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test
{
    internal class AsterixDbIntegration
    {
        private readonly TinySocial _dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        [Test]
        public void SingleResultDatetime()
        {
            List<FacebookUser> res = (from user in _dv.FacebookUsers where user.id == 8 select user).ToList();
            Assert.AreEqual(1, res.Count);
        }

        [Test]
        public void SimpleTypes()
        {
            var res = (from user in _dv.FacebookUsers
                select new
                {
                    uname = user.name,
                    //messages = (from message in dv.FacebookMessages where message.AuthorId == user.id select message.Message)
                }).ToList();
            Assert.AreEqual(1, res.Count);
        }
    }
}
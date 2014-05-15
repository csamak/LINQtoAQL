using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test
{
    class AsterixDbIntegration
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        [Test]
        public void SingleResultDatetime()
        {
            var res = (from user in dv.FacebookUsers where user.id == 8 select user).ToList();
            Assert.AreEqual(1, res.Count);
        }

        [Test]
        public void SimpleTypes()
        {
            var res = (from user in dv.FacebookUsers
                select new
                {
                    uname = user.name,
                    //messages = (from message in dv.FacebookMessages where message.AuthorId == user.id select message.Message)
                }).ToList();
            Assert.AreEqual(1, res.Count);
        }
    }
}

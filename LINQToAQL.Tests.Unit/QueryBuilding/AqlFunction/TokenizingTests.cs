using System;
using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding.AqlFunction
{
    internal class TokenizingTests : QueryBuildingBase
    {
        [Test]
        public void WordTokensRemoveEmptyEntries()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }

        [Test]
        public void WordTokensWrongSeparator()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new [] {"wrong"}, StringSplitOptions.None));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
            query = dv.FacebookUsers.Select(u => u.name.Split('x'));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }
    }
}
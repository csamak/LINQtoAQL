using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding.AqlFunction
{
    internal class StringTests : QueryBuildingBase
    {
        //Cannot be included with other tests since char is not a subclass of object.
        [Test, Category("StringQuerySet")]
        public void StringToCodepointSingleChar()
        {
            IQueryable<char> indexer = dv.FacebookUsers.Select(u => u.name[0]);
            Assert.AreEqual("for $u in dataset FacebookUsers return string-to-codepoint($u.name)[0]",
                GetQueryString(indexer.Expression));
        }

        //TODO: Aql like
        //TODO: string-concat
        //TODO: matches
        //TODO: replace
        //TODO: substring-before
        //TODO: substring-after
    }
}
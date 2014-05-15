using System;
using System.Linq;

namespace LINQToAQL.Test.Model
{
    class TinySocial
    {
        public TinySocial(Uri baseUri)
        {
            FacebookMessages = new AqlQueryable<FacebookMessage>(baseUri, "TinySocial");
            FacebookUsers = new AqlQueryable<FacebookUser>(baseUri, "TinySocial");
        }
        public IQueryable<FacebookMessage> FacebookMessages { get; private set; }
        public IQueryable<FacebookUser> FacebookUsers { get; private set; }
    }
}

using System;
using System.Linq;

namespace LINQToAQL.Tests.Common.Model
{
    public class TinySocial
    {
        public TinySocial(Uri baseUri)
        {
            FacebookMessages = new AqlQueryable<FacebookMessage>(baseUri, "TinySocial");
            FacebookUsers = new AqlQueryable<FacebookUser>(baseUri, "TinySocial");
            TweetMessages = new AqlQueryable<TweetMessage>(baseUri, "TinySocial");
            TwitterUsers = new AqlQueryable<TwitterUser>(baseUri, "TinySocial");
        }
        public IQueryable<FacebookMessage> FacebookMessages { get; private set; }
        public IQueryable<FacebookUser> FacebookUsers { get; private set; }
        public IQueryable<TweetMessage> TweetMessages { get; private set; }
        public IQueryable<TwitterUser> TwitterUsers { get; private set; }
    }
}

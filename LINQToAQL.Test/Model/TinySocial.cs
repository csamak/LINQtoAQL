using System.Linq;

namespace LINQToAQL.Test.Model
{
    class TinySocial
    {
        public TinySocial(string conString)
        {
            FacebookMessages = new AqlQueryable<FacebookMessage>(conString);
            FacebookUsers = new AqlQueryable<FacebookUser>(conString);
        }
        public IQueryable<FacebookMessage> FacebookMessages { get; private set; }
        public IQueryable<FacebookUser> FacebookUsers { get; private set; }
    }
}

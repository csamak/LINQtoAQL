using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Test.Annotations;

namespace LINQToAQL.Test.Model
{
    [Dataset("FacebookUsers"), UsedImplicitly]
    internal class FacebookUser
    {
        public FacebookUser(string conString)
        {
            employment = new AqlQueryable<Employment>(conString);
        }

        public int id { get; set; }
        public string alias { get; set; }
        public string name { get; set; }

        [Field(Name = "user-since")]
        public DateTime UserSince { get; set; }

        [Field(Name = "friend-ids")]
        public HashSet<int> FriendIds { get; set; } //dupes allowed?
        public IQueryable<Employment> employment { get; set; } //ordering?
    }
}
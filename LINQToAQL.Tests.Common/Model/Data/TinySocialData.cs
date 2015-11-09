using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Tests.Common.Properties;
using Newtonsoft.Json;

namespace LINQToAQL.Tests.Common.Model.Data
{
    internal static class TinySocialData
    {
        public static IEnumerable<FacebookMessage> FacebookMessages { get; } =
            JsonConvert.DeserializeObject<IEnumerable<FacebookMessage>>(Resources.FacebookMessages);

        public static IEnumerable<FacebookUser> FacebookUsers { get; } =
            JsonConvert.DeserializeObject<IEnumerable<FacebookUser>>(Resources.FacebookUsers);

        //enforces order, which is currently required by the tests.
        public static IEnumerable<FacebookUser> FacebookUsersByIds(params int[] ids)
        {
            return ids.Select(id => FacebookUsers.Single(u => u.id == id));
        }

        public static IEnumerable<FacebookMessage> FacebookMessagesByIds(params int[] ids)
        {
            return ids.Select(id => FacebookMessages.Single(u => u.Id == id));
        }
    }
}
using LINQToAQL.DataAnnotations;

namespace LINQToAQL.Test.Model
{
    [Dataset("TwitterUsers", Open = true)]
    internal class TwitterUser
    {
        [Field(Name = "screen-name")]
        public string ScreenName { get; set; }
        public string lang { get; set; }
        public int friends_count { get; set; }
        public int statuses_count { get; set; }
        public string name { get; set; }
        public int followers_count { get; set; }
    }
}

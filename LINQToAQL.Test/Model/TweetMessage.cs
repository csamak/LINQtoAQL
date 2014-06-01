using System;
using LINQToAQL.DataAnnotations;

namespace LINQToAQL.Test.Model
{
    [Dataset(Name = "TweetMessages")]
    internal class TweetMessage
    {
        public string tweetid { get; set; }
        public TwitterUser user { get; set; }
        //sender-location: point?
        [Field(Name = "send-time")]
        public DateTime SendTime { get; set; }

        //referred-topics: {{ string }}
        [Field(Name = "message-text")]
        public string MessageText { get; set; }
    }
}
using System;
using System.Collections.Generic;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Spatial;

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "TweetMessages")]
    public class TweetMessage
    {
        public string tweetid { get; set; }
        public TwitterUser user { get; set; }
        [Field(Name = "sender-location")]
        public Point SenderLocation { get; set; }
        [Field(Name = "send-time")]
        public DateTime SendTime { get; set; }

        //referred-topics: {{ string }}
        //list the right type?
        [Field(Name = "referred-topics")]
        public List<string> ReferredTopics { get; set; }
        [Field(Name = "message-text")]
        public string MessageText { get; set; }
    }
}
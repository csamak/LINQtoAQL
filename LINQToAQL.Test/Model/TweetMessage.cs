using System;
using System.Collections.Generic;
using LINQToAQL.DataAnnotations;
using NUnit.Framework;

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
        //list the right type?
        [Field(Name = "referred-topics")]
        public List<string> ReferredTopics { get; set; }
        [Field(Name = "message-text")]
        public string MessageText { get; set; }
    }
}
using LINQToAQL.DataAnnotations;
using LINQToAQL.Spatial;

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "FacebookMessages", Open = false)]
    public class FacebookMessage
    {
        [Field(Name = "message-id")]
        public int Id { get; set; }

        [Field(Name = "author-id")]
        public int? AuthorId { get; set; }

        [Field(Name = "in-response-to")]
        public int? InResponseTo { get; set; }

        [Field(Name = "sender-location")]
        public Point SenderLocation { get; set; } //not so sure about GeoCoordinate

        [Field(Name = "message")]
        public string Message { get; set; }
    }
}
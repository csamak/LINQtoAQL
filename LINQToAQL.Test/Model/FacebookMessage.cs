using System.Device.Location;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Test.Annotations;

namespace LINQToAQL.Test.Model
{
    [Dataset("FacebookMessages", Open = false), UsedImplicitly]
    internal class FacebookMessage
    {
        [Field(Name = "message-id")]
        public int Id { get; set; }

        [Field(Name = "author-id")]
        public int? AuthorId { get; set; }

        [Field(Name = "in-response-to")]
        public int? InResponseTo { get; set; }

        [Field(Name = "sender-location")]
        public GeoCoordinate SenderLocation { get; set; } //not so sure about GeoCoordinate

        [Field(Name = "message")]
        public string Message { get; set; }
    }
}
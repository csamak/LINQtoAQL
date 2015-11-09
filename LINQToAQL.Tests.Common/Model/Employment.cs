using System;
using LINQToAQL.DataAnnotations;

namespace LINQToAQL.Tests.Common.Model
{
    //Type - open
    public class Employment
    {
        [Field(Name = "organization-name")]
        public string OrganizationName { get; set; }

        [Field(Name = "start-date")]
        public DateTime StartDate { get; set; }

        [Field(Name = "end-date")]
        public DateTime? EndDate { get; set; }
    }
}
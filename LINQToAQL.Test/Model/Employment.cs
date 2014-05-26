using System;
using LINQToAQL.DataAnnotations;

namespace LINQToAQL.Test.Model
{
    //Type - open
    internal class Employment
    {
        [Field(Name = "organization-name")]
        public string OrganizationName { get; set; }

        [Field(Name = "start-date")]
        public DateTime StartDate { get; set; }

        [Field(Name = "end-date")]
        public DateTime? EndDate { get; set; }
    }
}
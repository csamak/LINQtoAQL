using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQToAQL.DataAnnotations;

namespace LINQToAQL.Test.Model
{
    //Type - open
    class Employment
    {
        [Field(Name = "organization-name")]
        public string OrganizationName { get; set; }

        [Field(Name = "start-date")]
        public DateTime StartDate { get; set; }

        [Field(Name = "end-date")]
        public DateTime? EndDate { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Spatial;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class SpatialQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("CreatePoint")
            {
                LinqQuery = dv.FacebookUsers.Select(u => new Point(u.id + 1.3, 3.2)),
                Aql = "for $u in dataset FacebookUsers return create-point(($u.id + 1.3), 3.2)",
                CleanJsonApi =
                    "[{\"point\":[7.3,3.2]},{\"point\":[2.3,3.2]},{\"point\":[3.3,3.2]},{\"point\":[5.3,3.2]},{\"point\":[9.3,3.2]},{\"point\":[10.3,3.2]},{\"point\":[11.3,3.2]},{\"point\":[4.3,3.2]},{\"point\":[6.3,3.2]},{\"point\":[8.3,3.2]}]",
                QueryResult = null
            },
            //TODO: handle conversion to double (make the forced conversion unecessary)
            new TestQuery("CreateLine")
            {
                LinqQuery =
                    dv.FacebookUsers.Where(u => u.id == 3)
                        .Select(u => new Line(new Point(u.id + 1.3, 1.9), new Point(2.4, u.id + 1.4))),
                Aql =
                    "for $u in dataset FacebookUsers where ($u.id = 3) return create-line(create-point(($u.id + 1.3), 1.9), create-point(2.4, ($u.id + 1.4)))",
                CleanJsonApi = "[{\"line\":[{\"point\":[4.3,1.9]},{\"point\":[2.4,4.4]}]}]",
                QueryResult = null
            }
        };
    }
}
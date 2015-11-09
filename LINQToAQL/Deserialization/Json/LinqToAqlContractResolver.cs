using System.Reflection;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LINQToAQL.Deserialization.Json
{
    class LinqToAqlContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            string field = member.GetAttributeValue((FieldAttribute f) => f.Name);
            if (field != null)
                property.PropertyName = field; //GetResolvedPropertyName(field)?
            return property;
        }
    }
}

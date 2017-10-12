using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Community
{
    public class CommunityCollectionSection : ModelBase
    {
        public string CollectionId { get; set; }
        public IEnumerable<Community> Communities { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            CollectionId = data.Resolve<string>("collectionId");

            var jArray = (JArray) data["childCommunityCollectionList"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            var communities = new List<Community>();
            foreach (var dictionary in enumerable)
            {
                var communityDict = ((JObject) dictionary["community"]).ToObject<Dictionary<string, object>>();
                var community = new Community();
                community.JsonResolve(communityDict);
                communities.Add(community);
            }
            Communities = communities;
        }
    }
}

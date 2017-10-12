using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Community
{
    public class CommunityList : ModelBase
    {
        private List<Community> _communities;

        public List<Community> Communities
        {
            get => _communities;
            set
            {
                _communities = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Communities = new List<Community>();
            var communitiesJArray = (JArray) data["communityList"];
            var enumerable = communitiesJArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            foreach (var dictionary in enumerable)
            {
                var community = new Community();
                community.JsonResolve(dictionary);
                Communities.Add(community);
            }
        }
    }
}

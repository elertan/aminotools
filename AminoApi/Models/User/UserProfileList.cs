using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.User
{
    public class UserProfileList : ModelBase
    {
        private List<UserProfile> _userProfiles;

        public List<UserProfile> UserProfiles
        {
            get => _userProfiles;
            set
            {
                _userProfiles = value;
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            UserProfiles = new List<UserProfile>();
            var userProfileJArray = (JArray)data["userProfileList"];
            var enumerable = userProfileJArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            foreach (var dictionary in enumerable)
            {
                var userProfile = new UserProfile();
                userProfile.JsonResolve(dictionary);
                UserProfiles.Add(userProfile);
            }
        }
    }
}
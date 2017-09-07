using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Chat
{
    public class Chat : ModelBase
    {
        public AlertOptions AlertOptions { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastReadTime { get; set; }
        public DateTime LastActivityTime { get; set; }
        public int AmountOfMembers { get; set; }
        public List<UserProfile> Members { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ThreadId { get; set; }
        public string Title { get; set; }
        public string RoomOwnerUserId { get; set; }
        public Uri Icon { get; set; }
        public string Keywords { get; set; }
        public string Content { get; set; }
        public Message LastMessage { get; set; }
        public string UserId { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            AlertOptions = (AlertOptions) Enum.ToObject(typeof(AlertOptions), data["alertOption"]);
            Content = data.Resolve<string>("content");
            var iconString = data.Resolve<string>("icon");
            if (!string.IsNullOrWhiteSpace(iconString)) Icon = new Uri(iconString);
            Keywords = data.Resolve<string>("keywords");
            UserId = data.Resolve<string>("uid");
            ThreadId = data.Resolve<string>("threadId");
            AmountOfMembers = data.Resolve<int>("membersCount");
            Title = data.Resolve<string>("title");
            Content = data.Resolve<string>("content");

            CreatedTime = data.Resolve<DateTime>("createdTime");
            ModifiedTime = data.Resolve<DateTime>("modifiedTime");
            LastReadTime = data.Resolve<DateTime>("lastReadTime");
            LastActivityTime = data.Resolve<DateTime>("latestActivityTime");

            if (data.ContainsKey("lastMessageSummary") && data["lastMessageSummary"] != null)
            {
                var message = new Message();
                message.JsonResolve(data["lastMessageSummary"].ToJObject().ToDictionary());
                LastMessage = message;
            }

            var jArray = (JArray) data["membersSummary"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            var userProfiles = new List<UserProfile>();
            foreach (var dictionary in enumerable)
            {
                var userProfile = new UserProfile();
                userProfile.JsonResolve(dictionary);
                userProfiles.Add(userProfile);
            }
            Members = userProfiles;
        }
    }
}

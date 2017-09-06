using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;

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

        public override void JsonResolve(Dictionary<string, object> data)
        {
            AlertOptions = data.Resolve<AlertOptions>("alertOption");
            Content = data.Resolve<string>("content");
            CreatedTime = data.Resolve<DateTime>("createdTime");
            var iconString = data.Resolve<string>("icon");
            if (!string.IsNullOrWhiteSpace(iconString)) Icon = new Uri(iconString);
            Keywords = data.Resolve<string>("keywords");

            // LastMessage
        }
    }
}

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
        private AlertOptions _alertOptions;
        private DateTime _createdTime;
        private DateTime _lastReadTime;
        private DateTime _lastActivityTime;
        private int _amountOfMembers;
        private List<UserProfile> _members;
        private DateTime _modifiedTime;
        private string _threadId;
        private string _title;
        private string _roomOwnerUserId;
        private Uri _icon;
        private string _keywords;
        private string _content;
        private Message _lastMessage;
        private string _userId;

        public AlertOptions AlertOptions
        {
            get => _alertOptions;
            set
            {
                _alertOptions = value; 
                OnPropertyChanged();
            }
        }

        public DateTime CreatedTime
        {
            get => _createdTime;
            set
            {
                _createdTime = value; 
                OnPropertyChanged();
            }
        }

        public DateTime LastReadTime
        {
            get => _lastReadTime;
            set
            {
                _lastReadTime = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasUnreadContent));
            }
        }

        public DateTime LastActivityTime
        {
            get => _lastActivityTime;
            set
            {
                _lastActivityTime = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasUnreadContent));
            }
        }

        public int AmountOfMembers
        {
            get => _amountOfMembers;
            set
            {
                _amountOfMembers = value; 
                OnPropertyChanged();
            }
        }

        public List<UserProfile> Members
        {
            get => _members;
            set
            {
                _members = value; 
                OnPropertyChanged();
            }
        }

        public DateTime ModifiedTime
        {
            get => _modifiedTime;
            set
            {
                _modifiedTime = value; 
                OnPropertyChanged();
            }
        }

        public string ThreadId
        {
            get => _threadId;
            set
            {
                _threadId = value; 
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public string RoomOwnerUserId
        {
            get => _roomOwnerUserId;
            set
            {
                _roomOwnerUserId = value; 
                OnPropertyChanged();
            }
        }

        public Uri Icon
        {
            get => _icon;
            set
            {
                _icon = value; 
                OnPropertyChanged();
            }
        }

        public string Keywords
        {
            get => _keywords;
            set
            {
                _keywords = value; 
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public Message LastMessage
        {
            get => _lastMessage;
            set
            {
                _lastMessage = value; 
                OnPropertyChanged();
            }
        }

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value; 
                OnPropertyChanged();
            }
        }

        public bool HasUnreadContent => LastActivityTime != LastReadTime;

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

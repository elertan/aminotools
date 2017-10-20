using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AminoApi.Models.Chat
{
    public class Message : ModelBase
    {
        private UserProfile _author;
        private string _content;
        private DateTime _createdTime;
        private string _id;
        private string _userId;
        private string _imageUrl;

        [ManyToOne]
        public UserProfile Author
        {
            get => _author;
            set
            {
                _author = value; 
                OnPropertyChanged();
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value; 
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

        public DateTime CreatedTime
        {
            get => _createdTime;
            set
            {
                _createdTime = value;
                OnPropertyChanged();
            }
        }

        [PrimaryKey]
        public string Id
        {
            get => _id;
            set
            {
                _id = value; 
                OnPropertyChanged();
            }
        }

        [ForeignKey(typeof(UserProfile))]
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            if (data.ContainsKey("message"))
            {
                JsonResolve(data["message"].ToJObject().ToObject<Dictionary<string, object>>());
                return;
            }

            Content = data.Resolve<string>("content");
            CreatedTime = data.Resolve<DateTime>("createdTime");
            Id = data.Resolve<string>("messageId");
            UserId = data.Resolve<string>("uid");
            var imageString = data.Resolve<string>("mediaValue");
            if (!string.IsNullOrWhiteSpace(imageString))
            {
                ImageUrl = imageString;
            }

            if (data.ContainsKey("author"))
            {
                var authorData = data["author"].ToJObject().ToObject<Dictionary<string, object>>();
                var userProfile = new UserProfile();
                userProfile.JsonResolve(authorData);
                Author = userProfile;
            }
        }
    }
}

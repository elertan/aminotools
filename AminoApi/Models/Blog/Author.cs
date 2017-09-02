using System;
using System.Collections.Generic;

namespace AminoApi.Models.Blog
{
    public class Author : ModelBase
    {
        private Uri _iconUri;
        private int _level;
        private string _nickname;
        private int _onlineStatus;
        private int _reputation;
        private int _role;
        private int _status;
        private string _userId;

        public Uri IconUri
        {
            get => _iconUri;
            set
            {
                _iconUri = value; 
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value; 
                OnPropertyChanged();
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value; 
                OnPropertyChanged();
            }
        }

        public int OnlineStatus
        {
            get => _onlineStatus;
            set
            {
                _onlineStatus = value; 
                OnPropertyChanged();
            }
        }

        public int Reputation
        {
            get => _reputation;
            set
            {
                _reputation = value; 
                OnPropertyChanged();
            }
        }

        public int Role
        {
            get => _role;
            set
            {
                _role = value; 
                OnPropertyChanged();
            }
        }

        public int Status
        {
            get => _status;
            set
            {
                _status = value; 
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

        public override void JsonResolve(Dictionary<string, object> data)
        {
            var uriString = Convert.ToString(data["icon"]);
            if (!string.IsNullOrWhiteSpace(uriString)) IconUri = new Uri(uriString);
            Level = Convert.ToInt32(data["level"]);
            //Mood = Convert.To???(data["mood"]);
            Nickname = Convert.ToString(data["nickname"]);
            OnlineStatus = Convert.ToInt32(data["onlineStatus"]);
            Reputation = Convert.ToInt32(data["reputation"]);
            Role = Convert.ToInt32(data["role"]);
            Status = Convert.ToInt32(data["status"]);
            UserId = Convert.ToString(data["uid"]);
        }
    }
}

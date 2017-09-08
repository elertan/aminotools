using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;
using Xamarin.Forms;

namespace AminoTools.Models.Chatting.GlobalChatting
{
    public class ChatCommunityModel : BaseModel
    {
        private readonly string _myUserId;
        private Chat _chat;
        private Community _community;

        public ChatCommunityModel(string myUserId)
        {
            _myUserId = myUserId;
        }

        public string LastMessageText
        {
            get
            {
                if (Chat.LastMessage != null)
                {
                    return Chat.LastMessage.Content;
                }
                return String.Empty;
            }
        }

        public string ChatTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Chat.Title)) return Chat.Title;
                return Chat.Members.First(m => m.Uid != _myUserId).Nickname;
            }
        }

        public string ChatIcon
        {
            get
            {
                if (Chat.AmountOfMembers < 3)
                {
                    return Chat.Members.First(m => m.Uid != _myUserId).Icon?.ToString();
                }
                return Chat.Icon?.ToString();
            }
        }

        public Chat Chat
        {
            get => _chat;
            set
            {
                _chat = value;
                OnPropertyChanged();
            }
        }

        public Community Community
        {
            get => _community;
            set
            {
                _community = value;
                OnPropertyChanged();
            }
        }
    }
}

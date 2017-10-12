using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Pages.Common;
using AminoTools.Providers.Contracts;
using Autofac;
using Xamarin.Forms;

namespace AminoTools.Models.Chatting.GlobalChatting
{
    public class MessageInfoModel : BaseModel
    {
        private Chat _chat;
        private Message _message;
        private ChatCommunityModel _chatCommunityModel;

        public ChatCommunityModel ChatCommunityModel
        {
            get => _chatCommunityModel;
            set
            {
                _chatCommunityModel = value; 
                OnPropertyChanged();
            }
        }

        public MessageInfoModel()
        {
            ImageTappedCommand = new Command(DoShowImage);
        }

        private async void DoShowImage()
        {
            var imagePage = new ImagePage(_message.Image);
            await App.MainNavigation.PushAsync(imagePage, true);
        }

        public bool ShouldBeAnimated { get; set; }

        public Chat Chat
        {
            get => _chat;
            set
            {
                _chat = value; 
                OnPropertyChanged();
            }
        }

        public Message Message
        {
            get => _message;
            set
            {
                _message = value; 
                OnPropertyChanged();
            }
        }

        public bool ShouldShowUserProfilePictureOnMessageView => Message.Author.Uid != App.Account.Uid;
        public Command ImageTappedCommand { get; }

        public Color MessageBackgroundColor
        {
            get
            {
                if (ShouldShowUserProfilePictureOnMessageView) return Color.WhiteSmoke;
                return Color.LightSkyBlue;
            }
        }

        public LayoutOptions HorizontalMessageLayoutOption
        {
            get
            {
                if (ShouldShowUserProfilePictureOnMessageView) return LayoutOptions.Start;
                return LayoutOptions.End;
            }
        }
    }
}

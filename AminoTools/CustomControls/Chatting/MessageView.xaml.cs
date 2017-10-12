using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.Pages.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomControls.Chatting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageView : ContentView, INotifyPropertyChanged
    {
        private MessageInfoModel _messageInfoModel;

        public ChatCommunityModel ChatCommunityModel { get; set; }

        public MessageView()
        {
            InitializeComponent();

            UserProfileIconTappedCommand = new Command(DoShowUserProfile);
            BindingContextChanged += MessageView_BindingContextChanged;
        }

        private async void DoShowUserProfile()
        {
            var app = (App)Application.Current;
            app.Variables.ProfilePage.Reset();
            app.Variables.ProfilePage.UserId = ChatCommunityModel.Chat.Members.First(m => m.Uid != app.Account.Uid)
                .Uid;
            app.Variables.ProfilePage.CommunityId = ChatCommunityModel.Community.Id;
            await ((App)Application.Current).MainNavigation.PushAsync(new ProfilePage());
        }

        public MessageInfoModel MessageInfoModel
        {
            get => _messageInfoModel;
            private set
            {
                _messageInfoModel = value; 
                OnPropertyChanged();
            }
        }

        public Command UserProfileIconTappedCommand { get; }

        private void MessageView_BindingContextChanged(object sender, EventArgs e)
        {
            MessageInfoModel = (MessageInfoModel) BindingContext;
        }
    }
}
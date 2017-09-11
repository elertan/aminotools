using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
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

            BindingContextChanged += MessageView_BindingContextChanged;
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

        private void MessageView_BindingContextChanged(object sender, EventArgs e)
        {
            MessageInfoModel = (MessageInfoModel) BindingContext;
        }
    }
}
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
        private Message _message;

        public ChatCommunityModel ChatCommunityModel { get; set; }

        public MessageView()
        {
            InitializeComponent();

            BindingContextChanged += MessageView_BindingContextChanged;
        }

        public Message Message
        {
            get => _message;
            private set
            {
                _message = value; 
                OnPropertyChanged();
            }
        }

        private void MessageView_BindingContextChanged(object sender, EventArgs e)
        {
            Message = (Message) BindingContext;
        }
    }
}
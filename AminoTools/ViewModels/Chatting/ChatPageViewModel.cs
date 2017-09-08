using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.Pages.Chatting;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Chatting;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Chatting
{
    public class ChatPageViewModel : BaseViewModel, IChatPageViewModel
    {
        private readonly IChatProvider _chatProvider;
        private ChatCommunityModel _chatCommunityModel;
        private ObservableRangeCollection<Message> _messages;
        private ChatPage _chatPage;

        public ChatPageViewModel(IChatProvider chatProvider)
        {
            _chatProvider = chatProvider;
            Initialize += ChatPageViewModel_Initialize;
        }

        private async void ChatPageViewModel_Initialize(object sender, EventArgs e)
        {
            _chatPage = (ChatPage) Page;
            ChatCommunityModel = _chatPage.ChatCommunityModel;

            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Loading messages";
                var apiResult = await _chatProvider.GetMessagesAsync(ChatCommunityModel.Community.Id, ChatCommunityModel.Chat.ThreadId);
                if (!apiResult.DidSucceed())
                {
                    await Page.DisplayAlert("Error", apiResult.Info.Message, "Ok");
                    return;
                }
                apiResult.Data.Messages.Reverse();
                Messages = new ObservableRangeCollection<Message>(apiResult.Data.Messages);
            });

            OnMessageReceived();
        }

        private void OnMessageReceived()
        {
            _chatPage.GetListView().ScrollTo(Messages.Last(), ScrollToPosition.End, false);
        }

        public ChatCommunityModel ChatCommunityModel
        {
            get => _chatCommunityModel;
            private set
            {
                _chatCommunityModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableRangeCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                Messages.CollectionChanged += Messages_CollectionChanged;
                OnPropertyChanged();
            }
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnMessageReceived();
        }
    }
}

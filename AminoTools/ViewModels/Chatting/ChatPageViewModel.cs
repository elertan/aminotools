using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableRangeCollection<MessageInfoModel> _messages;
        private ChatPage _chatPage;
        private int _loadMoreMessagesCounter = 1;
        private bool _isRefreshing;
        private string _message;
        private readonly TimeSpan _newMessagesCheckInterval = TimeSpan.FromSeconds(10);

        public ChatPageViewModel(IChatProvider chatProvider)
        {
            _chatProvider = chatProvider;
            Initialize += ChatPageViewModel_Initialize;

            LoadMoreMessagesCommand = new Command(DoLoadMoreMessages);
            SendMessageCommand = new Command(DoSendMessage);
        }

        private async void DoSendMessage()
        {
            if (!string.IsNullOrWhiteSpace(Message))
            {
                var msg = Message.Trim();
                var apiResult = await _chatProvider.SendMessageToChatAsync(ChatCommunityModel.Community.Id,
                    ChatCommunityModel.Chat.ThreadId, msg);
                if (!apiResult.DidSucceed())
                {
                    await Page.DisplayAlert("Error", apiResult.Info.Message, "Ok");
                    return;
                }

                _chatCommunityModel.Chat.LastActivityTime = DateTime.UtcNow;
                _chatCommunityModel.Chat.LastMessage = apiResult.Data;

                Message = String.Empty;

                var messageInfoModel = new MessageInfoModel {Message = apiResult.Data, Chat = ChatCommunityModel.Chat, ShouldBeAnimated = true, ChatCommunityModel = _chatCommunityModel};
                var m = Messages.ToList();
                m.Add(messageInfoModel);
                Messages = new ObservableRangeCollection<MessageInfoModel>(m);

                await Task.Delay(50);
                OnMessageReceived();
            }
        }

        private async void ChatPageViewModel_Initialize(object sender, EventArgs e)
        {
            _chatPage = (ChatPage) Page;
            ChatCommunityModel = _chatPage.ChatCommunityModel;
            _chatPage.Title = ChatCommunityModel.ChatTitle;

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
                Messages = new ObservableRangeCollection<MessageInfoModel>(apiResult.Data.Messages.Select(m => new MessageInfoModel {Message = m, Chat = ChatCommunityModel.Chat, ChatCommunityModel = _chatCommunityModel}));
            });

            OnMessageReceived();

            Device.StartTimer(_newMessagesCheckInterval, MessagesCheckCallback);
        }

        private bool MessagesCheckCallback()
        {
            Task.Run(async () =>
            {
                var messageInfoModel = Messages.LastOrDefault();
                // Chat has no messages at all
                if (messageInfoModel == null) return;

                var lastLocalMessage = messageInfoModel.Message;
                var newMessages = new List<Message>();
                // Keep loading messages until we find our last local one
                for (int i = 0;;i++)
                {
                    var apiResult = await _chatProvider.GetMessagesAsync(ChatCommunityModel.Community.Id, ChatCommunityModel.Chat.ThreadId, i);
                    if (!apiResult.DidSucceed())
                    {
                        await Page.DisplayAlert("Error", "Could not update messages", "Ok");
                        return;
                    }

                    // Does this iteration contain our last local message?
                    if (apiResult.Data.Messages.FirstOrDefault(m => m.Id == lastLocalMessage.Id) != null)
                    {
                        var message = apiResult.Data.Messages.First(m => m.Id == lastLocalMessage.Id);
                        var localMessageIndex = apiResult.Data.Messages.IndexOf(message);
                        // Get our new messages
                        var messages = apiResult.Data.Messages.GetRange(0, localMessageIndex);
                        newMessages.AddRange(messages);
                        break;
                    }
                    newMessages.AddRange(apiResult.Data.Messages);
                }

                Messages.AddRange(newMessages.Select(m => new MessageInfoModel {Message = m, Chat = ChatCommunityModel.Chat, ChatCommunityModel = _chatCommunityModel}));
            });
            
            return !WantsDisposal;
        }

        private async void DoLoadMoreMessages()
        {
            IsRefreshing = true;
            var apiResult = await _chatProvider.GetMessagesAsync(ChatCommunityModel.Community.Id, ChatCommunityModel.Chat.ThreadId, _loadMoreMessagesCounter);
            if (!apiResult.DidSucceed())
            {
                await Page.DisplayAlert("Error", apiResult.Info.Message, "Ok");
                return;
            }
            apiResult.Data.Messages.Reverse();

            var firstMessage = Messages.First();

            var collection = new ObservableRangeCollection<MessageInfoModel>(apiResult.Data.Messages.Select(m => new MessageInfoModel { Message = m, Chat = ChatCommunityModel.Chat, ChatCommunityModel = _chatCommunityModel}));
            collection.AddRange(Messages);
            Messages = collection;

            _loadMoreMessagesCounter++;
            IsRefreshing = false;

            await Task.Delay(50);
            _chatPage.GetListView().ScrollTo(firstMessage, ScrollToPosition.Start, false);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value; 
                OnPropertyChanged();
            }
        }

        private void OnMessageReceived()
        {
            _chatPage.GetListView().ScrollTo(Messages.Last(), ScrollToPosition.End, false);
        }

        public Command LoadMoreMessagesCommand { get; }
        public Command SendMessageCommand { get; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value; 
                OnPropertyChanged();
            }
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

        public ObservableRangeCollection<MessageInfoModel> Messages
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
            //OnMessageReceived();
        }
    }
}

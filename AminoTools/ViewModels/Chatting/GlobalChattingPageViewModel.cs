using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.Pages.Chatting;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Chatting;
using AminoTools.ViewModels.Contracts.Community;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Chatting
{
    public class GlobalChattingPageViewModel : BaseViewModel, IGlobalChattingPageViewModel
    {
        private readonly TimeSpan _threadCheckDelay = TimeSpan.FromSeconds(45);
        
        private readonly ICommunityProvider _communityProvider;
        private readonly IChatProvider _chatProvider;
        private ObservableRangeCollection<ChatCommunityModel> _chats;
        private bool _isLoading;
        private string _loadingText;
        private float _loadingProgress;

        public GlobalChattingPageViewModel(ICommunityProvider communityProvider, 
            IChatProvider chatProvider)
        {
            _communityProvider = communityProvider;
            _chatProvider = chatProvider;
            Initialize += GlobalChattingPageViewModel_Initialize;

            Chats = new ObservableRangeCollection<ChatCommunityModel>();
            Device.StartTimer(TimeSpan.FromMinutes(1), UpdateTimeOnChatsTimerCallback);
        }

        private bool UpdateTimeOnChatsTimerCallback()
        {
            if (Chats != null)
            {
                foreach (var chat in Chats)
                {
                    if (chat.Chat.LastMessage != null)
                    {
                        // Cause on property changed
                        chat.Chat.LastMessage.CreatedTime = chat.Chat.LastMessage.CreatedTime;
                    }
                }
            }
            return !WantsDisposal;
        }

        private async void GlobalChattingPageViewModel_Initialize(object sender, EventArgs e)
        {
            IsLoading = true;
            List<ChatCommunityModel> chats = new List<ChatCommunityModel>();
            await LoadingGrid.FadeTo(1);
            LoadingText = "Getting communities";
            var communitiesResult = await _communityProvider.GetAllJoinedCommunities();
            Communities = new List<AminoApi.Models.Community.Community>(communitiesResult.Data);

            if (!Communities.Any())
            {
                await Page.DisplayAlert("Error",
                    "You need to join some communities first in order to use the chat function", "Ok");
                return;
            }

            //var storedChats = await _chatProvider.GetStoredChatsAsync();
            //if (storedChats.Any())
            //{
            //    chats = storedChats.Select(sc => new ChatCommunityModel(App.Account.Uid) { Chat = sc, Community = sc.Community }).ToList();
            //    Chats = new ObservableRangeCollection<ChatCommunityModel>(chats.OrderByDescending(c => c.Chat.LastMessage?.CreatedTime));
            //}

            var i = 1;
            foreach (var community in Communities)
            {
                if (IsInitializationCanceled) return;
                LoadingText = $"Chats for community {i} / {Communities.Count}";
                LoadingProgress = (i - 1) / (float)Communities.Count;
                var chatsResult = await _chatProvider.GetChatsByCommunityAsync(community);
                if (chatsResult.Data.Any())
                {
                    var chatCommunityModels = chatsResult.Data.Select(c => new ChatCommunityModel(App.Account.Uid) { Chat = c, Community = community });
                    chats.AddRange(chatCommunityModels);
                }
                i++;
            }

            Chats = new ObservableRangeCollection<ChatCommunityModel>(chats.OrderByDescending(c => c.Chat.LastMessage?.CreatedTime));
            await LoadingGrid.LayoutTo(new Rectangle(0, 0, LoadingGrid.Width, 0), 250U, Easing.BounceIn);
            IsLoading = false;

            Device.StartTimer(_threadCheckDelay, ThreadCheckCallback);
        }

        public List<AminoApi.Models.Community.Community> Communities { get; set; }

        private bool ThreadCheckCallback()
        {
            Task.Run(async () =>
            {
                var newChats = new List<Chat>();

                foreach (var community in Communities)
                {
                    var apiResult = await _chatProvider.CheckForNewMessagesAsync(community.Id);
                    if (!apiResult.DidSucceed())
                    {
                        await Page.DisplayAlert("Error Chat Checking", apiResult.Info.Message, "Ok");
                        break;
                    }

                    // No new messages
                    if (!apiResult.Data.Any()) continue;

                    var chatsApiResult = await _chatProvider.GetChatsByCommunityAsync(community);
                    foreach (var chat in chatsApiResult.Data)
                    {
                        chat.Community = community;
                    }
                    newChats.AddRange(chatsApiResult.Data.Where(c => apiResult.Data.Any(tr => tr.ThreadId == c.ThreadId)));
                }

                // Nothing to update
                if (!newChats.Any()) return;

                var models = Chats.ToList();
                // Update current messages
                foreach (var newChat in newChats)
                {
                    var model = models.FirstOrDefault(m => m.Chat.ThreadId == newChat.ThreadId);
                    if (model != null)
                    {
                        models.Remove(model);
                    }
                    models.Add(new ChatCommunityModel(App.Account.Uid) { Chat = newChat, Community = newChat.Community });
                }

                Chats = new ObservableRangeCollection<ChatCommunityModel>(models.OrderByDescending(c => c.Chat.LastMessage?.CreatedTime));
            });
            return !WantsDisposal;
        }

        private Grid LoadingGrid => ((GlobalChattingPage) Page).GetLoadingGrid();

        public ObservableRangeCollection<ChatCommunityModel> Chats
        {
            get => _chats;
            set
            {
                _chats = value; 
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value; 
                OnPropertyChanged();
            }
        }

        public string LoadingText
        {
            get => _loadingText;
            set
            {
                _loadingText = value; 
                OnPropertyChanged();
            }
        }

        public float LoadingProgress
        {
            get => _loadingProgress;
            set
            {
                _loadingProgress = value; 
                OnPropertyChanged();
            }
        }
    }
}

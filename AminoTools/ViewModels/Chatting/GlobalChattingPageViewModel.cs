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
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Chatting
{
    public class GlobalChattingPageViewModel : BaseViewModel, IGlobalChattingPageViewModel
    {
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
            await LoadingGrid.FadeTo(1);
            LoadingText = "Getting communities";
            var communitiesResult = await _communityProvider.GetAllJoinedCommunities();
            var communities = new List<AminoApi.Models.Community.Community>(communitiesResult.Data);

            if (!communities.Any())
            {
                await Page.DisplayAlert("Error",
                    "You need to join some communities first in order to use the chat function", "Ok");
                return;
            }

            var i = 1;
            foreach (var community in communities)
            {
                if (IsInitializationCanceled) return;
                LoadingText = $"Chats for community {i} / {communities.Count}";
                LoadingProgress = (i - 1) / (float)communities.Count;
                var chatsResult = await _chatProvider.GetChatsByCommunityAsync(community.Id);
                if (chatsResult.Data.Any())
                {
                    var chatCommunityModels = chatsResult.Data.Select(c => new ChatCommunityModel(App.Account.Uid) { Chat = c, Community = community });
                    var chats = Chats.ToList();
                    chats.AddRange(chatCommunityModels);
                    Chats = new ObservableRangeCollection<ChatCommunityModel>(chats.OrderByDescending(c => c.Chat.LastMessage?.CreatedTime));
                }

                i++;
            }
            await LoadingGrid.LayoutTo(new Rectangle(0, 0, LoadingGrid.Width, 0), 250U, Easing.BounceIn);
            IsLoading = false;
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

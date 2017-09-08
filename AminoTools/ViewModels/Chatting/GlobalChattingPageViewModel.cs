using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Chatting;
using MvvmHelpers;

namespace AminoTools.ViewModels.Chatting
{
    public class GlobalChattingPageViewModel : BaseViewModel, IGlobalChattingPageViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IChatProvider _chatProvider;
        private ObservableRangeCollection<ChatCommunityModel> _chats;

        public GlobalChattingPageViewModel(ICommunityProvider communityProvider, 
            IChatProvider chatProvider)
        {
            _communityProvider = communityProvider;
            _chatProvider = chatProvider;
            Initialize += GlobalChattingPageViewModel_Initialize;

            Chats = new ObservableRangeCollection<ChatCommunityModel>();
        }

        private async void GlobalChattingPageViewModel_Initialize(object sender, EventArgs e)
        {
            List<AminoApi.Models.Community.Community> communities = new List<AminoApi.Models.Community.Community>();
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting communities";
                var communitiesResult = await _communityProvider.GetAllJoinedCommunities();
                communities.AddRange(communitiesResult.Data);
            });

            if (!communities.Any())
            {
                await Page.DisplayAlert("Error",
                    "You need to join some communities first in order to use the chat function", "Ok");
                return;
            }

            foreach (var community in communities)
            {
                if (IsInitializationCanceled) return;

                var chatsResult = await _chatProvider.GetChatsByCommunityAsync(community.Id);
                if (chatsResult.Data.Any())
                {
                    var chatCommunityModels = chatsResult.Data.Select(c => new ChatCommunityModel(App.Account.Uid) { Chat = c, Community = community });
                    Chats.AddRange(chatCommunityModels);
                }
            }
        }

        public ObservableRangeCollection<ChatCommunityModel> Chats
        {
            get => _chats;
            set
            {
                _chats = value; 
                OnPropertyChanged();
            }
        }
    }
}

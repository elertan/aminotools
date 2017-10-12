using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Community;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Community
{
    public class JoinRandomCommunitiesPageViewModel : BaseViewModel, IJoinRandomCommunitiesPageViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IRandomWordProvider _randomWordProvider;
        private int _minimumAmountOfMembers;

        public int MinimumAmountOfMembers
        {
            get => _minimumAmountOfMembers;
            set
            {
                _minimumAmountOfMembers = value; 
                OnPropertyChanged();
            }
        }

        public Command JoinCommunitiesCommand { get; }

        public JoinRandomCommunitiesPageViewModel(ICommunityProvider communityProvider, IRandomWordProvider randomWordProvider)
        {
            _communityProvider = communityProvider;
            _randomWordProvider = randomWordProvider;
            JoinCommunitiesCommand = new Command(DoJoinCommunities);

            MinimumAmountOfMembers = 500;
        }

        private async void DoJoinCommunities()
        {
            var joinedCommunityIds = new List<string>();

            var cts = new CancellationTokenSource();
            var i = 0;
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting Communities";
                while (!cts.IsCancellationRequested)
                {
                    var searchTerm = await _randomWordProvider.GetRandomWord();
                    IsBusyData.Description = $"Searching Communties for {searchTerm}";
                    var communitiesResult = await _communityProvider.GetCommunitiesByQuery(searchTerm);

                    if (!communitiesResult.DidSucceed())
                    {
                        await Page.DisplayAlert("Something went wrong", communitiesResult.Info.Message, "Ok");
                        cts.Cancel();
                        return;
                    }

                    var communities = communitiesResult.Data.Where(c => joinedCommunityIds.All(jci => jci != c.Id) && c.AmountOfMembers >= MinimumAmountOfMembers).ToList();

                    foreach (var community in communities)
                    {
                        IsBusyData.Description = $"Joining {community.Name}";
                        await _communityProvider.JoinCommunity(community.Id);
                        joinedCommunityIds.Add(community.Id);
                        i++;
                        if (cts.IsCancellationRequested) return;
                    }
                }
            }, cts);

            if (cts.IsCancellationRequested)
            {
                await Page.DisplayAlert("Canceled", $"You have joined {i} communities!", "Ok");
            }
            else
            {
                await Page.DisplayAlert("Finished", $"You have joined {i} communities!", "Ok");
            }
        }
    }
}

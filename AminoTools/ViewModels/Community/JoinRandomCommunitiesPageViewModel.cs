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
        public Command JoinCommunitiesCommand { get; }

        public JoinRandomCommunitiesPageViewModel(ICommunityProvider communityProvider)
        {
            _communityProvider = communityProvider;
            JoinCommunitiesCommand = new Command(DoJoinCommunities);
        }

        private async void DoJoinCommunities()
        {
            var cts = new CancellationTokenSource();
            await DoAsBusyStateCustom(async () =>
            {
                var i = 0;
                while (!cts.IsCancellationRequested)
                {
                    IsBusyData.Description = "Getting Communities";
                    var response = await _communityProvider.GetCommunitiesFromExplore(i * 25);
                    var section = response.Sections.FirstOrDefault();
                    if (section == null) return;

                    foreach (var community in section.Communities)
                    {
                        IsBusyData.Description = $"Joining {community.Name}";
                        await Task.Delay(500);
                        if (cts.IsCancellationRequested) return;
                    }

                    i++;
                }
            }, cts);
        }
    }
}

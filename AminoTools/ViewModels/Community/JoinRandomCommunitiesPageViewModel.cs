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
            var i = 0;
            await DoAsBusyStateCustom(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    IsBusyData.Description = "We're counting! " + i;
                    i++;
                    var response = await _communityProvider.GetCommunitiesFromExplore();
                    break;
                }
            }, cts);
        }
    }
}

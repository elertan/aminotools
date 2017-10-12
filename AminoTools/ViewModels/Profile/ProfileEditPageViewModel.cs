using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;
using AminoTools.Pages.Blogs;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Profile;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Profile
{
    public class ProfileEditPageViewModel : BaseViewModel, IProfileEditPageViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IUserProvider _userProvider;

        public ProfileEditPageViewModel(ICommunityProvider communityProvider,
            IUserProvider userProvider)
        {
            _communityProvider = communityProvider;
            _userProvider = userProvider;
            Profile = new UserProfile();
            var currentProfile = App.Variables.ProfileEditPage.Profile;
            Profile.Icon = currentProfile.Icon;
            Profile.Nickname = currentProfile.Nickname;
            Profile.Status = currentProfile.Status;
            Profile.Uid = currentProfile.Uid;
            SaveChangesCommand = new Command(DoSaveChanges);
            EditProfilePictureCommand = new Command(DoEditProfilePicture);
        }

        private async void DoSaveChanges()
        {
            //var communitySelectionPage = new CommunitySelectionPage();
            //await App.MainNavigation.PushAsync(communitySelectionPage);
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting communities";
                var apiResult = await _communityProvider.GetAllJoinedCommunities();
                if (!apiResult.DidSucceed())
                {
                    await Page.DisplayAlert("Something went wrong", apiResult.Info.Message, "Ok");
                    return;
                }
                if (!apiResult.Data.Any())
                {
                    await Page.DisplayAlert("Whoops", "You have not yet joined any communities, saving is not possible.",
                        "Ok");
                    return;
                }
                foreach (var community in apiResult.Data)
                {
                    IsBusyData.Description = $"Setting profile for community '{community.Name}'";
                }
            });
        }

        private async void DoEditProfilePicture()
        {
            
        }

        public UserProfile Profile { get; set; }
        public Command EditProfilePictureCommand { get; }
        public Command SaveChangesCommand { get; }
    }
}

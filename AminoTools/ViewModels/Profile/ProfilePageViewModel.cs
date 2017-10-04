using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoApi.Models.User;
using AminoTools.Pages.Common;
using AminoTools.Pages.Profile;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Profile;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Profile
{
    public class ProfilePageViewModel : BaseViewModel, IProfilePageViewModel
    {
        private readonly IUserProvider _userProvider;
        private bool _isMyProfile;
        private UserProfile _profile;
        private string _bio;
        private Command _editMyProfileCommand;

        public bool IsMyProfile
        {
            get => _isMyProfile;
            set
            {
                _isMyProfile = value; 
                OnPropertyChanged();
            }
        }

        public UserProfile Profile
        {
            get => _profile;
            set
            {
                _profile = value; 
                OnPropertyChanged();
            }
        }

        public string Bio
        {
            get => _bio;
            set
            {
                _bio = value; 
                OnPropertyChanged();
            }
        }

        public Command EditMyProfileCommand
        {
            get => _editMyProfileCommand;
            set
            {
                _editMyProfileCommand = value; 
                OnPropertyChanged();
            }
        }

        public Command ShowProfilePictureCommand { get; }

        public ProfilePageViewModel(IUserProvider userProvider)
        {
            _userProvider = userProvider;
            EditMyProfileCommand = new Command(DoEditMyProfile);
            ShowProfilePictureCommand = new Command(DoShowProfilePicture);
            Initialize += ProfilePageViewModel_Initialize;
        }

        private async void DoShowProfilePicture()
        {
            var imagePage = new ImagePage(Profile.Icon);
            await App.MainNavigation.PushAsync(imagePage);
        }

        private async void DoEditMyProfile()
        {
            App.Variables.ProfileEditPage.Profile = Profile;
            await App.MainNavigation.PushAsync(new ProfileEditPage());
        }

        private async void ProfilePageViewModel_Initialize(object sender, EventArgs e)
        {
            if (App.Variables.ProfilePage.IsMyProfile)
            {
                IsMyProfile = true;
                Profile = App.Account;
                return;
            }
            var communityId = App.Variables.ProfilePage.CommunityId;
            var userId = App.Variables.ProfilePage.UserId;

            var apiResult = await _userProvider.GetUserProfileByIdAsync(communityId, userId);
            if (!apiResult.DidSucceed())
            {
                await Page.DisplayAlert("Whoops", "Failed to get the user's profile", "Ok");
                return;
            }
            Profile = apiResult.Data;
        }
    }
}

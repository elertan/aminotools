using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;
using AminoTools.ViewModels.Contracts.Profile;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Profile
{
    public class ProfileEditPageViewModel : BaseViewModel, IProfileEditPageViewModel
    {
        public ProfileEditPageViewModel()
        {
            Profile = new UserProfile();
            var currentProfile = App.Variables.ProfileEditPage.Profile;
            Profile.Icon = currentProfile.Icon;
            Profile.Nickname = currentProfile.Nickname;
            Profile.Status = currentProfile.Status;
            Profile.Uid = currentProfile.Uid;
            EditProfilePictureCommand = new Command(DoEditProfilePicture);
        }

        private async void DoEditProfilePicture()
        {
            
        }

        public UserProfile Profile { get; set; }
        public Command EditProfilePictureCommand { get; }
    }
}

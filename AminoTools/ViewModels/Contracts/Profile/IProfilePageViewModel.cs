using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoApi.Models.User;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Profile
{
    public interface IProfilePageViewModel : IViewModel
    {
        bool IsMyProfile { get; set; }
        UserProfile Profile { get; set; }
        string Bio { get; set; }
        Command EditMyProfileCommand { get; set; }
        Command ShowProfilePictureCommand { get; }
    }
}

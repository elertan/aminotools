using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.User;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Profile
{
    public interface IProfileEditPageViewModel : IViewModel
    {
        UserProfile Profile { get; set; }
        Command EditProfilePictureCommand { get; }
        Command SaveChangesCommand { get; }
    }
}

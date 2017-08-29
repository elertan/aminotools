using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Auth
{
    public interface ILoginPageViewModel
    {
        string Username { get; set; }
        string Password { get; set; }
        bool MayLogin { get; }
        Command LoginCommand { get; set; }
    }
}

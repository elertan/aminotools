using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using MvvmHelpers;
using Xamarin.Forms;
using MenuItem = AminoTools.Models.MainPageMenu.MenuItem;

namespace AminoTools.ViewModels.Contracts
{
    public interface IMainPageMenuPageViewModel
    {
        ObservableRangeCollection<MenuItem> MenuItems { get; set; }
        MenuItem SelectedItem { get; set; }
        Account Account { get; }
        Command UserIconTappedCommand { get; }
    }
}

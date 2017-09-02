using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoTools.Models.MainPageMenu;
using MvvmHelpers;

namespace AminoTools.ViewModels.Contracts
{
    public interface IMainPageMenuPageViewModel
    {
        ObservableRangeCollection<MenuItem> MenuItems { get; set; }
        MenuItem SelectedItem { get; set; }
        Account Account { get; }
    }
}

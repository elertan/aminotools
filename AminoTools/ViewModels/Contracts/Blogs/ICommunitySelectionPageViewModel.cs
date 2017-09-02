using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Community;
using AminoTools.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Blogs
{
    public interface ICommunitySelectionPageViewModel
    {
        ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>> SelectableCommunities { get; set; }
        Command SelectAllCommand { get; set; }
        Command SelectNoneCommand { get; set; }
        string SendButtonText { get; }
        bool SendButtonIsEnabled { get; }
        Command SendButtonCommand { get; set; }
    }
}

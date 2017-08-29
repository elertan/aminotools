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
        ObservableRangeCollection<SelectableItem<Community>> SelectableCommunities { get; set; }
        Command SelectAllCommand { get; set; }
        Command SelectNoneCommand { get; set; }
        string SendButtonText { get; }
        Command SendButtonCommand { get; set; }
    }
}

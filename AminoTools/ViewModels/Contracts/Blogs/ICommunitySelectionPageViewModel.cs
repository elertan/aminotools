using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Community;
using AminoTools.Models;
using AminoTools.Models.Common;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Blogs
{
    public interface ICommunitySelectionPageViewModel
    {
        ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>> SelectableCommunities { get; set; }
        Command SelectAllCommand { get; set; }
        Command SelectNoneCommand { get; set; }
        bool CompleteSelectionButtonIsEnabled { get; }
        Command CompleteSelectionCommand { get; set; }
        bool HasReturnedSelectionResult { get; set; }
        Action<CommunitySelectionResult> CommunitySelectionResultAction { get; }
    }
}

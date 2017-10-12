using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoApi.Models.Community;
using AminoApi.Models.Media;
using AminoTools.Models;
using AminoTools.Models.Common;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Blogs;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class CommunitySelectionPageViewModel : BaseViewModel, ICommunitySelectionPageViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>> _selectableCommunities;
        private Command _selectAllCommand;
        private Command _selectNoneCommand;
        private Command _sendButtonCommand;
        private bool _hasReturnedSelectionResult;

        public ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>> SelectableCommunities
        {
            get => _selectableCommunities;
            set
            {
                _selectableCommunities = value; 
                OnPropertyChanged();
            }
        }

        public Command SelectAllCommand
        {
            get => _selectAllCommand;
            set
            {
                _selectAllCommand = value; 
                OnPropertyChanged();
            }
        }

        public Command SelectNoneCommand
        {
            get => _selectNoneCommand;
            set
            {
                _selectNoneCommand = value; 
                OnPropertyChanged();
            }
        }

        public bool CompleteSelectionButtonIsEnabled => !(SelectableCommunities == null || !IsNextAllowed());

        public Command CompleteSelectionCommand

        {
            get => _sendButtonCommand;
            set
            {
                _sendButtonCommand = value; 
                OnPropertyChanged();
            }
        }

        public bool HasReturnedSelectionResult
        {
            get => _hasReturnedSelectionResult;
            set
            {
                _hasReturnedSelectionResult = value; 
                OnPropertyChanged();
            }
        }

        public Action<CommunitySelectionResult> CommunitySelectionResultAction =>
            (Action<CommunitySelectionResult>) ViewModelParameter;

        public CommunitySelectionPageViewModel(ICommunityProvider communityProvider)
        {
            _communityProvider = communityProvider;

            SelectAllCommand = new Command(DoSelectAll);
            SelectNoneCommand = new Command(DoSelectNone);
            CompleteSelectionCommand = new Command(DoCompleteSelection);

            Initialize += CommunitySelectionPageViewModel_Initialize;
        }

        private async void DoCompleteSelection()
        {
            HasReturnedSelectionResult = true;
            await App.MainNavigation.PopAsync();
            CommunitySelectionResultAction(new CommunitySelectionResult {SelectedCommunities = SelectableCommunities.Where(sc => sc.IsSelected).Select(sc => sc.Item).ToList()});
        }


        private bool IsNextAllowed()
        {
            return SelectableCommunities.Any(c => c.IsSelected);
        }

        private void DoSelectNone()
        {
            foreach (var selectableCommunity in SelectableCommunities)
            {
                selectableCommunity.IsSelected = false;
            }
        }

        private void DoSelectAll()
        {
            foreach (var selectableCommunity in SelectableCommunities)
            {
                selectableCommunity.IsSelected = true;
            }
        }

        private async void CommunitySelectionPageViewModel_Initialize(object sender, EventArgs e)
        {
            Page.Disappearing += Page_Disappearing;
            var communitiesResult = await DoAsBusyState(_communityProvider.GetAllJoinedCommunities());

            if (!communitiesResult.DidSucceed())
            {
                await Page.DisplayAlert("Something went wrong", communitiesResult.Info.Message, "Ok");
                return;
            }

            var selectableCommunties = GetSelectableCommuntiesByCommunities(communitiesResult.Data);
            SelectableCommunities = new ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>>(selectableCommunties);
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            if (!HasReturnedSelectionResult) CommunitySelectionResultAction(new CommunitySelectionResult());
        }

        private IEnumerable<SelectableItem<AminoApi.Models.Community.Community>> GetSelectableCommuntiesByCommunities(IEnumerable<AminoApi.Models.Community.Community> communities)
        {
            var selectableItems = communities.Select(c => new SelectableItem<AminoApi.Models.Community.Community> { Item = c }).ToArray();
            foreach (var item in selectableItems)
            {
                item.IsSelectedChanged += Item_IsSelectedChanged;
            }
            return selectableItems;
        }

        private void Item_IsSelectedChanged(object sender, EventArgs e)
        {
            //OnPropertyChanged(nameof(SendButtonText));
            OnPropertyChanged(nameof(CompleteSelectionButtonIsEnabled));
        }
    }
}

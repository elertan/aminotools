using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Community;
using AminoTools.Models;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Blogs;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Blogs
{
    public class CommunitySelectionPageViewModel : BaseViewModel, ICommunitySelectionPageViewModel
    {
        private readonly ICommunityProvider _communityProvider;
        private readonly IBlogProvider _blogProvider;
        private ObservableRangeCollection<SelectableItem<Community>> _selectableCommunities;
        private Command _selectAllCommand;
        private Command _selectNoneCommand;
        private Command _sendButtonCommand;

        public ObservableRangeCollection<SelectableItem<Community>> SelectableCommunities
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

        public string SendButtonText 
        {
            get
            {
                if (SelectableCommunities == null || !MaySendBlog())
                {
                    return "Select some communities first..";
                }
                return $"Send to {SelectableCommunities.Count} communities!";
            }
        }

        public Command SendButtonCommand
        {
            get => _sendButtonCommand;
            set
            {
                _sendButtonCommand = value; 
                OnPropertyChanged();
            }
        }

        public CommunitySelectionPageViewModel(ICommunityProvider communityProvider,
            IBlogProvider blogProvider)
        {
            _communityProvider = communityProvider;
            _blogProvider = blogProvider;
            SelectableCommunities = new ObservableRangeCollection<SelectableItem<Community>>();

            SelectAllCommand = new Command(DoSelectAll);
            SelectNoneCommand = new Command(DoSelectNone);
            SendButtonCommand = new Command(DoSendBlog);

            Initialize += CommunitySelectionPageViewModel_Initialize;
        }

        private async void DoSendBlog()
        {
            if (!MaySendBlog())
            {
                await Page.DisplayAlert("Oops!", "You need to select at least one community to send the blog to", "Ok");
                return;
            }

            App.Variables.MultiBlog.Communities = SelectableCommunities.Where(sc => sc.IsSelected).Select(sc => sc.Item);

            var blog = App.Variables.MultiBlog.Blog;
            foreach (var community in App.Variables.MultiBlog.Communities)
            {
                await DoAsBusyState(_blogProvider.PostBlog(community.Id, blog.Title, blog.Content));
            }

            await Page.DisplayAlert("Yay!",
                $"Your blog has been posted on {App.Variables.MultiBlog.Communities.Count()} communities!", "Wohoo!");
        }

        private bool MaySendBlog()
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
            var communities = await DoAsBusyState(_communityProvider.GetJoinedCommunities());
            var selectableCommunties = GetSelectableCommuntiesByCommunities(communities);
            SelectableCommunities.AddRange(selectableCommunties);
        }

        private IEnumerable<SelectableItem<Community>> GetSelectableCommuntiesByCommunities(IEnumerable<Community> communities)
        {
            var selectableItems = communities.Select(c => new SelectableItem<Community> { Item = c });
            foreach (var item in selectableItems)
            {
                item.PropertyChanged += SelectableItem_PropertyChanged;
            }
            return selectableItems;
        }

        private void SelectableItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SendButtonText));
        }
    }
}

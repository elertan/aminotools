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
        private readonly IMediaProvider _mediaProvider;
        private ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>> _selectableCommunities;
        private Command _selectAllCommand;
        private Command _selectNoneCommand;
        private Command _sendButtonCommand;

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

        public string SendButtonText 
        {
            get
            {
                if (SelectableCommunities == null || !MaySendBlog())
                {
                    return "No communities selected";
                }
                return $"Send to {SelectableCommunities.Count(sc => sc.IsSelected)} communities!";
            }
        }

        public bool SendButtonIsEnabled => !(SelectableCommunities == null || !MaySendBlog());

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
            IBlogProvider blogProvider,
            IMediaProvider mediaProvider)
        {
            _communityProvider = communityProvider;
            _blogProvider = blogProvider;
            _mediaProvider = mediaProvider;

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

            var cts = new CancellationTokenSource();
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting Selected Communities";
                App.Variables.MultiBlog.Communities = SelectableCommunities.Where(sc => sc.IsSelected).Select(sc => sc.Item);

                IsBusyData.IsProgessBarVisible = true;

                IsBusyData.Description = "Uploading Images";
                var imageItems = new List<ImageItem>();
                var imagesIterator = 0;
                foreach (var imageSource in App.Variables.MultiBlog.BlogImageSources)
                {
                    var imageItem = await _mediaProvider.UploadImage(imageSource.MemoryStream);
                    imageItems.Add(imageItem);
                    imagesIterator++;

                    IsBusyData.Progress = imagesIterator / (float)App.Variables.MultiBlog.BlogImageSources.Count;
                }

                IsBusyData.Progress = 0f;

                var blog = App.Variables.MultiBlog.Blog;
                var i = 0;
                foreach (var community in App.Variables.MultiBlog.Communities)
                {
                    IsBusyData.Description = $"Sending blog to {community.Name}";
                    if (i != 0) IsBusyData.Progress = i / (float)App.Variables.MultiBlog.Communities.Count();
                    await _blogProvider.PostBlog(community.Id, blog.Title, blog.Content, imageItems);
                    i++;
                    if (!cts.IsCancellationRequested) continue;

                    await Page.DisplayAlert("Canceled", $"Your blog has been posted on {i} communities, we can't change that.", "Ok");
                    return;
                }
                await Page.DisplayAlert("Succes!",
                    $"Your blog has been posted on {i} communities", "Ok");
            }, cts);

            
            await App.MainNavigation.PopToRootAsync();
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
            var communities = await DoAsBusyState(_communityProvider.GetAllJoinedCommunities());
            var selectableCommunties = GetSelectableCommuntiesByCommunities(communities);
            SelectableCommunities = new ObservableRangeCollection<SelectableItem<AminoApi.Models.Community.Community>>(selectableCommunties);
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
            OnPropertyChanged(nameof(SendButtonText));
            OnPropertyChanged(nameof(SendButtonIsEnabled));
        }
    }
}

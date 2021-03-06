﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoTools.Pages;
using AminoTools.Pages.Blogs;
using AminoTools.Pages.Chatting;
using AminoTools.Pages.Community;
using AminoTools.Pages.Profile;
using AminoTools.Pages.Settings;
using AminoTools.ViewModels.Contracts;
using MvvmHelpers;
using Xamarin.Forms;
using MenuItem = AminoTools.Models.MainPageMenu.MenuItem;

namespace AminoTools.ViewModels
{
    public class MainPageMenuPageViewModel : BaseViewModel, IMainPageMenuPageViewModel
    {
        private ObservableRangeCollection<MenuItem> _menuItems;
        private MenuItem _selectedItem;

        public ObservableRangeCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value; 
                OnPropertyChanged();
            }
        }

        public MenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

                if (!IsInitializing && value != null) OnMenuItemTapped(value);
            }
        }

        public Account Account => ((App) Application.Current).Account;
        public Command UserIconTappedCommand { get; }

        private async void OnMenuItemTapped(MenuItem menuItem)
        {
            // Reset the menu item selection status

            var page = (Page) Activator.CreateInstance(menuItem.TargetPageType);
            await App.SetMainPage(page);
        }

        public MainPageMenuPageViewModel()
        {
            UserIconTappedCommand = new Command(DoGoToUserProfile);
            Initialize += MainPageMenuPageViewModel_Initialize;
        }

        private async void DoGoToUserProfile()
        {
            App.Variables.ProfilePage.Reset();
            App.Variables.ProfilePage.IsMyProfile = true;
            await App.SetMainPage(new ProfilePage());
        }

        private void MainPageMenuPageViewModel_Initialize(object sender, EventArgs e)
        {
            MenuItems = new ObservableRangeCollection<MenuItem>();
            MenuItems.AddRange(new[]
            {
                new MenuItem
                {
                    Title  = "Home",
                    TargetPageType = typeof(HomePage)
                },
                new MenuItem
                {
                    Title  = "Chats",
                    TargetPageType = typeof(GlobalChattingPage)
                },
                new MenuItem
                {
                    Title = "Blogs",
                    TargetPageType = typeof(BlogsPage)
                },
                new MenuItem
                {
                    Title = "Communities",
                    TargetPageType = typeof(CommunityPage)
                },
                new MenuItem
                {
                    Title = "Settings",
                    TargetPageType = typeof(SettingsPage)
                }
            });
#if DEBUG
            MenuItems.Add(new MenuItem
            {
                Title = "Debug Test Page",
                TargetPageType = typeof(TestPage)
            });
#endif

            SelectedItem = MenuItems.First();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoTools.Pages;
using AminoTools.Pages.Blogs;
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

        private async void OnMenuItemTapped(MenuItem menuItem)
        {
            // Reset the menu item selection status

            var page = (Page) Activator.CreateInstance(menuItem.TargetPageType);
            await App.SetMainPage(page);
        }

        public MainPageMenuPageViewModel()
        {
            Initialize += MainPageMenuPageViewModel_Initialize;
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
                    Title = "Blogs",
                    TargetPageType = typeof(BlogsPage)
                },
                new MenuItem
                {
                    Title = "Settings",
                    TargetPageType = typeof(SettingsPage)
                }
            });
            SelectedItem = MenuItems.First();
        }
    }
}

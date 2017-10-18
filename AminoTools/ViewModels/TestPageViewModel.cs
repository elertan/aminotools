using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Providers;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class TestPageViewModel : BaseViewModel, ITestPageViewModel
    {
        private readonly IBlogProvider _blogProvider;
        private readonly IDatabaseProvider _databaseProvider;
        private Command _buttonCommand;
        private Command _destroyDatabaseCommand;

        public Command ButtonCommand
        {
            get => _buttonCommand;
            set
            {
                _buttonCommand = value; 
                OnPropertyChanged();
            }
        }

        public Command DestroyDatabaseCommand
        {
            get => _destroyDatabaseCommand;
            set
            {
                _destroyDatabaseCommand = value; 
                OnPropertyChanged();
            }
        }

        public TestPageViewModel(IBlogProvider blogProvider, IDatabaseProvider databaseProvider)
        {
            _blogProvider = blogProvider;
            _databaseProvider = databaseProvider;
            Initialize += TestPageViewModel_Initialize;
        }

        private void TestPageViewModel_Initialize(object sender, EventArgs e)
        {
            ButtonCommand = new Command(DoTest);  
            DestroyDatabaseCommand = new Command(DoDestroyDatabase);
        }

        private async void DoDestroyDatabase()
        {
            var accepted = await Page.DisplayAlert("Are you sure?", "Are you sure you want to destroy all data?", "Yes", "No");
            if (!accepted) return;

            var db = await _databaseProvider.GetDatabaseAsync();
            await db.DestroyAsync();
        }

        private async void DoTest()
        {
            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Getting Blogs";
                await Task.Delay(2000);
                IsBusyData.Description = "Cleaning up";
                await Task.Delay(1000);
                IsBusyData.Description = "Updating view!";
                await Task.Delay(1000);
                IsBusyData.Description = "Finished : )!";
                await Task.Delay(1000);
            });
        }
    }
}

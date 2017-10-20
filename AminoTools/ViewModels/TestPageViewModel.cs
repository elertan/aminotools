using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoApi.Models.User;
using AminoTools.Providers;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts;
using SQLiteNetExtensionsAsync.Extensions;
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

        public Command DatabaseTestCommand { get; }

        public TestPageViewModel(IBlogProvider blogProvider, IDatabaseProvider databaseProvider)
        {
            _blogProvider = blogProvider;
            _databaseProvider = databaseProvider;

            ButtonCommand = new Command(DoTest);
            DestroyDatabaseCommand = new Command(DoDestroyDatabase);
            DatabaseTestCommand = new Command(DoDatabaseTest);

            Initialize += TestPageViewModel_Initialize;
        }

        private void TestPageViewModel_Initialize(object sender, EventArgs e)
        {
            
        }

        private async void DoDatabaseTest()
        {
            try
            {
                await DoAsBusyStateCustom(async () =>
                {
                    IsBusyData.Description = "Storing into db";

                    var chat = new Chat();
                    chat.ThreadId = "thread1";
                    chat.Members = new List<UserProfile>
                    {
                        new UserProfile {Nickname = "User1", Uid = "id1"},
                        new UserProfile {Nickname = "User2", Uid = "id2"}
                    };
                    chat.Community = new AminoApi.Models.Community.Community
                    {
                        Id = "id1",
                        Name = "Shit"
                    };

                    var db = await _databaseProvider.GetDatabaseAsync();
                    await db.Connection.InsertWithChildrenAsync(chat);

                    IsBusyData.Description = "Reading data from db";
                    var communities = await db.Connection.Table<AminoApi.Models.Community.Community>().ToListAsync();

                    if (!communities.Any())
                    {
                        IsBusyData.Description = "Database returned no results";
                        await Task.Delay(2000);
                    }

                    foreach (var c in communities)
                    {
                        IsBusyData.Description = c.Name;
                        await Task.Delay(2000);
                    }
                });
            }
            catch (Exception ex)
            {
                await Page.DisplayAlert("Exception Thrown", ex.Message, "Ok");
                throw ex;
            }
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

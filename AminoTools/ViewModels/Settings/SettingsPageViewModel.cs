using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts.Settings;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Settings
{
    public class SettingsPageViewModel : BaseViewModel, ISettingsPageViewModel
    {
        private readonly IDatabaseProvider _databaseProvider;

        public SettingsPageViewModel(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            Initialize += SettingsPageViewModel_Initialize;
            DeleteLocalStorageCommand = new Command(DoDeleteLocalStorage);
        }

        private async void DoDeleteLocalStorage()
        {
            var accepted = await Page.DisplayAlert("Are you sure?", "Deleting the local storage resets any saved data",
                "Delete", "Cancel");
            if (!accepted) return;

            await DoAsBusyStateCustom(async () =>
            {
                IsBusyData.Description = "Deleting Local Database";
                var database = await _databaseProvider.GetDatabaseAsync();
                await database.DestroyAsync();
            });

            await Page.DisplayAlert("Success", "Deleted Local Storage", "Ok");
        }

        private void SettingsPageViewModel_Initialize(object sender, EventArgs e)
        {
            
        }

        public Command DeleteLocalStorageCommand { get; }
    }
}

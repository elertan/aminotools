using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoTools.Databases;
using AminoTools.DependencyServices;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class DatabaseProvider :  IDatabaseProvider
    {
        public async Task<Database> GetDatabaseAsync()
        {
            var database = new Database(((App) Application.Current).DbConnection);
            await database.InitializeAsync();
            return database;
        }
    }
}

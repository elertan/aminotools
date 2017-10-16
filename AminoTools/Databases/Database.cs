using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoApi.Models.User;
using SQLite;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace AminoTools.Databases
{
    public class Database
    {
        public readonly SQLiteAsyncConnection Connection;

        public Database(SQLiteAsyncConnection connection)
        {
            Connection = connection;
        }

        public async Task InitializeAsync()
        {
            // Add Tables here
            await Connection.CreateTablesAsync(
                typeof(Chat),
                typeof(UserProfile));
        }
    }
}

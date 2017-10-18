using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;
using AminoApi.Models.Media;
using AminoApi.Models.User;
using SQLite;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace AminoTools.Databases
{
    public class Database
    {
        public readonly SQLiteAsyncConnection Connection;
        // Order matters.
        private readonly Type[] _tableTypes =
        {
            typeof(ImageItem),
            typeof(Chat),
            typeof(UserProfile),
            typeof(Community),
            typeof(Message)
        };

        public Database(SQLiteAsyncConnection connection)
        {
            Connection = connection;
        }

        public async Task InitializeAsync()
        {
            // Add Tables here
            await Connection.CreateTablesAsync(_tableTypes);
        }

        public async Task DestroyAsync()
        {
            foreach (var tableType in _tableTypes)
            {
                await Connection.DropTableAsync(tableType);
            }
        }
    }
}

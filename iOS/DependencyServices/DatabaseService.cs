using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AminoTools.DependencyServices;
using AminoTools.iOS.DependencyServices;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseService))]
namespace AminoTools.iOS.DependencyServices
{
    public class DatabaseService : IDatabaseService
    {
        private const string DbFileName = "AminoToolsDb.db";
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryFolder = Path.Combine(documentsPath, "..", "Library", "Database");
            if (!Directory.Exists(libraryFolder)) Directory.CreateDirectory(libraryFolder);
            var filePath = Path.Combine(libraryFolder, DbFileName);
            return new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(new SQLitePlatformIOS(), new SQLiteConnectionString(filePath, true)));
        }
    }
}

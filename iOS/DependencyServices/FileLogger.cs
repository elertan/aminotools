using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.DependencyServices;
using AminoTools.iOS.DependencyServices;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(FileLogger))]
namespace AminoTools.iOS.DependencyServices
{
    public class FileLogger : IFileLogger
    {
        public string GetLibraryPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Resources);
        }

        public void Log(string filename, string content)
        {
            var libraryPath = GetLibraryPath();
            var filePath = Path.Combine(libraryPath, filename);

            File.WriteAllText(filePath, content);
        }

        public string GetContent(string filename)
        {
            var libraryPath = GetLibraryPath();
            var filePath = Path.Combine(libraryPath, filename);

            if (!File.Exists(filePath)) return null;

            return File.ReadAllText(filePath);
        }

        public async Task<string> GetContentAsync(string filename)
        {
            var libraryPath = GetLibraryPath();
            var filePath = Path.Combine(libraryPath, filename);

            if (!File.Exists(filePath)) return null;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        public void DeleteFile(string filename)
        {
            var libraryPath = GetLibraryPath();
            var filePath = Path.Combine(libraryPath, filename);

            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }
    }
}
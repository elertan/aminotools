using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.DependencyServices
{
    public interface IFileLogger
    {
        string GetLibraryPath();
        void Log(string filename, string content);
        string GetContent(string filename);
        Task<string> GetContentAsync(string filename);
        void DeleteFile(string filename);
    }
}

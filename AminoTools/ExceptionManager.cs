using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using AminoTools.DependencyServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools
{
    public static class ExceptionManager
    {
        private const string exceptionFile = "fatal.log";

        public static void ReportException(Exception ex)
        {
#if DEBUG
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Exception: " + ex.Message);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Stack Trace: " + ex.StackTrace);
            Debug.WriteLine(stringBuilder.ToString());
#endif

            var json = JsonConvert.SerializeObject(ex);
            var fileLogger = DependencyService.Get<IFileLogger>();
            fileLogger.Log(exceptionFile, json);
        }

        public static async Task<Exception> GetExceptionAsync()
        {
            var fileLogger = DependencyService.Get<IFileLogger>();
            var json = await fileLogger.GetContentAsync(exceptionFile);
            return json == null ? null : JsonConvert.DeserializeObject<Exception>(json);
        }

        public static void ClearExceptionAsync()
        {
            var fileLogger = DependencyService.Get<IFileLogger>();
            fileLogger.DeleteFile(exceptionFile);
        }
    }
}

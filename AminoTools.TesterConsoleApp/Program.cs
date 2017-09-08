using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Auth;
using AminoApi.Models.Media;

namespace AminoTools.TesterConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync();
            Console.ReadKey();
        }

        private static async void MainAsync()
        {
            var api = new Api(new HttpClient());

            while (true)
            {
                Console.Write("Enter username: ");
                var email = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Enter password: ");
                var password = Console.ReadLine();
                Console.WriteLine();

                Console.WriteLine("Logging in...");
                var loginResult = await api.LoginAsync(email, password);
                if (loginResult.DidSucceed())
                {
                    api.Sid = loginResult.Data.Sid;
                    break;
                }
                Console.Clear();
                Console.WriteLine("Error:\n" + loginResult.Info.Message);
                Console.ReadKey();
                Console.Clear();
            }

            await DoTest(api);
        }

        private static async Task DoTest(Api api)
        {
            var apiResult = await api.GetJoinedChatsAsync("24");
            Console.WriteLine(apiResult.DidSucceed());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi;

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
            var loginResult = await api.Global.S.Auth.Login("denkievits@gmail.com", "4dpwpagvw");


        }
    }
}

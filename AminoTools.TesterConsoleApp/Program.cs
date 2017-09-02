using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
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
            Console.WriteLine("Logging in...");
            var loginResult = await api.Login("myemail", "mypass");
            api.Sid = loginResult.Data.Sid;

            Console.WriteLine("Getting joined communities");
            var communitiesResult = await api.GetJoinedCommunities();
            if (communitiesResult.DidSucceed())
            {
                foreach (var community in communitiesResult.Data.Communities)
                {
                    Console.WriteLine($"{community.Name} ({community.AmountOfMembers} Members) - {community.Tagline}");
                }

                Console.WriteLine("Posting new blog");
                var com = communitiesResult.Data.Communities.First(c => c.Name == "Test Amino Elertan");

                var postBlogResult = await api.PostBlog(com.Id, "TestBlog123", "Some ExampleContent", new ImageItem[0]);
            }
            else
            {
                Console.WriteLine($"Error: {communitiesResult.Info.Message}");
            }
        }
    }
}

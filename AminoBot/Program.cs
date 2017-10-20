using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.User;

namespace AminoBot
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

            await DoBot(api);
        }

        private static async Task DoBot(IApi api)
        {
            const string communityId = "50653388";
            const string welcomeMessage =
                "Hey, welcome to DA! 😊\r\n\r\nI\'m Alavon, and I\'m here to help you out if you have any questions about the community.\r\n\r\nIf you prefer just reading through some of the basics, then this post will help you out a lot http://aminoapps.com/p/j2y0am 😄.\r\n\r\nFeel free to talk to me whenever you like!\r\n\r\nEnjoy your stay ❤️";
            var passedMemberIds = new List<string>();
            var delay = TimeSpan.FromSeconds(10);
            Console.WriteLine("Bot started");
            var startMembersApiResult = await api.GetMembersForCommunityAsync(communityId, MemberType.RecentlyJoined);
            passedMemberIds.AddRange(startMembersApiResult.Data.UserProfiles.Select(up => up.Uid));

            while (true)
            {
                Console.WriteLine($"Waiting for {delay.Seconds} seconds...");
                await Task.Delay(delay);
                var membersApiResult = await api.GetMembersForCommunityAsync(communityId, MemberType.RecentlyJoined);
                if (!membersApiResult.DidSucceed())
                {
                    Console.WriteLine("Request Failure");
                    continue;
                }
                var newMembers = membersApiResult.Data.UserProfiles.Where(up => !passedMemberIds.Contains(up.Uid)).Select(up => up.Uid).ToList();
                foreach (var newMember in newMembers)
                {
                    Console.WriteLine("Commenting for new member");
                    await api.PostCommentOnUsersWallForCommunityAsync(communityId, newMember, welcomeMessage);
                }
                passedMemberIds.AddRange(newMembers);
            }
        }
    }
}

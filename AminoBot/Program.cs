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
        static string uid = null;

        private static async Task Main(string[] args)
        {
            var api = new Api(new HttpClient());

            while (true)
            {
                Console.Write("Enter phonenumber: ");
                var phonenumber = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Enter password: ");
                var password = Console.ReadLine();
                Console.WriteLine();

                Console.WriteLine("Logging in...");
                var loginResult = await api.LoginByPhonenumberAsync(phonenumber, password);
                if (loginResult.DidSucceed())
                {
                    api.Sid = loginResult.Data.Sid;
                    uid = loginResult.Data.Uid;
                    break;
                }
                Console.Clear();
                Console.WriteLine("Error:\n" + loginResult.Info.Message);
                Console.ReadKey();
                Console.Clear();
            }

            await DoBot1(api);
            //await LeaveGroupChats(api);
        }

        private static async Task DoBot(IApi api)
        {
            // League of legends amino is 24 and anime is 3
            var rng = new Random();
            const string communityId = "3";

            var passedUserIds = new List<string>();
            string lastPageToken = null;
            var count = 0;
            while (true)
            {
                count++;
                Console.WriteLine("Grabbing feed");
                var feed = await api.GetBlogFeedForCommunityAsync(communityId, 25, lastPageToken);
                if (!feed.DidSucceed())
                {
                    Console.WriteLine("Error occured?");
                    break;
                }
                if (feed.Data.Blogs.Count == 0)
                {
                    Console.WriteLine("No blogs left?");
                    break;
                }
                lastPageToken = feed.Data.Paging.NextPageToken;
                if (count == 10)
                {
                    count = 0;
                    lastPageToken = null;
                }
                var newBlogs = feed.Data.Blogs.Where(x => !passedUserIds.Contains(x.Author.UserId));
                var nicknames = newBlogs.Select(x => x.Author.Nickname).Aggregate((x1, x2) => x1 + ", " + x2);
                Console.WriteLine($"Following {nicknames} ...");
                await api.FollowMembers(communityId, uid, newBlogs.Select(x => x.Author.UserId).ToArray());
                // Ignore passed users and loop
                foreach (var blog in newBlogs)
                {
                    passedUserIds.Add(blog.Author.UserId);

                    for (var i = 0; ; i++)
                    {
                        if (i == 3)
                        {
                            break;
                        }
                        await Task.Delay(rng.Next(200, 500));
                        var blogs = await api.GetBlogsByUserIdAsync(communityId, blog.Author.UserId, i * 25, 25);

                        if (blogs.Data.Blogs.Count == 0)
                        {
                            break;
                        }

                        foreach (var b in blogs.Data.Blogs.Take(8))
                        {
                            Console.WriteLine($"Liking blog by {b.Author.Nickname} \"{b.Title}\"");
                            await api.VoteBlog(communityId, b.BlogId, AminoApi.Models.Blog.VoteValue.Like, AminoApi.Models.Blog.VoteEventSource.FeedList);
                            await Task.Delay(rng.Next(50, 200));
                        }
                    }
                }
            }
        }

        private static async Task DoBot1(IApi api)
        {
            // League of legends amino is 24 and anime is 3
            var rng = new Random();
            const string communityId = "3";

            for (var i = 14000; ; i++) // 13139
            {
                Console.WriteLine("At " + i);
                var members = await api.GetMembersForCommunityAsync(communityId, MemberType.RecentlyJoined, i * 50, 50);
                //var nicknames = members.Data.UserProfiles.Select(x => x.Nickname).Aggregate((x1, x2) => x1 + ", " + x2);
                //Console.WriteLine($"Following {nicknames} ...");
                await Task.Delay(500);
                await api.FollowMembers(communityId, uid, members.Data.UserProfiles.Select(x => x.Uid).ToArray());
                await Task.Delay(500);
            }
        }

        private static async Task LeaveGroupChats(IApi api)
        {
            // League of legends amino is 24 and anime is 3
            var rng = new Random();
            const string communityId = "3";

            for (var i = 0; ; i++) // 13139
            {
                Console.WriteLine("At " + i);
                var chats = await api.GetJoinedChatsAsync(communityId, i * 25, 25);
                if (chats.Data.Chats.Count == 0)
                {
                    break;
                }
                // Where group chat or where I am the only one in the chat
                foreach (var chat in chats.Data.Chats.Where(x => x.Title != null || x.Members.All(y => y.Uid == uid)))
                {
                    Console.WriteLine($"Removing {chat.Title}");
                    await api.RemoveFromChatAsync(communityId, chat.ThreadId, uid);
                }
            }
        }
    }
}

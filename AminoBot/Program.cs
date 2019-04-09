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
                if (count == 5)
                {
                    count = 0;
                    lastPageToken = null;
                }

                // Ignore passed users and loop
                foreach (var blog in feed.Data.Blogs.Where(x => !passedUserIds.Contains(x.Author.UserId)))
                {
                    passedUserIds.Add(blog.Author.UserId);

                    for (var i = 0; ; i++)
                    {
                        if (i == 1)
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
                            await Task.Delay(rng.Next(200, 500));
                        }
                        
                    }
                }
            }
        }
    }
}

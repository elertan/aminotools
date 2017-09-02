using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Feed
{
    public class FeedHeadlines : ModelBase
    {
        private int _amountOfCommunitiesJoined;
        private IEnumerable<Community.Community> _communities;
        private IEnumerable<HeadLineBlog> _blogs;

        public int AmountOfCommunitiesJoined
        {
            get => _amountOfCommunitiesJoined;
            set
            {
                _amountOfCommunitiesJoined = value; 
                OnPropertyChanged();
            }
        }

        public IEnumerable<Community.Community> Communities
        {
            get => _communities;
            set
            {
                _communities = value; 
                OnPropertyChanged();
            }
        }

        public IEnumerable<HeadLineBlog> Blogs
        {
            get => _blogs;
            set
            {
                _blogs = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Blogs = new List<HeadLineBlog>();

            AmountOfCommunitiesJoined = Convert.ToInt32(data["numberOfJoinedCommunities"]);

            var communityJObject = (JObject)data["communityInfoMapping"];
            var communities = new List<Dictionary<string, object>>();

            foreach (var x in communityJObject)
            {
                communities.Add(x.Value.ToObject<Dictionary<string, object>>());
            }

            Communities = communities.Select(c =>
            {
                var community = new Community.Community();
                community.JsonResolve(c);
                return community;
            }).ToList();

            var blogsJArray = (JArray)data["headlinePostList"];
            var blogs = blogsJArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            var headLineBlogs = blogs.Select(b =>
            {
                var hlBlog = new HeadLineBlog();
                hlBlog.JsonResolve(b);
                return hlBlog;
            }).ToList();
            Blogs = headLineBlogs;
            foreach (var blog in Blogs)
            {
                blog.Community = Communities.First(c => c.Id == blog.CommunityId);
            }
        }
    }
}
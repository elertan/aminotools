﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoApi.Models.Blog;
using AminoApi.Models.Community;
using AminoApi.Models.Feed;
using AminoApi.Models.Media;

namespace AminoApi
{
    public interface IApi
    {
        string Version { get; set; }
        string DeviceId { get; set; }
        string Sid { get; set; }

        Task<ApiResult<Account>> Login(string email, string password);

        Task<ApiResult<CommunityList>> GetJoinedCommunities(int start = 0, int size = 50);

        Task<ApiResult<CommunityCollectionResponse>> GetCommunityCollectionBySections(int start = 0, int size = 25, string languageCode = "en");

        Task<ApiResult<BlogList>> GetBlogsByUserIdAsync(string communityId, string userId,
            int start = 0, int size = 25);

        Task<ApiResult<Blog>> PostBlog(string communityId, string title, string content,
            IEnumerable<ImageItem> imageItems = null);

        Task<ApiResult<FeedHeadlines>> GetFeedHeadlines(int start = 0, int size = 25);
    }
}

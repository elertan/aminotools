﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Blog;
using AminoApi.Models.Media;

namespace AminoTools.Providers.Contracts
{
    public interface IBlogProvider
    {
        Task<ApiResult<Blog>> PostBlog(string communityId, string title, string content, IEnumerable<ImageItem> imageItems = null);
        Task<ApiResult<IEnumerable<Blog>>> GetBlogsByUserIdAsync(string communtityId, string userId, int index = 0, int amount = 25);
    }
}

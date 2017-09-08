using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models;
using AminoApi.Models.Blog;
using AminoApi.Models.Media;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class BlogProvider : Provider, IBlogProvider
    {
        public BlogProvider(IApi api) : base(api)
        {
            
        }

        public async Task<ApiResult<Blog>> PostBlog(string communityId, string title, string content, IEnumerable<ImageItem> imageItems = null)
        {
            var result = await Api.PostBlogAsync(communityId, title, content, imageItems);
            return ApiResult.Create(result.Data, result.Info);
        }

        public async Task<ApiResult<IEnumerable<Blog>>> GetBlogsByUserIdAsync(string communtityId, string userId, int index = 0, int amount = 25)
        {
            var result = await Api.GetBlogsByUserIdAsync(communtityId, userId, index, amount);
            if (!result.DidSucceed()) throw new Exception(result.Info.Message);
            return ApiResult.Create((IEnumerable<Blog>)result.Data.Blogs, result.Info);
        }
    }
}

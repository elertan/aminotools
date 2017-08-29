using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models;
using AminoApi.Models.Blog;

namespace AminoTools.Providers
{
    public class BlogProvider : Provider, IBlogProvider
    {
        public BlogProvider(Api api) : base(api)
        {
            
        }

        public async Task<IEnumerable<Blog>> GetBlogsByUserIdAsync(string communtityId, string userId, int index = 0, int amount = 25)
        {
            var result = await Api.S.GetBlogsByUserIdAsync(communtityId, userId, index, amount);
            if (!result.DidSucceed()) throw new Exception(result.Info.Message);
            return result.Data.Blogs;
        }
    }
}

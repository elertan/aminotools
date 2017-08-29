using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Blog;

namespace AminoTools.Providers
{
    public interface IBlogProvider
    {
        Task<IEnumerable<Blog>> GetBlogsByUserIdAsync(string communtityId, string userId, int index = 0, int amount = 25);
    }
}

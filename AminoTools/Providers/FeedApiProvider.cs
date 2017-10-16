using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Feed;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class FeedApiProvider : ApiProvider, IFeedProvider
    {
        public FeedApiProvider(IApi api) : base(api) { }

        public async Task<ApiResult<FeedHeadlines>> GetFeedHeadlines(int index = 0, int amount = 25)
        {
            var result = await Api.GetFeedHeadlinesAsync(index, amount);
            return result;
        }
    }
}

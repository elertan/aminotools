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
    public class FeedProvider : Provider, IFeedProvider
    {
        public FeedProvider() : base(((App)Application.Current).Api)
        {

        }

        public async Task<FeedHeadlines> GetFeedHeadlines(int index = 0, int amount = 25)
        {
            var result = await Api.GetFeedHeadlines(index, amount);
            return result.Data;
        }
    }
}

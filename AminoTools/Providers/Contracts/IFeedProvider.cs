using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Feed;

namespace AminoTools.Providers.Contracts
{
    public interface IFeedProvider
    {
        Task<FeedHeadlines> GetFeedHeadlines(int index = 0, int amount = 25);
    }
}

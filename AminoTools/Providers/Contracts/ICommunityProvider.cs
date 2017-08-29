using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Community;

namespace AminoTools.Providers.Contracts
{
    public interface ICommunityProvider
    {
        Task<IEnumerable<Community>> GetJoinedCommunities(int index = 0, int amount = 50);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Chat;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class ChatProvider : Provider, IChatProvider
    {
        public ChatProvider(IApi api) : base(api)
        {
        }


        public async Task<ApiResult<List<Chat>>> GetChatsByCommunityAsync(string communityId)
        {
            var chats = new List<Chat>();
            for (var i = 0;; i++)
            {
                var threadListResult = await Api.GetJoinedChatsAsync(communityId, i * 25);
                if (!threadListResult.DidSucceed() || !threadListResult.Data.Chats.Any())
                {
                    return ApiResult.Create(chats, threadListResult.Info);
                }
                chats.AddRange(threadListResult.Data.Chats);
            }
        }

        public async Task<ApiResult<MessageList>> GetMessagesAsync(string communityId, string threadId, int index = 0)
        {
            return await Api.GetMessagesForUserByCommunityIdAsync(communityId, threadId, index * 25);
        }
    }
}

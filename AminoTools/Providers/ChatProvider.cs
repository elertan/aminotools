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
                var threadListResult = await Api.GetJoinedChatsAsync(communityId, i * 50, 50);
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

        public async Task<ApiResult<Message>> SendMessageToChatAsync(string communityId, string threadId, string content)
        {
            return await Api.SendMessageToChatAsync(communityId, threadId, content);
        }

        public async Task<ApiResult<Message>> SendImageToChatAsync(string communityId, string threadId, string base64JpgImageContent)
        {
            return await Api.SendImageToChatAsync(communityId, threadId, base64JpgImageContent);
        }
    }
}

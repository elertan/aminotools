using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class ChatApiProvider : ApiProvider, IChatProvider
    {
        private readonly IDatabaseProvider _databaseProvider;

        public ChatApiProvider(IApi api, IDatabaseProvider databaseProvider) : base(api)
        {
            _databaseProvider = databaseProvider;
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

        public async Task StoreChatsForCommunityAsync(List<Chat> chats, Community community)
        {
            foreach (var dataChat in chats)
            {
                dataChat.Community = community;
                dataChat.CommunityId = community.Id;
            }
            var database = await _databaseProvider.GetDatabaseAsync();
            var storedChats = await database.Connection.Table<Chat>().ToListAsync();
            foreach (var chat in chats)
            {
                if (storedChats.Any(c => c.ThreadId == chat.ThreadId)) continue;
                await database.Connection.InsertAsync(chat);
            }
            foreach (var chatDbModel in storedChats)
            {
                if (chats.All(c => c.ThreadId != chatDbModel.ThreadId))
                    await database.Connection.DeleteAsync(chatDbModel);
            }
        }

        public async Task<List<Chat>> GetStoredChatsAsync()
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            var chats = await db.Connection.Table<Chat>().ToListAsync();
            return chats;
        }
    }
}

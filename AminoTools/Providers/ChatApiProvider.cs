using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;
using AminoTools.Providers.Contracts;
using SQLiteNetExtensionsAsync.Extensions;

namespace AminoTools.Providers
{
    public class ChatApiProvider : ApiProvider, IChatProvider
    {
        private readonly IDatabaseProvider _databaseProvider;
        private readonly ICommunityProvider _communityProvider;
        private readonly IUserProvider _userProvider;

        public ChatApiProvider(IApi api, 
            IDatabaseProvider databaseProvider, 
            ICommunityProvider communityProvider,
            IUserProvider userProvider) : base(api)
        {
            _databaseProvider = databaseProvider;
            _communityProvider = communityProvider;
            _userProvider = userProvider;
        }


        public async Task<ApiResult<List<Chat>>> GetChatsByCommunityAsync(Community community)
        {
            var chats = new List<Chat>();
            for (var i = 0;; i++)
            {
                var threadListResult = await Api.GetJoinedChatsAsync(community.Id, i * 50, 50);
                if (!threadListResult.DidSucceed() || !threadListResult.Data.Chats.Any())
                {
                    // Add missing information
                    foreach (var chat in chats)
                    {
                        chat.Community = community;
                        chat.CommunityId = community.Id;
                        if (chat.LastMessage != null) chat.LastMessageId = chat.LastMessage.Id;
                    }
                    // Store to database
                    //if (chats.Any()) await StoreChatsAsync(chats);
                    return ApiResult.Create(chats, threadListResult.Info);
                }
                chats.AddRange(threadListResult.Data.Chats);
            }
        }

        public async Task<ApiResult<List<ThreadCheck>>> CheckForNewMessagesAsync(string communityId)
        {
            var apiResult = await Api.ThreadCheckAsync(communityId);
            return ApiResult.Create(apiResult.Data.ThreadChecks, apiResult.Info);
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

        public async Task<List<Chat>> GetStoredChatsAsync()
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            var chats = await db.Connection.GetAllWithChildrenAsync<Chat>();
            foreach (var chat in chats)
            {
                if (!string.IsNullOrWhiteSpace(chat.LastMessageId))
                {
                    chat.LastMessage = await GetStoredMessageAsync(chat.LastMessageId);
                }
                chat.Community = await _communityProvider.GetStoredCommunityAsync(chat.CommunityId);
            }
            return chats;
        }

        public async Task StoreChatsAsync(List<Chat> chats)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            foreach (var chat in chats)
            {
                if (chat.LastMessage != null) await StoreMessagesAsync(chat.LastMessage);
                await _communityProvider.StoreCommunitiesAsync(chat.Community);
            }
            await db.Connection.InsertOrReplaceWithChildrenAsync(chats, true);
        }

        private async Task StoreMessagesAsync(params Message[] messages)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            await db.Connection.InsertOrReplaceWithChildrenAsync(messages, true);
        }

        private async Task<Message> GetStoredMessageAsync(string messageId)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            return await db.Connection.Table<Message>().Where(m => m.Id == messageId).FirstAsync();
        }
    }
}

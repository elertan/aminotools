using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using MvvmHelpers;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Chatting
{
    public interface IChatPageViewModel : IViewModel
    {
        ChatCommunityModel ChatCommunityModel { get; }
        ObservableRangeCollection<MessageInfoModel> Messages { get; set; }
        Command LoadMoreMessagesCommand { get; }
        Command SendMessageCommand { get; }
        string Message { get; set; }
        bool IsRefreshing { get; set; }
    }
}

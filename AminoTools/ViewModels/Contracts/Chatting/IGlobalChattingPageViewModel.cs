using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;

namespace AminoTools.ViewModels.Contracts.Chatting
{
    public interface IGlobalChattingPageViewModel
    {
        ObservableRangeCollection<object> Chats { get; set; }
    }
}

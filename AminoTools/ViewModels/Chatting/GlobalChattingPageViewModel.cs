using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels.Contracts.Chatting;
using MvvmHelpers;

namespace AminoTools.ViewModels.Chatting
{
    public class GlobalChattingPageViewModel : BaseViewModel, IGlobalChattingPageViewModel
    {
        private ObservableRangeCollection<object> _chats;

        public GlobalChattingPageViewModel()
        {
            Initialize += GlobalChattingPageViewModel_Initialize;
        }

        private void GlobalChattingPageViewModel_Initialize(object sender, EventArgs e)
        {
            Chats = new ObservableRangeCollection<object>
            {
                new object()
            };
        }

        public ObservableRangeCollection<object> Chats
        {
            get => _chats;
            set
            {
                _chats = value; 
                OnPropertyChanged();
            }
        }
    }
}

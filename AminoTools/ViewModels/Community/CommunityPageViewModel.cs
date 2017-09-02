using AminoTools.ViewModels.Contracts.Community;

namespace AminoTools.ViewModels.Community
{
    public class CommunityPageViewModel : BaseViewModel, ICommunityPageViewModel
    {
        public CommunityPageViewModel()
        {
            Initialize += CommunityPageViewModel_Initialize;
        }

        private void CommunityPageViewModel_Initialize(object sender, System.EventArgs e)
        {
            
        }
    }
}

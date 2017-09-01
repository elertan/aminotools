namespace AminoTools.Models
{
    public class IsBusyData : BaseModel
    {
        private string _description;
        private float _progress;
        private bool _isBusy;

        public string Description
        {
            get => _description;
            set
            {
                _description = value; 
                OnPropertyChanged();
            }
        }

        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value; 
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value; 
                OnPropertyChanged();
            }
        }
    }
}

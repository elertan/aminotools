using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.Models.MainPageMenu
{
    public class MenuItem : BaseModel
    {
        private string _title;
        private Type _targetPageType;

        public string Title
        {
            get => _title;
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public Type TargetPageType
        {
            get => _targetPageType;
            set
            {
                _targetPageType = value; 
                OnPropertyChanged();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.Models
{
    public class SelectableItem<T> : BaseModel
    {
        private T _item;
        private bool _isSelected;

        public event EventHandler IsSelectedChanged;

        public T Item
        {
            get => _item;
            set
            {
                _item = value; 
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value; 
                OnPropertyChanged();
                OnIsSelectedChanged();
            }
        }

        protected virtual void OnIsSelectedChanged()
        {
            IsSelectedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

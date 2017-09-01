using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Annotations;
using AminoTools.Models;

namespace AminoTools
{
    public class IsBusyTask : Task, INotifyPropertyChanged
    {
        private IsBusyData _data;

        public IsBusyData Data
        {
            get => _data;
            set
            {
                _data = value; 
                OnPropertyChanged();
            }
        }

        public IsBusyTask(Action action) : base(action)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

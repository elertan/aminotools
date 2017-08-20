using System.ComponentModel;
using System.Runtime.CompilerServices;
using AminoApi.Annotations;

namespace AminoApi.Models
{
    public class ModelBase : INotifyPropertyChanged, IModelResolvable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Resolve(dynamic data)
        {
            throw new System.NotImplementedException("You forget to override Resolve or you let a base call through");
        }
    }
}
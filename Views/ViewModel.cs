using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace axion.Views
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        protected virtual void OnPropertyChanged(string? propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}

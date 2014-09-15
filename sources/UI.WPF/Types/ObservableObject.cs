using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Queue.UI.WPF.Types
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                var pc = PropertyChanged;
                if (pc != null)
                {
                    pc(this, new PropertyChangedEventArgs(propName));
                }
            }
        }
    }
}
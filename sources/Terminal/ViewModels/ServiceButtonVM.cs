using Junte.UI.WPF;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Queue.Terminal.ViewModels
{
    public class ServiceButtonVM : ObservableObject
    {
        private string name;
        private string code;
        private SolidColorBrush serviceBrush;

        private Lazy<ICommand> selectServiceCommand;

        public ICommand SelectServiceCommand { get { return this.selectServiceCommand.Value; } }

        public event EventHandler OnServiceSelected;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }

        public SolidColorBrush ServiceBrush
        {
            get { return serviceBrush; }
            set { SetProperty(ref serviceBrush, value); }
        }

        public ServiceButtonVM()
        {
            selectServiceCommand = new Lazy<ICommand>(() => new RelayCommand(() =>
            {
                if (OnServiceSelected != null)
                {
                    OnServiceSelected(this, null);
                }
            }));
        }
    }
}
using Junte.UI.WPF;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Queue.Terminal.ViewModels
{
    public class ServiceButtonViewModel : ObservableObject
    {
        private string name;
        private string code;
        private float fontSize;
        private SolidColorBrush serviceBrush;

        private Lazy<ICommand> selectServiceCommand;

        public ICommand SelectServiceCommand { get { return this.selectServiceCommand.Value; } }

        public event EventHandler OnServiceSelected = delegate { };

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public float FontSize
        {
            get { return fontSize; }
            set { SetProperty(ref fontSize, value); }
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

        public ServiceButtonViewModel()
        {
            selectServiceCommand = new Lazy<ICommand>(() => new RelayCommand(() => OnServiceSelected(this, null)));
        }
    }
}
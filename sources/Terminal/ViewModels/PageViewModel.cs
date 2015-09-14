using Junte.UI.WPF;
using Microsoft.Practices.Unity;
using Queue.Terminal.Core;

namespace Queue.Terminal.ViewModels
{
    public abstract class PageViewModel : ObservableObject
    {
        private ClientRequestModel model;

        [Dependency]
        public ClientRequestModel Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }
}
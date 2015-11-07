using Microsoft.Practices.Unity;
using Queue.Terminal.Core;
using Queue.UI.WPF;

namespace Queue.Terminal.ViewModels
{
    public abstract class PageViewModel : RichViewModel
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
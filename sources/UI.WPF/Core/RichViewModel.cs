using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Queue.UI.WPF
{
    public abstract class RichViewModel : ObservableObject
    {
        public RichViewModel()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(GetType(), this);
        }
    }
}
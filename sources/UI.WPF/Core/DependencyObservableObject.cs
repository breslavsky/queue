using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Queue.UI.WPF.Core
{
    public abstract class DependencyObservableObject : ObservableObject
    {
        public DependencyObservableObject()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(GetType(), this);
        }
    }
}
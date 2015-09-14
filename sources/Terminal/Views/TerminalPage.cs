using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Controls;

namespace Queue.Terminal.Views
{
    public abstract class TerminalPage : Page
    {
        protected abstract Type ModelType { get; }

        public TerminalPage()
        {
            DataContext = ServiceLocator.Current.GetInstance<IUnityContainer>()
                                                .Resolve(ModelType);
        }
    }
}
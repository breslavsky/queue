using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Terminal.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Terminal.UserControls
{
    public partial class SearchServiceResults : UserControl
    {
        private readonly SearchServiceResultsViewModel model;

        public SearchServiceResults()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                model = ServiceLocator.Current.GetInstance<IUnityContainer>().Resolve<SearchServiceResultsViewModel>();
                model.Initialize(servicesGrid);
                DataContext = model;
            }
        }

        public SearchServiceResultsViewModel GetModel()
        {
            return model;
        }
    }
}
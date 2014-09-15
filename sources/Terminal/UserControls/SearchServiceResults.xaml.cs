using Queue.Terminal.Models;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Terminal.UserControls
{
    public partial class SearchServiceResults : UserControl
    {
        private readonly SearchServiceResultsVM model;

        public SearchServiceResults()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                model = new SearchServiceResultsVM();
                model.Initialize(servicesGrid);
                DataContext = model;
            }
        }

        public SearchServiceResultsVM GetModel()
        {
            return model;
        }
    }
}
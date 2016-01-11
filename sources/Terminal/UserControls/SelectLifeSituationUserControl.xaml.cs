using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Queue.Terminal.UserControls
{
    public partial class SelectLifeSituationUserControl : UserControl
    {
        public event EventHandler<Service> ServiceSelected
        {
            add { model.OnServiceSelected += value; }
            remove { model.OnServiceSelected -= value; }
        }

        private readonly SelectLifeSituationViewModel model;

        private ObjectsPager pager;

        public SelectLifeSituationUserControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                model = new SelectLifeSituationViewModel();
                model.OnRenderServices += RenderServices;

                pager = new ObjectsPager(servicesGrid);

                prevButton.DataContext = pager;
                nextButton.DataContext = pager;
                DataContext = model;
            }
        }

        private void RenderServices(object sender, RenderServicesEventArgs args)
        {
            pager.UpdateServices(args.Services, args.Cols, args.Rows);
        }
    }
}
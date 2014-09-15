using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Queue.UI.WPF
{
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            InitializeComponent();           
        }

        public LoadingControl Show()
        {
            Visibility = Visibility.Visible;

            foreach (FrameworkElement child in ((Panel)Parent).Children)
            {
                if (!child.Equals(this))
                {
                    child.Blur();
                }
            }

            return this;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;

            foreach (FrameworkElement child in ((Panel)Parent).Children)
            {
                if (!child.Equals(this))
                {
                    child.UnBlur();
                }
            }
        }
    }
}

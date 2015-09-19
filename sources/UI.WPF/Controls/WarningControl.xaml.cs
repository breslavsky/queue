using Microsoft.Practices.ServiceLocation;
using Queue.UI.WPF.Types;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class WarningControl : UserControl
    {
        private Action closed;

        public WarningControl(string message, Action closed)
        {
            InitializeComponent();

            warningTextBlock.Text = message;
            this.closed = closed;
        }

        public void Hide()
        {
            if (closed != null)
            {
                closed();
            }

            ServiceLocator.Current.GetInstance<IMainWindow>().HideMessageBox(this);
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
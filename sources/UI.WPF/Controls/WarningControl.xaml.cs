using Microsoft.Practices.ServiceLocation;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class WarningControl : UserControl
    {
        private Action closed;
        public string Text { get; set; }

        public bool Closeable { get; set; }

        public WarningControl(string message, Action closed, bool closeable)
        {
            InitializeComponent();

            this.closed = closed;

            Text = message;
            Closeable = closeable;
            DataContext = this;
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
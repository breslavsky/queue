﻿using Queue.UI.WPF.ViewModels;
using System.Windows.Controls;

namespace Queue.UI.WPF.Controls
{
    public partial class ConnectionStatusControl : UserControl
    {
        public ConnectionStatusControl()
        {
            InitializeComponent();

            DataContext = new ConnectionStatusControlViewModel();
        }
    }
}
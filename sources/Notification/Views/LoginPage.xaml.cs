﻿using Queue.Notification.ViewModels;
using Queue.UI.WPF;

namespace Queue.Notification.Views
{
    public partial class LoginPage : RichPage
    {
        public LoginPageViewModel Model { get; private set; }

        public LoginPage()
            : base()
        {
            InitializeComponent();

            Model = new LoginPageViewModel();

            DataContext = Model;
        }
    }
}
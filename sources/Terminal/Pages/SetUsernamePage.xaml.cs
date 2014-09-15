using Queue.Terminal.Models.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.Pages
{
    public partial class SetUsernamePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SetUsernamePageVM); } }

        private SetUsernamePageVM viewModel;

        public SetUsernamePage() :
            base()
        {
            InitializeComponent();

            viewModel = DataContext as SetUsernamePageVM;

            usernameTextBox.TextChanged += usernameTextBox_TextChanged;
        }

        private void TerminalPage_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, usernameTextBox);

            if (viewModel.Model.CurrentClient != null)
            {
                usernameTextBox.Text += viewModel.Model.CurrentClient.ToString();
            }
        }

        private void virtualKeyboard_OnBackspace(object sender, UI.WPF.VirtualKeyboardEvent e)
        {
            if (usernameTextBox.Text.Length > 0)
            {
                usernameTextBox.Text = usernameTextBox.Text.Remove(usernameTextBox.Text.Length - 1);
            }
        }

        private void virtualKeyboard_OnTyping(object sender, UI.WPF.VirtualKeyboardEvent e)
        {
            usernameTextBox.Text += e.Letter;
        }

        private void usernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Username = usernameTextBox.Text;
            usernameTextBox.CaretIndex = viewModel.Username.Length;
        }
    }
}
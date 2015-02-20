using Queue.Terminal.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.Views
{
    public partial class SetClientPage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SetClientPageVM); } }

        private SetClientPageVM viewModel;

        public SetClientPage() :
            base()
        {
            InitializeComponent();

            viewModel = DataContext as SetClientPageVM;

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
using System.Windows;

namespace UI.WPF.Tests
{
    public partial class KeyboardWindow : Window
    {
        public KeyboardWindow()
        {
            InitializeComponent();
        }

        private void KeyboardControl_OnLetter(object sender, string e)
        {
        }

        private void OnLetter(object sender, string e)
        {
            int y = 9;
        }

        private void OnBackspace(object sender, System.EventArgs e)
        {
            int y = 9;
        }
    }
}
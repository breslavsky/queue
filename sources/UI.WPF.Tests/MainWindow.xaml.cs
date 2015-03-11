using System.Windows;

namespace UI.WPF.Tests
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new KeyboardWindow().ShowDialog();
        }
    }
}
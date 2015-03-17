using Queue.UI.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public partial class KeyboardControl : UserControl
    {
        public static DependencyProperty OnLetterProperty = DependencyProperty.Register("OnLetter", typeof(string), typeof(KeyboardControl));
        public static DependencyProperty OnBackspaceProperty = DependencyProperty.Register("OnBackspace", typeof(string), typeof(KeyboardControl));

        public event EventHandler<string> OnLetter
        {
            add { (DataContext as KeyboardControlViewModel).OnLetter += value; }
            remove { (DataContext as KeyboardControlViewModel).OnLetter -= value; }
        }

        public event EventHandler OnBackspace
        {
            add { (DataContext as KeyboardControlViewModel).OnBackspace += value; }
            remove { (DataContext as KeyboardControlViewModel).OnBackspace -= value; }
        }

        public KeyboardControl()
        {
            InitializeComponent();

            DataContext = new KeyboardControlViewModel();
        }
    }
}
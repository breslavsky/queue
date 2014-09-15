﻿using Queue.Display.Models;
using Queue.UI.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Display
{
    public partial class HomePage : RichPage
    {
        private const int MinFontSize = 5;

        public HomePage()
            : base()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.None;

            (DataContext as HomePageVM).Initialize();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as HomePageVM).Dispose();
        }

        private void OnCommentTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            FrameworkElement parent = (FrameworkElement)tb.Parent;

            while (tb.ActualHeight > parent.ActualHeight & tb.FontSize > MinFontSize)
            {
                tb.FontSize -= 1;
                tb.UpdateLayout();
            }
        }
    }
}
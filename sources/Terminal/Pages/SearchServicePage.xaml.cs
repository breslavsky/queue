using Queue.Terminal.Models.Pages;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.Pages
{
    public partial class SearchServicePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SearchServicePageVM); } }

        private SearchServicePageVM viewModel;

        public SearchServicePage()
        {
            InitializeComponent();

            viewModel = DataContext as SearchServicePageVM;
            viewModel.OnSearch += viewModel_OnSearch;
            viewModel.Initialize(searchResults.GetModel());

            filterTextBox.TextChanged += searchPhraseTextBox_TextChanged;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, filterTextBox);
            ShowKeyboard();
        }

        private void searchPhraseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Filter = filterTextBox.Text;
            filterTextBox.CaretIndex = viewModel.Filter.Length;
        }

        private void virtualKeyboard_OnBackspace(object sender, VirtualKeyboardEvent e)
        {
            if (filterTextBox.Text.Length > 0)
            {
                filterTextBox.Text = filterTextBox.Text.Remove(filterTextBox.Text.Length - 1);
            }
        }

        private void virtualKeyboard_OnTyping(object sender, VirtualKeyboardEvent e)
        {
            filterTextBox.Text += e.Letter;
        }

        private void viewModel_OnSearch(object sender, EventArgs e)
        {
            ShowResults();
        }

        private void filterTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowKeyboard();
        }

        private void ShowKeyboard()
        {
            ResultsVisibility(false);
            KeyboardVisibility(true);
        }

        private void ShowResults()
        {
            ResultsVisibility(true);
            KeyboardVisibility(false);
        }

        private void KeyboardVisibility(bool value)
        {
            Visibility v = value ? Visibility.Visible : Visibility.Collapsed;
            if (v != keyboard.Visibility)
            {
                keyboard.Visibility = v;
            }
        }

        private void ResultsVisibility(bool value)
        {
            Visibility v = value ? Visibility.Visible : Visibility.Collapsed;
            if (v != searchResults.Visibility)
            {
                searchResults.Visibility = v;
            }
        }
    }
}
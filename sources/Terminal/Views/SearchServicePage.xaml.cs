using Queue.Terminal.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.Views
{
    public partial class SearchServicePage : TerminalPage
    {
        protected override Type ModelType { get { return typeof(SearchServicePageViewModel); } }

        private SearchServicePageViewModel viewModel;

        public SearchServicePage()
        {
            InitializeComponent();

            viewModel = DataContext as SearchServicePageViewModel;
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

        private void keyboard_OnBackspace(object sender, EventArgs e)
        {
            if (filterTextBox.Text.Length > 0)
            {
                filterTextBox.Text = filterTextBox.Text.Remove(filterTextBox.Text.Length - 1);
            }
        }

        private void keyboard_OnLetter(object sender, string letter)
        {
            filterTextBox.Text += letter;
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
            var v = value ? Visibility.Visible : Visibility.Collapsed;
            if (v != keyboard.Visibility)
            {
                keyboard.Visibility = v;
            }
        }

        private void ResultsVisibility(bool value)
        {
            var v = value ? Visibility.Visible : Visibility.Collapsed;
            if (v != searchResults.Visibility)
            {
                searchResults.Visibility = v;
            }
        }
    }
}
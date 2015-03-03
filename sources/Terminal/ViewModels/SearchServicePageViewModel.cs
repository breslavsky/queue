using Junte.UI.WPF;
using System;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SearchServicePageViewModel : PageViewModel
    {
        private string filter;

        private IServiceSearch searcher;

        public string Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        public ICommand SearchCommand { get; set; }

        public event EventHandler OnSearch = delegate { };

        public SearchServicePageViewModel()
        {
            SearchCommand = new RelayCommand(Search);
        }

        public void Initialize(IServiceSearch searcher)
        {
            this.searcher = searcher;
        }

        private void Search()
        {
            searcher.Search(Filter);
            OnSearch(this, null);
        }
    }
}
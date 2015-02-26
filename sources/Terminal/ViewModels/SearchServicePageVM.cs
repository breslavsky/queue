using Junte.UI.WPF;
using System;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SearchServicePageVM : PageVM
    {
        private string filter;

        private Lazy<ICommand> searchCommand;
        private IServiceSearch searcher;

        public string Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        public ICommand SearchCommand { get { return searchCommand.Value; } }

        public event EventHandler OnSearch = delegate { };

        public SearchServicePageVM()
        {
            searchCommand = new Lazy<ICommand>(() => new RelayCommand(Search));
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
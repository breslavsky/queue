using Queue.Terminal.Types;
using Queue.UI.WPF.Types;
using System;
using System.Windows.Input;

namespace Queue.Terminal.Models.Pages
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

        public event EventHandler OnSearch;

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

            if (OnSearch != null)
            {
                OnSearch(this, null);
            }
        }
    }
}
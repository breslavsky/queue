using Junte.UI.WPF;
using Queue.Terminal.UserControls;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Terminal.Core
{
    public class ObjectsPager : ObservableObject
    {
        private Grid grid;
        private SelectServiceButton[] services;

        private bool hasNext;
        private bool hasPrev;

        private int cols;
        private int rows;
        private int servicesPerPage;

        private int currentPage;

        public bool HasNext
        {
            get { return hasNext; }
            set { SetProperty(ref hasNext, value); }
        }

        public bool HasPrev
        {
            get { return hasPrev; }
            set { SetProperty(ref hasPrev, value); }
        }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ObjectsPager(Grid grid)
        {
            this.grid = grid;

            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
        }

        public void UpdateServices(SelectServiceButton[] services, int cols, int rows)
        {
            this.services = services;
            this.cols = cols;
            this.rows = rows;
            this.servicesPerPage = cols * rows;

            Update();
        }

        private void Update()
        {
            currentPage = 0;
            ShowPage(currentPage);
        }

        private void Next()
        {
            ShowPage(++currentPage);
        }

        private void Prev()
        {
            ShowPage(--currentPage);
        }

        private void ShowPage(int pageNo)
        {
            HasPrev = pageNo > 0;

            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition());

            grid.ColumnDefinitions.Clear();

            int row = 0;
            int col = 0;

            foreach (var button in services.Skip(pageNo * servicesPerPage).Take(servicesPerPage))
            {
                if (col >= cols)
                {
                    col = 0;
                    grid.RowDefinitions.Add(new RowDefinition());
                    row++;
                }
                else
                {
                    if (col + 1 > grid.ColumnDefinitions.Count)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }

                button.SetValue(Grid.ColumnProperty, col++);
                button.SetValue(Grid.RowProperty, row);
                grid.Children.Add(button);
            }

            HasNext = services.Skip((pageNo + 1) * servicesPerPage).Take(servicesPerPage).Count() > 0;
        }
    }
}
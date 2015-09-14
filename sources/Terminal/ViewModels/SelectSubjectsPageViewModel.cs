using Junte.UI.WPF;
using Microsoft.Practices.Unity;
using Queue.Terminal.Core;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectSubjectsPageViewModel : PageViewModel
    {
        private bool canInc;
        private bool canDec;

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ICommand DecCommand { get; set; }

        public ICommand IncCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        public bool CanInc
        {
            get { return canInc; }
            set { SetProperty(ref canInc, value); }
        }

        public bool CanDec
        {
            get { return canDec; }
            set { SetProperty(ref canDec, value); }
        }

        public SelectSubjectsPageViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
            DecCommand = new RelayCommand(DecSubjectsCount);
            IncCommand = new RelayCommand(IncSubjectsCount);
        }

        private void Loaded()
        {
            if (Model.Subjects == null)
            {
                Model.Subjects = 1;
            }

            UpdateIncDecEnable();
        }

        private void IncSubjectsCount()
        {
            if (Model.Subjects < Model.MaxSubjects)
            {
                Model.Subjects++;
            }
            UpdateIncDecEnable();
        }

        private void DecSubjectsCount()
        {
            if (Model.Subjects > 1)
            {
                Model.Subjects--;
            }
            UpdateIncDecEnable();
        }

        private void UpdateIncDecEnable()
        {
            CanInc = Model.Subjects < Model.MaxSubjects;
            CanDec = Model.Subjects > 1;
        }

        private void Next()
        {
            Navigator.NextPage();
        }

        private void Prev()
        {
            Model.Subjects = null;
            Navigator.PrevPage();
        }
    }
}
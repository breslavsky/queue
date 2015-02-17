using Junte.UI.WPF;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectSubjectsCountPageVM : PageVM
    {
        private bool canInc;
        private bool canDec;

        public SelectSubjectsCountPageVM()
        {
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
            DecCommand = new RelayCommand(DecSubjectsCount);
            IncCommand = new RelayCommand(IncSubjectsCount);
        }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ICommand DecCommand { get; set; }

        public ICommand IncCommand { get; set; }

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

        public void Initialize()
        {
            if (Model.SubjectsCount == null)
            {
                Model.SubjectsCount = 1;
            }

            UpdateIncDecEnable();
        }

        private void IncSubjectsCount()
        {
            if (Model.SubjectsCount < Model.MaxSubjects)
            {
                Model.SubjectsCount++;
            }
            UpdateIncDecEnable();
        }

        private void DecSubjectsCount()
        {
            if (Model.SubjectsCount > 1)
            {
                Model.SubjectsCount--;
            }
            UpdateIncDecEnable();
        }

        private void UpdateIncDecEnable()
        {
            CanInc = Model.SubjectsCount < Model.MaxSubjects;
            CanDec = Model.SubjectsCount > 1;
        }

        private void Next()
        {
            navigator.NextPage();
        }

        private void Prev()
        {
            Model.SubjectsCount = null;
            navigator.PrevPage();
        }
    }
}
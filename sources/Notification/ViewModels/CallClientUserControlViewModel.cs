using Junte.UI.WPF;
using Queue.Services.DTO;

namespace Queue.Notification.ViewModels
{
    public class CallClientUserControlViewModel : ObservableObject
    {
        private bool active;
        private int number;
        private string workplaceTitle;

        public bool Active
        {
            get { return active; }
            set { SetProperty(ref active, value); }
        }

        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }

        public string WorkplaceTitle
        {
            get { return workplaceTitle; }
            set { SetProperty(ref workplaceTitle, value); }
        }

        public void ShowMessage(ClientRequest request)
        {
            Number = request.Number;
            WorkplaceTitle = request.Operator.Workplace.ToString();

            Active = true;
        }

        public void CloseMessage()
        {
            Active = false;
        }
    }
}
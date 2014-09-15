using Queue.Services.DTO;
using Queue.UI.WPF.Types;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Notification.Models
{
    public class CallClientUserControlVM : ObservableObject
    {
        private bool active;
        private int number;
        private string workplaceType;
        private int workplaceNumber;

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

        public string WorkplaceType
        {
            get { return workplaceType; }
            set { SetProperty(ref workplaceType, value); }
        }

        public int WorkplaceNumber
        {
            get { return workplaceNumber; }
            set { SetProperty(ref workplaceNumber, value); }
        }

        public void ShowMessage(ClientRequest request)
        {
            Number = request.Number;
            Workplace workplace = request.Operator.Workplace;
            WorkplaceType = Translation.WorkplaceType.ResourceManager.GetString(workplace.Type.ToString());
            WorkplaceNumber = workplace.Number;

            Active = true;
        }

        public void CloseMessage()
        {
            Active = false;
        }
    }
}
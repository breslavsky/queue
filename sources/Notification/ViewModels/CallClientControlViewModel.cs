using Junte.UI.WPF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Sounds;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Queue.Notification.ViewModels
{
    public class CallClientControlViewModel : ObservableObject
    {
        private object callLock = new object();

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

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public ClientRequestsListener ClientRequestsListener { get; set; }

        [Dependency]
        public HubDisplayService HubQualityService { get; set; }

        public CallClientControlViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            ClientRequestsListener.CallClient += OnCallClient;
        }

        private void OnCallClient(object sender, ClientRequest request)
        {
            Task.Run(() =>
            {
                lock (callLock)
                {
                    ShowMessage(request);
                    PlayVoice(request);
                    CloseMessage();
                }
            });
        }

        private void PlayVoice(ClientRequest request)
        {
            using (var soundPlayer = new SoundPlayer())
            {
                soundPlayer.PlayStream(Tones.Notify);
                soundPlayer.PlayStream(Words.Number);

                soundPlayer.PlayNumber(request.Number);

                var workplace = request.Operator.Workplace;
                soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Type.ToString()));
                soundPlayer.PlayNumber(workplace.Number);

                if (workplace.Modificator != WorkplaceModificator.None)
                {
                    soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Modificator.ToString()));
                }
            }
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

        private void Unloaded()
        {
            ClientRequestsListener.CallClient -= OnCallClient;
        }
    }
}
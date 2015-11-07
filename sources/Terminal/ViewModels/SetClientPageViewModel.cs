using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.WPF;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SetClientPageViewModel : PageViewModel
    {
        private string username;

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IServerUserTcpService> UserChannelManager { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        public ICommand UnloadedCommand { get; set; }

        public SetClientPageViewModel()
            : base()
        {
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Next()
        {
            if (String.IsNullOrWhiteSpace(Username))
            {
                Window.Warning(Translater.Message("NoNameWarn"));
                return;
            }

            if (Username == TerminalConfig.PIN.ToString())
            {
                Application.Current.Shutdown();
            }

            Window.ExecuteLongTask(DoSetClient);
        }

        private async Task DoSetClient()
        {
            using (var channel = ChannelManager.CreateChannel())
            using (var userChannel = UserChannelManager.CreateChannel())
            {
                await userChannel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

                var words = Username.Split(' ');
                var surname = words[0];

                var name = String.Empty;
                if (words.Length > 1)
                {
                    name = words[1];
                }

                var patronymic = String.Empty;
                if (words.Length > 2)
                {
                    patronymic = words[2];
                }

                Model.CurrentClient = await channel.Service.EditClient(new Client()
                {
                    Surname = surname,
                    Name = name,
                    Patronymic = patronymic
                });

                Navigator.NextPage();
            }
        }

        private void Prev()
        {
            Model.CurrentClient = null;
            Navigator.PrevPage();
        }

        private void Unloaded()
        {
            try
            {
                ChannelManager.Dispose();
                UserChannelManager.Dispose();
            }
            catch { }
        }
    }
}
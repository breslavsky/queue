using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using System;
using System.ServiceModel;
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
        public TerminalWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public TerminalConfig TerminalConfig { get; set; }

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        public SetClientPageViewModel()
        {
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
        }

        private async void Next()
        {
            if (String.IsNullOrWhiteSpace(Username))
            {
                Window.ShowWarning(Translater.Message("NoNameWarn"));
                return;
            }

            if (Username == TerminalConfig.PIN.ToString())
            {
                Application.Current.Shutdown();
            }

            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    await channel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

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

                    Model.CurrentClient = await TaskPool.AddTask(channel.Service.EditClient(new Client()
                    {
                        Surname = surname,
                        Name = name,
                        Patronymic = patronymic
                    }));

                    Navigator.NextPage();
                }
                catch (FaultException exception)
                {
                    Window.ShowWarning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }
        }

        private void Prev()
        {
            Model.CurrentClient = null;
            Navigator.PrevPage();
        }
    }
}
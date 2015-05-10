using Junte.Translation;
using Junte.UI.WPF;
using Queue.Services.DTO;
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
            if (string.IsNullOrWhiteSpace(Username))
            {
                screen.ShowWarning(Translater.Message("NoNameWarn"));
                return;
            }

            if (Username == terminalConfig.PIN.ToString())
            {
                Application.Current.Shutdown();
            }

            using (var channel = channelManager.CreateChannel())
            {
                var loading = screen.ShowLoading();

                try
                {
                    await channel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

                    string[] words = Username.Split(' ');

                    string surname = words[0];

                    string name = string.Empty;
                    if (words.Length > 1)
                    {
                        name = words[1];
                    }

                    string patronymic = string.Empty;
                    if (words.Length > 2)
                    {
                        patronymic = words[2];
                    }

                    Model.CurrentClient = await taskPool.AddTask(channel.Service.EditClient(new Client()
                    {
                        Surname = surname,
                        Name = name,
                        Patronymic = patronymic
                    }));

                    navigator.NextPage();
                }
                catch (FaultException exception)
                {
                    screen.ShowWarning(exception.Reason.ToString());
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
            navigator.PrevPage();
        }
    }
}
using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using MahApps.Metro;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Display.Models;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.Display.ViewModels
{
    public class LoginPageViewModel : RichViewModel, IDisposable
    {
        private bool disposed;
        private AccentColorComboBoxItem selectedAccent;
        private bool isConnected;

        private string endpoint;
        private bool isRemember;
        private RichPage owner;
        private IdentifiedEntityLink[] workplaces;
        private Guid selectedWorkplace;

        private WorkplaceService workplaceService;
        private ChannelManager<IWorkplaceTcpService> channelManager;

        public event EventHandler OnLogined = delegate { };

        private Language selectedLanguage;

        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
        }

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value); }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty(ref isConnected, value); }
        }

        public IdentifiedEntityLink[] Workplaces
        {
            get { return workplaces; }
            set { SetProperty(ref workplaces, value); }
        }

        public Guid SelectedWorkplace
        {
            get { return selectedWorkplace; }
            set { SetProperty(ref selectedWorkplace, value); }
        }

        public AccentColorComboBoxItem[] AccentColors { get; set; }

        public AccentColorComboBoxItem SelectedAccent
        {
            get { return selectedAccent; }
            set
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(value.Name);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

                SetProperty(ref selectedAccent, value);
            }
        }

        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                SetProperty(ref selectedLanguage, value);

                var culture = selectedLanguage.GetCulture();

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = culture;
                selectedLanguage.SetCurrent();
            }
        }

        public Workplace Workplace { get; private set; }

        public ICommand ConnectCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public AppSettings AppSettings { get; set; }

        public LoginPageViewModel(RichPage owner)
            : base()
        {
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember || (SelectedWorkplace != Guid.Empty))
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            Endpoint = AppSettings.Endpoint;
            SelectedWorkplace = AppSettings.WorkplaceId;
            SelectedLanguage = AppSettings.Language;
            IsRemember = AppSettings.IsRemember;

            if (!String.IsNullOrWhiteSpace(AppSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == AppSettings.Accent);
            }
        }

        private async void Connect()
        {
            if (workplaceService != null)
            {
                workplaceService.Dispose();
            }

            workplaceService = new WorkplaceService(Endpoint);

            if (channelManager != null)
            {
                channelManager.Dispose();
            }

            channelManager = workplaceService.CreateChannelManager();

            IsConnected = false;

            Workplaces = await Window.ExecuteLongTask(async () =>
             {
                 using (var channel = channelManager.CreateChannel())
                 {
                     return await channel.Service.GetWorkplacesLinks();
                 }
             });

            if (Workplaces == null)
            {
                return;
            }

            SelectedWorkplace = AppSettings.WorkplaceId != Guid.Empty ? AppSettings.WorkplaceId : Workplaces.First().Id;
            IsConnected = true;

            if (isConnected && IsRemember)
            {
                Login();
            }
        }

        private async void Login()
        {
            if (SelectedWorkplace == Guid.Empty)
            {
                return;
            }

            Workplace = await Window.ExecuteLongTask(async () =>
                 {
                     using (var channel = channelManager.CreateChannel())
                     {
                         return await channel.Service.GetWorkplace(SelectedWorkplace);
                     }
                 });

            if (Workplace == null)
            {
                return;
            }

            SaveSettings();

            OnLogined(this, null);
        }

        private void SaveSettings()
        {
            AppSettings.Endpoint = Endpoint;
            AppSettings.WorkplaceId = SelectedWorkplace;
            AppSettings.IsRemember = IsRemember;
            AppSettings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            AppSettings.Language = SelectedLanguage;

            ConfigurationManager.Save();
        }

        private void Unloaded()
        {
            Dispose();
        }

        #region IDisposable

        ~LoginPageViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    if (channelManager != null)
                    {
                        channelManager.Dispose();
                    }
                }
                catch { }
            }

            disposed = true;
        }

        #endregion IDisposable
    }
}
using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.Common;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Queue.Terminal.ViewModels
{
    public class PrintCouponPageViewModel : PageViewModel, IDisposable
    {
        private bool disposed;
        private DispatcherTimer timer;
        private bool success;
        private WarningControl warn;

        public bool Success
        {
            get { return success; }
            set { SetProperty(ref success, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public TerminalWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        public PrintCouponPageViewModel()
            : base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
        }

        private async void Loaded()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    await channel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

                    var clientRequest = await AddClientRequest(channel);

                    Success = true;

                    await PrintCoupon(channel, clientRequest);
                }
                catch (FaultException exception)
                {
                    warn = Window.Warning(exception.Reason, () => Navigator.NextPage());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }

                timer.Start();
            }
        }

        private void Unloaded()
        {
            Dispose();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (warn != null)
            {
                warn.Hide(true);
            }

            Navigator.Reset();
        }

        private async Task<ClientRequest> AddClientRequest(Channel<IServerTcpService> channel)
        {
            var clientId = Model.CurrentClient == null ?
                                        Guid.Empty :
                                        Model.CurrentClient.Id;

            switch (Model.RequestType)
            {
                case ClientRequestType.Live:

                    return await TaskPool.AddTask(channel.Service.AddLiveClientRequest(clientId,
                                         Model.SelectedService.Id,
                                         false,
                                         new Dictionary<Guid, object>(),
                                         (int)(Model.Subjects ?? 1)));

                case ClientRequestType.Early:

                    return await TaskPool.AddTask(channel.Service.AddEarlyClientRequest(clientId,
                                        Model.SelectedService.Id,
                                        Model.SelectedDate.Value,
                                        Model.SelectedTime.Value,
                                        new Dictionary<Guid, object>(),
                                        (int)(Model.Subjects ?? 1)));
            }

            return null;
        }

        private async Task PrintCoupon(Channel<IServerTcpService> channel, ClientRequest clientRequest)
        {
            var data = await TaskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
            var template = ServiceLocator.Current.GetInstance<CouponConfig>().Template;
            XPSUtils.PrintXaml(template, data);
        }

        #region IDisposable

        ~PrintCouponPageViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    timer.Stop();
                    timer.Tick -= timer_Tick;
                    timer = null;
                }
                catch { }
            }

            disposed = true;
        }

        #endregion IDisposable
    }
}
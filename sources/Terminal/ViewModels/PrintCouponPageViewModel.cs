using Junte.UI.WPF;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.Common;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Printing;
using System.ServiceModel;
using System.Threading.Tasks;
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

        public PrintCouponPageViewModel()
            : base()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
        }

        internal async void Initialize()
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    await channel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

                    ClientRequest clientRequest = await AddClientRequest(channel);

                    Success = true;

                    await PrintCoupon(channel, clientRequest);
                }
                catch (FaultException exception)
                {
                    warn = screen.ShowWarning(exception.Reason, () => navigator.Reset());
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

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (warn != null)
            {
                warn.Hide(true);
            }

            navigator.Reset();
        }

        private async Task<ClientRequest> AddClientRequest(Channel<IServerTcpService> channel)
        {
            Guid clientId = Model.CurrentClient == null ?
                                        Guid.Empty :
                                        Model.CurrentClient.Id;

            switch (Model.RequestType)
            {
                case ClientRequestType.Live:

                    return await taskPool.AddTask(channel.Service.AddLiveClientRequest(clientId,
                                         Model.SelectedService.Id,
                                         false,
                                         new Dictionary<Guid, object>(),
                                         (int)(Model.Subjects ?? 1)));

                case ClientRequestType.Early:

                    return await taskPool.AddTask(channel.Service.AddEarlyClientRequest(clientId,
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
            ClientRequestCoupon data = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
            string template = ServiceLocator.Current.GetInstance<CouponConfig>().Template;
            string xpsFile = XPSGenerator.FromXaml(template, data);
            try
            {
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                defaultPrintQueue.AddJob(clientRequest.ToString(), xpsFile, false);
            }
            catch { }
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
                    timer = null;
                }
                catch { }
            }

            disposed = true;
        }

        #endregion IDisposable
    }
}
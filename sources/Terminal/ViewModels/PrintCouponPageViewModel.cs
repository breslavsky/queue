using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.UI.Common;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Queue.Terminal.ViewModels
{
    public class PrintCouponPageViewModel : PageViewModel, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed;
        private DispatcherTimer timer;
        private bool success;

        public bool Success
        {
            get { return success; }
            set { SetProperty(ref success, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ICommonTemplateManager CommonTemplateManager { get; set; }

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
            var couponData = await Window.ExecuteLongTask(async () =>
             {
                 using (var channel = ChannelManager.CreateChannel())
                 {
                     var clientRequest = await AddClientRequest(channel);

                     Success = true;

                     logger.Debug("печать талона [client: {0}; service: {1}]", clientRequest.Client, clientRequest.Service);
                     return await channel.Service.GetClientRequestCoupon(clientRequest.Id);
                 }
             });

            if (couponData != null)
            {
                PrintCoupon(couponData);
            }

            timer.Start();
        }

        private void PrintCoupon(ClientRequestCoupon data)
        {
            try
            {
                XPSUtils.PrintXaml(CommonTemplateManager.GetTemplate(Templates.Coupon), data);
            }
            catch (Exception e)
            {
                Window.Warning(e.Message);
            }
        }

        private void Unloaded()
        {
            Dispose();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
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

                    return await channel.Service.AddLiveClientRequest(clientId,
                                         Model.SelectedService.Id,
                                         false,
                                         new Dictionary<Guid, object>(),
                                         (int)(Model.Subjects ?? 1));

                case ClientRequestType.Early:

                    return await channel.Service.AddEarlyClientRequest(clientId,
                                        Model.SelectedService.Id,
                                        Model.SelectedDate.Value,
                                        Model.SelectedTime.Value,
                                        new Dictionary<Guid, object>(),
                                        (int)(Model.Subjects ?? 1));
            }

            return null;
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

                    ChannelManager.Dispose();
                }
                catch { }
            }

            disposed = true;
        }

        #endregion IDisposable
    }
}
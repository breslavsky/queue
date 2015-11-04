using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Services.Media;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Queue.Media
{
    public sealed class MediaInstance
    {
        #region dependency

        [Dependency]
        public IUnityContainer Container { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MediaSettings mediaSettings;
        private readonly IList<ServiceHost> hosts = new List<ServiceHost>();

        public MediaInstance(MediaSettings mediaSettings)
        {
            this.mediaSettings = mediaSettings;

            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this);

            Container.RegisterInstance(mediaSettings);

            CreateServices();
        }

        private void CreateServices()
        {
            {
                var host = new MediaServiceHost();

                var uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.Http, mediaSettings.Port));
                var endpoint = host.AddServiceEndpoint(typeof(IMediaService), Bindings.WebHttpBinding, uri.ToString());
                endpoint.Behaviors.Add(new WebHttpBehavior());

                hosts.Add(host);
            }
        }

        public void Start()
        {
            logger.Info("Starting");

            foreach (var h in hosts)
            {
                h.Open();
            }
        }

        public void Stop()
        {
            logger.Info("Stoping");

            foreach (var h in hosts)
            {
                h.Close();
            }
        }
    }
}
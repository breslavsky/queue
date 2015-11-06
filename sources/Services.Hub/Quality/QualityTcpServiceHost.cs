using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class QualityTcpServiceHost : ServiceHost
    {
        public QualityTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(QualityTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new QualityTcpServiceProvider());
            }
        }
    }
}
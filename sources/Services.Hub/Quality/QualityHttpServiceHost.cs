using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class QualityHttpServiceHost : ServiceHost
    {
        public QualityHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(QualityHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new QualityHttpServiceProvider());
            }
        }
    }
}
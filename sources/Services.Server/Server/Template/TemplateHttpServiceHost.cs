using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class TemplateHttpServiceHost : ServiceHost
    {
        public TemplateHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(TemplateHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new TemplateHttpServiceProvider());
            }
        }
    }
}
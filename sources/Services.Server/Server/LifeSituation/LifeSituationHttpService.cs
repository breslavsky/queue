using Queue.Services.Contracts.Server;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class LifeSituationHttpService : LifeSituationService, ILifeSituationHttpService
    {
    }
}
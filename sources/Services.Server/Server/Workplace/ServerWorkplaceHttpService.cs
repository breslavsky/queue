using Queue.Services.Contracts;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class ServerWorkplaceHttpService : ServerWorkplaceService, IServerWorkplaceHttpService
    {
    }
}
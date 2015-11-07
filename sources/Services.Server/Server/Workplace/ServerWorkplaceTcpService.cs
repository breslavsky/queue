using Queue.Services.Contracts;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class ServerWorkplaceTcpService : ServerWorkplaceService, IServerWorkplaceTcpService
    {
    }
}
using Queue.Services.Contracts.Server;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class WorkplaceTcpService : WorkplaceService, IWorkplaceTcpService
    {
    }
}
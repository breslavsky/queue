using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Server
{
    internal class ServerUserTcpServiceProvider : StandardServiceProvider<ServerUserTcpService>
    {
    }
}
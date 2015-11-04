using Junte.WCF;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Media
{
    internal class MediaServiceProvider : StandardServiceProvider<MediaService>
    {
    }
}
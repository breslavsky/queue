using Junte.WCF;
using NLog;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Queue.Services.Media
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true,
        UseSynchronizationContext = false)]
    public partial class MediaService : IMediaService
    {
        private const int BufferLength = 1024 * 1024;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MediaService(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser, string folder)
        {
            ChannelBuilder = channelBuilder;
            CurrentAdministrator = currentUser;
            Folder = folder;
        }

        protected DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        protected Administrator CurrentAdministrator { get; private set; }

        protected string Folder { get; private set; }

        public virtual string Index()
        {
            return "Служба успешно запущена";
        }

        public void UploadMediaConfigFile(string mediaConfigFileId, Stream data)
        {
            var file = string.Format("{0}/{1}", Folder, mediaConfigFileId);

            using (var fileStream = File.Open(file, FileMode.Create))
            {
                byte[] buffer = new byte[BufferLength];

                int readed = 0;
                do
                {
                    readed = data.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, readed);
                } while (readed > 0);
            }
        }

        public Stream LoadMediaConfigFile(string mediaConfigFileId)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "video/wmv";
            var file = string.Format("{0}/{1}", Folder, mediaConfigFileId);
            return File.OpenRead(file);
        }
    }
}
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts.Media;
using Queue.Services.Media.Settings;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Queue.Services.Media
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true,
        UseSynchronizationContext = false)]
    public partial class MediaService : DependencyService, IMediaService
    {
        #region dependency

        [Dependency]
        public MediaServiceSettings Settings { get; set; }

        #endregion dependency

        private const int BufferLength = 1024 * 1024;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MediaService()
            : base()
        {
        }

        public virtual string Index()
        {
            return "working";
        }

        public void UploadMediaConfigFile(string mediaConfigFileId, Stream data)
        {
            var file = string.Format("{0}/{1}", Settings.MediaFolder, mediaConfigFileId);

            using (var fileStream = File.Open(file, FileMode.Create))
            {
                var buffer = new byte[BufferLength];

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
            return File.OpenRead(Path.Combine(Settings.MediaFolder, mediaConfigFileId));
        }
    }
}
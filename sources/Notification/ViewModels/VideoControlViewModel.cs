using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Notification.UserControls;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF.Core;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.MediaListPlayer;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace Queue.Notification.ViewModels
{
    public class VideoControlViewModel : DependencyObservableObject
    {
        private const string MediaFileUriPattern = "{0}/media-config/files/{1}/load";

        private readonly VideoControl control;

        private bool disposed;
        private VlcControl vlcControl;

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public VideoControlViewModel(VideoControl control)
        {
            this.control = control;

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private async void Loaded()
        {
            try
            {
                vlcControl = CreateVLCControl();
                await ReadConfig();
            }
            catch (Exception ex)
            {
                UIHelper.Warning(null, String.Format("Ошибка инициализации видеоплеера: {0}", ex.Message));
            }
        }

        private async Task ReadConfig()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    ApplyConfig(await TaskPool.AddTask(channel.Service.GetMediaConfig()),
                                await TaskPool.AddTask(channel.Service.GetMediaConfigFiles()));
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    throw new QueueException(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    throw new QueueException(exception.Message);
                }
            }
        }

        private void ApplyConfig(MediaConfig config, MediaConfigFile[] mediaFiles)
        {
            foreach (var file in mediaFiles)
            {
                vlcControl.Medias.Add(new LocationMedia(String.Format(MediaFileUriPattern, config.ServiceUrl, file.Id)));
            }

            vlcControl.Stop();
            vlcControl.Play();
        }

        private VlcControl CreateVLCControl()
        {
            try
            {
                InitializeContextIfNeed();

                var vlcControl = new VlcControl()
                {
                    PlaybackMode = PlaybackModes.Loop
                };

                vlcControl.AudioProperties.IsMute = true;
                control.mainGrid.Children.Add(vlcControl);

                var vlcImage = new Image();
                vlcImage.SetBinding(Image.SourceProperty, new Binding("VideoSource")
                {
                    Source = vlcControl
                });
                control.mainGrid.Children.Add(vlcImage);
                return vlcControl;
            }
            catch
            {
                throw new QueueException("Не удалось создать компонент для отображения видео. Возможно не установлен VLC плеер");
            }
        }

        private void InitializeContextIfNeed()
        {
            if (VlcContext.IsInitialized)
            {
                return;
            }

            VlcContext.LibVlcPluginsPath = CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_AMD64;
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Standard;
            VlcContext.Initialize();
        }

        private void Unloaded()
        {
            Dispose();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    VlcContext.CloseAll();

                    if (TaskPool != null)
                    {
                        TaskPool.Cancel();
                        TaskPool.Dispose();
                        TaskPool = null;
                    }
                }
            }
            catch { }

            disposed = true;
        }

        ~VideoControlViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}
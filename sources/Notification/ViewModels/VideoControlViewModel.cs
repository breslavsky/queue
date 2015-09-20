﻿using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Notification.UserControls;
using Queue.Services.Contracts;
using Queue.Services.DTO;
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
    public class VideoControlViewModel
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

            ServiceLocator.Current.GetInstance<UnityContainer>().BuildUp(this);

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private async void Loaded()
        {
            vlcControl = CreateVLCControl();
            await ReadConfig();
        }

        private async Task ReadConfig()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    Update(await TaskPool.AddTask(channel.Service.GetMediaConfig()),
                            await TaskPool.AddTask(channel.Service.GetMediaConfigFiles()));
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
            }
        }

        private void Update(MediaConfig config, MediaConfigFile[] mediaFiles)
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
            VlcControl vlc = null;
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
                vlc = vlcControl;
            }
            catch (Exception ex)
            {
                UIHelper.Warning(null, String.Format("Ошибка при инициализации плеера VLC: {0}", ex.Message));
            }

            return vlc;
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
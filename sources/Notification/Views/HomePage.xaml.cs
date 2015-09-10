﻿using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Notification.ViewModels;
using Queue.Services.DTO;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.MediaListPlayer;
using Vlc.DotNet.Wpf;

namespace Queue.Notification.Views
{
    public partial class HomePage : RichPage
    {
        private HomePageViewModel model;

        public HomePage()
            : base()
        {
            InitializeComponent();

            ServiceLocator.Current.GetInstance<UnityContainer>().RegisterInstance<IMainWindow>(this);

            model = new HomePageViewModel();
            model.RequestUpdated += model_CurrentClientRequestPlanUpdated;
            model.RequestsLengthChanged += model_ClientRequestsLengthChanged;

            DataContext = model;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.None;

            tickerControl.Initialize(this);
            this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Mouse.OverrideCursor = Cursors.None;

            model.Initialize(CreateVLCControl());
        }

        private void model_ClientRequestsLengthChanged(object sender, int length)
        {
            CallingClientRequestsControl.Model.ClientRequestsLength = length;
        }

        private void model_CurrentClientRequestPlanUpdated(object sender, ClientRequest e)
        {
            CallingClientRequestsControl.Model.AddToClientRequests(e);
        }

        private VlcControl CreateVLCControl()
        {
            try
            {
                InitVlcContext();

                VlcControl vlcControl = new VlcControl()
                {
                    PlaybackMode = PlaybackModes.Loop
                };

                vlcControl.AudioProperties.IsMute = true;
                videoGrid.Children.Add(vlcControl);

                Image vlcImage = new Image();
                vlcImage.SetBinding(Image.SourceProperty, new Binding("VideoSource")
                {
                    Source = vlcControl
                });
                videoGrid.Children.Add(vlcImage);

                return vlcControl;
            }
            catch (Exception ex)
            {
                UIHelper.Warning(null, string.Format("Ошибка при инициализации плеера VLC: {0}", ex.Message));
            }

            return null;
        }

        private void InitVlcContext()
        {
            VlcContext.LibVlcPluginsPath = CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_AMD64;
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");

            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Standard;
            VlcContext.Initialize();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            VlcContext.CloseAll();

            model.Dispose();
        }
    }
}
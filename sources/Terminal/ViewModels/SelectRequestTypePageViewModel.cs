using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Terminal.Core;
using Queue.UI.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectRequestTypePageViewModel : PageViewModel
    {
        private string comment;
        private string liveComment;
        private string earlyComment;
        private bool allowLive;
        private bool allowEarly;
        private GridLength commentRowHeight;

        public ICommand SelectTypeCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        public GridLength CommentRowHeight
        {
            get { return commentRowHeight; }
            set { SetProperty(ref commentRowHeight, value); }
        }

        public string LiveComment
        {
            get { return liveComment; }
            set { SetProperty(ref liveComment, value); }
        }

        public string EarlyComment
        {
            get { return earlyComment; }
            set { SetProperty(ref earlyComment, value); }
        }

        public bool AllowLive
        {
            get { return allowLive; }
            set { SetProperty(ref allowLive, value); }
        }

        public bool AllowEarly
        {
            get { return allowEarly; }
            set { SetProperty(ref allowEarly, value); }
        }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        public SelectRequestTypePageViewModel() :
            base()
        {
            SelectTypeCommand = new RelayCommand<ClientRequestType>(SelectType);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        public void Loaded()
        {
            UpdateView();
        }

        private async void SelectType(ClientRequestType type)
        {
            Model.RequestType = type;

            await Model.AdjustMaxSubjects();
            Navigator.NextPage();
        }

        private void UpdateView()
        {
            Model.MaxSubjects = null;

            Comment = Model.SelectedService.Comment;
            CommentRowHeight = new GridLength(String.IsNullOrEmpty(Comment) ? 10 : 50);

            AdjustLive();
            AdjustEarly();
        }

        private async void AdjustLive()
        {
            AllowLive = false;

            if (!Model.SelectedService.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                LiveComment = EarlyComment = Translater.Message("DisabledByAdmin");
            }
            else
            {
                LiveComment = string.Empty;

                var timeIntervals = await Window.ExecuteLongTask(async () =>
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        return (await channel.Service.GetServiceFreeTime(Model.SelectedService.Id, ServerDateTime.Today, ClientRequestType.Live)).TimeIntervals;
                    }
                });

                if (timeIntervals != null && timeIntervals.Length > 0)
                {
                    Model.MaxSubjects = Math.Min(Model.SelectedService.MaxSubjects, timeIntervals.Length);
                    AllowLive = true;

                    LiveComment = Translater.Message("AvailableCount", timeIntervals.Length);
                }
                else
                {
                    LiveComment = Translater.Message("NoFreeTime");
                }
            }
        }

        private void AdjustEarly()
        {
            AllowEarly = false;

            if (Model.SelectedService.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                EarlyComment = string.Empty;
                AllowEarly = true;
            }
            else
            {
                EarlyComment = Translater.Message("DisabledByAdmin");
            }
        }

        private void Unloaded()
        {
            try
            {
                ChannelManager.Dispose();
            }
            catch { }
        }
    }
}
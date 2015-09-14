using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Terminal.Core;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
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
        public TerminalWindow Window { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public SelectRequestTypePageViewModel()
        {
            SelectTypeCommand = new RelayCommand<ClientRequestType>(SelectType);
        }

        private async void SelectType(ClientRequestType type)
        {
            Model.RequestType = type;
            var loading = Window.ShowLoading();

            try
            {
                await Model.AdjustMaxSubjects();
                Navigator.NextPage();
            }
            catch (FaultException exception)
            {
                Window.ShowWarning(exception.Reason.ToString());
            }
            catch (Exception exception)
            {
                Window.ShowWarning(exception.Message);
            }
            finally
            {
                loading.Hide();
            }
        }

        public async void Initialize()
        {
            await UpdateView();
        }

        private async Task UpdateView()
        {
            Model.MaxSubjects = null;

            Comment = Model.SelectedService.Comment;
            CommentRowHeight = new GridLength(String.IsNullOrEmpty(Comment) ? 10 : 50);

            await AdjustLive();
            AdjustEarly();
        }

        private async Task AdjustLive()
        {
            AllowLive = false;

            if (!Model.SelectedService.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                LiveComment = EarlyComment = Translater.Message("DisabledByAdmin");
                return;
            }

            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    LiveComment = string.Empty;

                    var timeIntervals = (await TaskPool.AddTask(channel.Service.GetServiceFreeTime(Model.SelectedService.Id, ServerDateTime.Today, ClientRequestType.Live))).TimeIntervals;
                    if (timeIntervals.Length > 0)
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
                catch (FaultException exception)
                {
                    LiveComment = exception.Reason.ToString();
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
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
    }
}
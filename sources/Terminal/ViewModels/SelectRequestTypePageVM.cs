using Junte.UI.WPF;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Queue.Terminal.ViewModels
{
    public class SelectRequestTypePageVM : PageVM
    {
        private string comment;
        private string liveComment;
        private string earlyComment;
        private bool allowLive;
        private bool allowEarly;
        private GridLength commentRowHeight;

        private Lazy<ICommand> selectTypeCommand;

        public SelectRequestTypePageVM()
        {
            selectTypeCommand = new Lazy<ICommand>(() => new RelayCommand<ClientRequestType>((type) =>
            {
                model.QueueType = type;
                navigator.NextPage();
            }));
        }

        public ICommand SelectTypeCommand { get { return this.selectTypeCommand.Value; } }

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

        public async void Initialize()
        {
            await UpdateView();
        }

        private async Task UpdateView()
        {
            model.MaxSubjects = null;

            Comment = model.SelectedService.Comment;
            CommentRowHeight = new GridLength(string.IsNullOrEmpty(Comment) ? 10 : 50);

            AllowLive = false;

            if (model.SelectedService.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
                {
                    var loading = screen.ShowLoading();

                    try
                    {
                        LiveComment = string.Empty;

                        var timeIntervals = (await taskPool.AddTask(channel.Service.GetServiceFreeTime(model.SelectedService.Id, ServerDateTime.Today, ClientRequestType.Live))).TimeIntervals;
                        if (timeIntervals.Length > 0)
                        {
                            model.MaxSubjects = Math.Min(model.SelectedService.MaxSubjects, timeIntervals.Length);
                            AllowLive = true;

                            LiveComment = string.Format("Доступно {0}", timeIntervals.Length);
                        }
                        else
                        {
                            LiveComment = "Нет свободного времени";
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
            else
            {
                LiveComment = "Отключено администратором";
            }

            AllowEarly = false;

            if (model.SelectedService.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                model.MaxSubjects = model.SelectedService.MaxSubjects;

                EarlyComment = string.Empty;
                AllowEarly = true;
            }
            else
            {
                EarlyComment = "Отключено администратором";
            }
        }
    }
}
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.UI.WPF;
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

        public SelectRequestTypePageViewModel()
        {
            SelectTypeCommand = new RelayCommand<ClientRequestType>(SelectType);
        }

        private async void SelectType(ClientRequestType type)
        {
            model.RequestType = type;
            LoadingControl loading = screen.ShowLoading();

            try
            {
                await Model.AdjustMaxSubjects();
                navigator.NextPage();
            }
            catch (FaultException exception)
            {
                screen.ShowWarning(exception.Reason.ToString());
            }
            catch (Exception exception)
            {
                screen.ShowWarning(exception.Message);
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
            model.MaxSubjects = null;

            Comment = model.SelectedService.Comment;
            CommentRowHeight = new GridLength(string.IsNullOrEmpty(Comment) ? 10 : 50);

            await AdjustLive();
            AdjustEarly();
        }

        private async Task AdjustLive()
        {
            AllowLive = false;

            if (model.SelectedService.LiveRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
            {
                using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
                {
                    LoadingControl loading = screen.ShowLoading();

                    try
                    {
                        LiveComment = string.Empty;

                        var timeIntervals = (await taskPool.AddTask(channel.Service.GetServiceFreeTime(model.SelectedService.Id, ServerDateTime.Today, ClientRequestType.Live))).TimeIntervals;
                        if (timeIntervals.Length > 0)
                        {
                            model.MaxSubjects = Math.Min(model.SelectedService.MaxSubjects, timeIntervals.Length);
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
            else
            {
                LiveComment = EarlyComment = Translater.Message("DisabledByAdmin");
            }
        }

        private void AdjustEarly()
        {
            AllowEarly = false;

            if (model.SelectedService.EarlyRegistrator.HasFlag(ClientRequestRegistrator.Terminal))
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
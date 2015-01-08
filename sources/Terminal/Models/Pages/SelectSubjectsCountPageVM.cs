using Junte.UI.WPF;
using Junte.UI.WPF.Types;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Printing;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Queue.Terminal.Models.Pages
{
    public class SelectSubjectsCountPageVM : PageVM
    {
        private bool canInc;
        private bool canDec;

        public SelectSubjectsCountPageVM()
        {
            PrevCommand = new RelayCommand(Prev);
            NextCommand = new RelayCommand(Next);
            DecCommand = new RelayCommand(DecSubjectsCount);
            IncCommand = new RelayCommand(IncSubjectsCount);
        }

        public ICommand NextCommand { get; set; }

        public ICommand PrevCommand { get; set; }

        public ICommand DecCommand { get; set; }

        public ICommand IncCommand { get; set; }

        public bool CanInc
        {
            get { return canInc; }
            set { SetProperty(ref canInc, value); }
        }

        public bool CanDec
        {
            get { return canDec; }
            set { SetProperty(ref canDec, value); }
        }

        public void Initialize()
        {
            UpdateIncDecEnable();
        }

        private void IncSubjectsCount()
        {
            if (Model.SubjectsCount < Model.MaxSubjects)
            {
                Model.SubjectsCount++;
            }
            UpdateIncDecEnable();
        }

        private void DecSubjectsCount()
        {
            if (Model.SubjectsCount > 1)
            {
                Model.SubjectsCount--;
            }
            UpdateIncDecEnable();
        }

        private void UpdateIncDecEnable()
        {
            CanInc = Model.SubjectsCount < Model.MaxSubjects;
            CanDec = Model.SubjectsCount > 1;
        }

        private async void Next()
        {
            using (var channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    await channel.Service.OpenUserSession(Model.CurrentAdministrator.SessionId);

                    var parameters = new Dictionary<Guid, object>();

                    ClientRequest clientRequest;

                    switch (Model.QueueType)
                    {
                        case ClientRequestType.Live:

                            clientRequest = await taskPool.AddTask(channel.Service.AddLiveClientRequest(Model.CurrentClient.Id,
                                                Model.SelectedService.Id,
                                                false,
                                                parameters,
                                                (int)Model.SubjectsCount));
                            break;

                        case ClientRequestType.Early:

                            clientRequest = await taskPool.AddTask(channel.Service.AddEarlyClientRequest(Model.CurrentClient.Id,
                                                Model.SelectedService.Id,
                                                Model.SelectedDate.Value,
                                                Model.SelectedTime.Value,
                                                parameters,
                                                (int)Model.SubjectsCount));
                            break;

                        default:
                            throw new SystemException();
                    }

                    ClientRequestCoupon data = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
                    string template = ServiceLocator.Current.GetInstance<CouponConfig>().Template;
                    string xpsFile = XPSGenerator.FromXaml(template, data);
                    try
                    {
                        PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                        defaultPrintQueue.AddJob(clientRequest.ToString(), xpsFile, false);
                    }
                    catch
                    {
                    }

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken token = cancellationTokenSource.Token;

                    NoticeControl notice = screen.ShowNotice("Спасибо, Вы записаны, возьмите талон", () =>
                    {
                        cancellationTokenSource.Cancel();
                        navigator.NextPage();
                    });

                    await Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        await loading.Dispatcher.BeginInvoke(
                            DispatcherPriority.Background,
                            new Action(() =>
                            {
                                if (!token.IsCancellationRequested)
                                {
                                    notice.Hide();
                                }
                            }));
                    }, token);
                }
                catch (FaultException exception)
                {
                    screen.ShowWarning(exception.Reason);
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

        private void Prev()
        {
            navigator.PrevPage();
        }
    }
}
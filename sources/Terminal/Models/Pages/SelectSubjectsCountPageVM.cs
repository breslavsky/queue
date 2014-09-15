using Junte.UI.WPF;
using Queue.Model.Common;
using Queue.Services.DTO;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

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
                    await channel.Service.OpenUserSession(Model.CurrentManager.SessionId);

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

                    string coupon = await taskPool.AddTask(channel.Service.GetClientRequestCoupon(clientRequest.Id));
                    string xpsFile = CreateXPS(coupon);

                    try
                    {
                        PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                        defaultPrintQueue.AddJob(clientRequest.ToString(), xpsFile, false);
                    }
                    catch
                    {
                        //exception
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

        private string CreateXPS(string coupon)
        {
            string xpsFile = Path.GetTempFileName() + ".xps";

            using (XmlTextReader xmlReader = new XmlTextReader(new StringReader(coupon)))
            using (Package container = Package.Open(xpsFile, FileMode.Create))
            using (XpsDocument document = new XpsDocument(container, CompressionOption.SuperFast))
            {
                Grid grid = (Grid)XamlReader.Load(xmlReader);

                FixedPage fixedPage = new FixedPage();
                fixedPage.Children.Add(grid);

                PageContent pageConent = new PageContent();
                ((IAddChild)pageConent).AddChild(fixedPage);

                FixedDocument fixedDocument = new FixedDocument();
                fixedDocument.Pages.Add(pageConent);

                XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(document);
                xpsDocumentWriter.Write(fixedDocument);
            }

            return xpsFile;
        }

        private void Prev()
        {
            navigator.PrevPage();
        }
    }
}
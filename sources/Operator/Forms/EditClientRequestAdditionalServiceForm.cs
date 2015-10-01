using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator
{
    public partial class EditClientRequestAdditionalServiceForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueOperator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly Guid clientRequestAdditionalServiceId;
        private readonly Guid clientRequestId;
        private readonly TaskPool taskPool;
        private ClientRequest clientRequest;
        private ClientRequestAdditionalService clientRequestAdditionalService;

        #endregion fields

        #region properties

        public ClientRequestAdditionalService ClientRequestAdditionalService
        {
            get { return clientRequestAdditionalService; }
            private set
            {
                clientRequestAdditionalService = value;

                additionalServiceControl.Select<AdditionalService>(clientRequestAdditionalService.AdditionalService);
                quantityUpDown.Value = (decimal)clientRequestAdditionalService.Quantity;
            }
        }

        #endregion properties

        public EditClientRequestAdditionalServiceForm(Guid? clientRequestId, Guid? clientRequestAdditionalServiceId = null)
            : base()
        {
            InitializeComponent();

            this.clientRequestId = clientRequestId.HasValue
                ? clientRequestId.Value : Guid.Empty;
            this.clientRequestAdditionalServiceId = clientRequestAdditionalServiceId.HasValue ?
                clientRequestAdditionalServiceId.Value : Guid.Empty;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void additionalServiceControl_Leave(object sender, EventArgs e)
        {
            clientRequestAdditionalService.AdditionalService = additionalServiceControl.Selected<AdditionalService>();
        }

        private void EditAdditionalServiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskPool.Cancel();

            if (components != null)
            {
                components.Dispose();
            }
            if (taskPool != null)
            {
                taskPool.Dispose();
            }
            if (channelManager != null)
            {
                channelManager.Dispose();
            }
        }

        private async void EditAdditionalServiceForm_Load(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    Enabled = false;

                    if (clientRequestId != Guid.Empty)
                    {
                        clientRequest = await taskPool.AddTask(channel.Service.GetClientRequest(clientRequestId));
                    }

                    additionalServiceControl.Initialize(await taskPool.AddTask(channel.Service.GetAdditionalServiceLinks()));

                    if (clientRequestAdditionalServiceId != Guid.Empty)
                    {
                        ClientRequestAdditionalService = await taskPool.AddTask(channel.Service.GetClientRequestAdditionalService(clientRequestAdditionalServiceId));
                    }
                    else
                    {
                        ClientRequestAdditionalService = new ClientRequestAdditionalService()
                        {
                            ClientRequest = clientRequest,
                            Quantity = 1
                        };
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
                finally
                {
                    Enabled = true;
                }
            }
        }

        private void quantityUpDown_Leave(object sender, EventArgs e)
        {
            clientRequestAdditionalService.Quantity = (float)quantityUpDown.Value;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    clientRequestAdditionalService = await taskPool.AddTask(channel.Service.EditClientRequestAdditionalService(clientRequestAdditionalService));

                    if (Saved != null)
                    {
                        Saved(this, EventArgs.Empty);
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
                finally
                {
                    saveButton.Enabled = true;
                }
            }
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}
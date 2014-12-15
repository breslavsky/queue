using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Administrator
{
    public partial class ClientRequestsForm : Queue.UI.WinForms.RichForm
    {
        private const byte PageSize = 50;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;

        private ClientRequestFilter filter = new ClientRequestFilter()
        {
            IsRequestDate = true
        };

        private int startIndex = 0;
        private TaskPool taskPool;

        public ClientRequestsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            stateControl.Initialize<ClientRequestState>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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
            base.Dispose(disposing);
        }

        private void ClientRequestsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void ClientRequestsForm_Load(object sender, EventArgs e)
        {
            requestDatePicker.Value = DateTime.Today;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    serviceControl.Initialize<Service>(await taskPool.AddTask(channel.Service.GetServiceList()));
                    operatorControl.Initialize<QueueOperator>(await taskPool.AddTask(channel.Service.GetUserList(UserRole.Operator)));

                    RefreshClienRequestsGridView();
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
            }
        }

        private void clientRequestsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = clientRequestsGridView.Rows[e.RowIndex];
                var clientRequest = row.Tag as ClientRequest;

                using (var form = new EditClientRequestForm(channelBuilder, currentUser, clientRequest.Id))
                {
                    form.ShowDialog();
                    RenderClientRequestsGridViewRow(row, form.ClientRequest);
                }
            }
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            RefreshClienRequestsGridView();
        }

        private async void RefreshClienRequestsGridView()
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    ClientRequest[] clientRequests = await taskPool.AddTask(channel.Service.FindClientRequests(startIndex, PageSize, filter));

                    clientRequestsGridView.Rows.Clear();
                    foreach (ClientRequest r in clientRequests)
                    {
                        int index = clientRequestsGridView.Rows.Add();
                        var row = clientRequestsGridView.Rows[index];

                        RenderClientRequestsGridViewRow(row, r);
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
            }
        }

        private void RenderClientRequestsGridViewRow(DataGridViewRow row, ClientRequest clientRequest)
        {
            row.Cells["numberColumn"].Value = clientRequest.Number;
            row.Cells["requestDateColumn"].Value = clientRequest.RequestDate.ToShortDateString();
            row.Cells["requestTimeColumn"].Value = clientRequest.RequestTime.ToString("hh\\:mm\\:ss");
            if (clientRequest.CallingStartTime != TimeSpan.Zero)
            {
                row.Cells["callingStartTimeColumn"].Value = clientRequest.CallingStartTime.ToString("hh\\:mm\\:ss");
            }

            var waitingStartTime = clientRequest.WaitingStartTime;
            if (waitingStartTime != TimeSpan.Zero)
            {
                row.Cells["waitingStartTimeColumn"].Value = waitingStartTime.ToString("hh\\:mm\\:ss");
            }

            var waitingTime = clientRequest.CallingFinishTime - waitingStartTime;
            if (waitingTime != TimeSpan.Zero)
            {
                row.Cells["waitingTimeColumn"].Value = waitingTime.ToString("hh\\:mm\\:ss");
            }

            if (clientRequest.CallingFinishTime != TimeSpan.Zero)
            {
                row.Cells["сallingFinishTimeColumn"].Value = clientRequest.CallingFinishTime.ToString("hh\\:mm\\:ss");
            }

            var callingTime = clientRequest.CallingFinishTime - clientRequest.CallingStartTime;
            if (callingTime != TimeSpan.Zero)
            {
                row.Cells["callingTimeColumn"].Value = callingTime.ToString("hh\\:mm\\:ss");
            }

            if (clientRequest.RenderStartTime != TimeSpan.Zero)
            {
                row.Cells["renderStartTimeColumn"].Value = clientRequest.RenderStartTime.ToString("hh\\:mm\\:ss");
            }

            if (clientRequest.RenderFinishTime != TimeSpan.Zero)
            {
                row.Cells["renderFinishTimeColumn"].Value = clientRequest.RenderFinishTime.ToString("hh\\:mm\\:ss");
            }

            var renderTime = clientRequest.RenderFinishTime - clientRequest.RenderStartTime;
            if (renderTime != TimeSpan.Zero)
            {
                row.Cells["renderTimeColumn"].Value = renderTime.ToString("hh\\:mm\\:ss");
            }

            if (clientRequest.Productivity > 0)
            {
                row.Cells["productivityColumn"].Value = clientRequest.Productivity;
            }

            row.Cells["subjectsColumn"].Value = clientRequest.Subjects.ToString();
            row.Cells["clientColumn"].Value = clientRequest.Client;
            row.Cells["operatorColumn"].Value = clientRequest.Operator;
            row.Cells["serviceColumn"].Value = clientRequest.Service;
            row.Cells["stateColumn"].Value = Translation.ClientRequestState.ResourceManager.GetString(clientRequest.State.ToString());

            row.Tag = clientRequest;
            row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(clientRequest.Color);
        }

        #region navigation

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (clientRequestsGridView.Rows.Count == PageSize)
            {
                startIndex += PageSize;
            }
            RefreshClienRequestsGridView();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            startIndex -= PageSize;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            RefreshClienRequestsGridView();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            startIndex = 0;
            RefreshClienRequestsGridView();
        }

        #endregion navigation

        #region bindings

        private void detailsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            callingStartTimeColumn.Visible =
                waitingStartTimeColumn.Visible =
                waitingTimeColumn.Visible =
                сallingFinishTimeColumn.Visible =
                callingTimeColumn.Visible =
                renderStartTimeColumn.Visible =
                renderFinishTimeColumn.Visible =
                renderTimeColumn.Visible =
                productivityColumn.Visible =
                detailsCheckBox.Checked;
        }

        private void operatorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filter.IsOperator = operatorPanel.Enabled
                = operatorCheckBox.Checked;
        }

        private void operatorControl_Leave(object sender, EventArgs e)
        {
            QueueOperator selectedOperator = operatorControl.Selected<QueueOperator>();
            if (selectedOperator != null)
            {
                filter.OperatorId = selectedOperator.Id;
            }
        }

        private void queryTextBox_Leave(object sender, EventArgs e)
        {
            filter.Query = queryTextBox.Text;
        }

        private void requestDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filter.IsRequestDate = requestDatePanel.Enabled
                = requestDateCheckBox.Checked;
        }

        private void requestDatePicker_ValueChanged(object sender, EventArgs e)
        {
            filter.RequestDate = requestDatePicker.Value;
        }

        private void serviceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filter.IsService = servicePanel.Enabled
                = serviceCheckBox.Checked;
        }

        private void serviceControl_Leave(object sender, EventArgs e)
        {
            Service selectedService = serviceControl.Selected<Service>();
            if (selectedService != null)
            {
                filter.ServiceId = selectedService.Id;
            }
        }

        private void stateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filter.IsState = statePanel.Enabled
                = stateCheckBox.Checked;
        }

        private void stateControl_Leave(object sender, EventArgs e)
        {
            filter.State = stateControl.Selected<ClientRequestState>();
        }

        #endregion bindings
    }
}
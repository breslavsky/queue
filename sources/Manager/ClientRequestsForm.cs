using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Manager
{
    public partial class ClientRequestsForm : Queue.UI.WinForms.RichForm
    {
        private const byte PAGE_SIZE = 50;
        private int startIndex = 0;

        private bool loaded = false;

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        public ClientRequestsForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();

            stateComboBox.DisplayMember = DataListItem.Value;
            stateComboBox.ValueMember = DataListItem.Key;
            stateComboBox.DataSource = EnumDataListItem.GetList<ClientRequestState>();
        }

        private bool isRequestDate
        {
            get { return requestDateCheckBox.Checked; }
            set { requestDateCheckBox.Checked = value; }
        }

        private bool isOperator
        {
            get { return operatorCheckBox.Checked; }
            set { operatorCheckBox.Checked = value; }
        }

        private bool isService
        {
            get { return serviceCheckBox.Checked; }
            set { serviceCheckBox.Checked = value; }
        }

        private bool isState
        {
            get { return stateCheckBox.Checked; }
            set { stateCheckBox.Checked = value; }
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

        private async void ClientRequestsForm_Load(object sender, EventArgs e)
        {
            requestDatePicker.Value = DateTime.Today;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorComboBox.Enabled = false;

                    await channel.Service.OpenUserSession(currentUser.SessionId);

                    var operators = await taskPool.AddTask(channel.Service.GetUserList(UserRole.Operator));
                    if (operators.Count > 0)
                    {
                        operatorComboBox.DisplayMember = DataListItem.Value;
                        operatorComboBox.ValueMember = DataListItem.Key;
                        operatorComboBox.DataSource = new BindingSource(operators, null);
                        operatorComboBox.Enabled = true;
                    }

                    serviceComboBox.Enabled = false;

                    var services = await taskPool.AddTask(channel.Service.GetServiceList());
                    if (services.Count > 0)
                    {
                        serviceComboBox.DisplayMember = DataListItem.Value;
                        serviceComboBox.ValueMember = DataListItem.Key;
                        serviceComboBox.DataSource = new BindingSource(services, null);
                        serviceComboBox.Enabled = true;
                    }

                    RefreshGridView();
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

            loaded = true;
        }

        private async void RefreshGridView()
        {
            var filter = new ClientRequestFilter()
            {
                Query = queryTextBox.Text.Trim()
            };

            object value = requestDatePicker.Value;

            if (isRequestDate && value != null)
            {
                filter.RequestDate = requestDatePicker.Value;
            }

            value = operatorComboBox.SelectedValue;

            if (isOperator && value != null)
            {
                filter.OperatorId = (Guid)value;
            }

            value = serviceComboBox.SelectedValue;

            if (isService && value != null)
            {
                filter.ServiceId = (Guid)value;
            }

            value = stateComboBox.SelectedValue;

            if (isState && value != null)
            {
                filter.State = (ClientRequestState?)value;
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(currentUser.SessionId);
                    var clientRequests = await taskPool.AddTask(channel.Service.FindClientRequests(startIndex, PAGE_SIZE, filter));

                    gridView.Rows.Clear();
                    foreach (var r in clientRequests)
                    {
                        int index = gridView.Rows.Add();
                        var row = gridView.Rows[index];

                        RenderGridViewRow(row, r);
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

        private void RenderGridViewRow(DataGridViewRow row, ClientRequest clientRequest)
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

        private void gridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = gridView.Rows[rowIndex];
                    var clientRequest = row.Tag as ClientRequest;

                    using (var form = new EditClientRequestForm(channelBuilder, currentUser, clientRequest.Id))
                    {
                        form.ShowDialog();
                        //RenderGridRow(row, form.ClientRequest);
                    }
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            startIndex = 0;
            RefreshGridView();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            startIndex -= PAGE_SIZE;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            RefreshGridView();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (gridView.Rows.Count == PAGE_SIZE)
            {
                startIndex += PAGE_SIZE;
            }
            RefreshGridView();
        }

        private void requestDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            requestDatePanel.Enabled = isRequestDate;
        }

        private void operatorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            operatorPanel.Enabled = isOperator;
        }

        private void serviceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            servicePanel.Enabled = isService;
        }

        private void stateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            statePanel.Enabled = isState;
        }

        private void filterChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                startIndex = 0;
                RefreshGridView();
            }
        }

        private void ClientRequestsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

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
    }
}
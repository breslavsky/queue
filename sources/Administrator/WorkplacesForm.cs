using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class WorkplacesForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public WorkplacesForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private async void WorkplacesForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    foreach (var workplace in await taskPool.AddTask(channel.Service.GetWorkplaces()))
                    {
                        int index = workplacesGridView.Rows.Add();
                        var row = workplacesGridView.Rows[index];

                        WorkplacesGridViewRenderRow(row, workplace);
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

        private void WorkplacesGridViewRenderRow(DataGridViewRow row, Workplace workplace)
        {
            row.Cells["typeColumn"].Value = workplace.Type.Translate();
            row.Cells["numberColumn"].Value = workplace.Number;
            row.Cells["modificatorColumn"].Value = workplace.Modificator.Translate();
            row.Cells["commentColumn"].Value = workplace.Comment;
            row.Cells["displayColumn"].Value = workplace.Display;
            row.Cells["segmentsColumn"].Value = workplace.Segments;
            row.Tag = workplace;
        }

        private async void workplacesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = workplacesGridView.Rows[rowIndex];
                var cell = row.Cells[columnIndex];

                Workplace workplace = row.Tag as Workplace;

                switch (cell.OwningColumn.Name)
                {
                    case "deleteColumn":

                        if (MessageBox.Show("Вы действительно хотите удалить рабочее место?",
                            "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            using (var channel = channelManager.CreateChannel())
                            {
                                try
                                {
                                    deleteColumn.ReadOnly = true;

                                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                                    await taskPool.AddTask(channel.Service.DeleteWorkplace(workplace.Id));

                                    workplacesGridView.Rows.Remove(row);
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
                                    deleteColumn.ReadOnly = false;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void workplacesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = workplacesGridView.Rows[rowIndex];
                Workplace workplace = row.Tag as Workplace;

                using (var f = new EditWorkplaceForm(channelBuilder, currentUser, workplace.Id))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        WorkplacesGridViewRenderRow(row, f.Workplace);
                    }
                }
            }
        }

        private async void addWorkplaceButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addWorkplaceButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    var workplace = await taskPool.AddTask(channel.Service.AddWorkplace());

                    var row = workplacesGridView.Rows[workplacesGridView.Rows.Add()];

                    WorkplacesGridViewRenderRow(row, workplace);

                    using (var f = new EditWorkplaceForm(channelBuilder, currentUser, workplace.Id))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            WorkplacesGridViewRenderRow(row, f.Workplace);
                        }
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
                    addWorkplaceButton.Enabled = true;
                }
            }
        }

        private void WorkplacesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
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
    }
}
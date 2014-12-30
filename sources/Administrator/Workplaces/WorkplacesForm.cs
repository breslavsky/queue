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
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public WorkplacesForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
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

        private void addWorkplaceButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditWorkplaceForm(channelBuilder, currentUser))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = workplacesGridView.Rows[workplacesGridView.Rows.Add()];
                        row.Selected = true;
                    }
                    WorkplacesGridViewRenderRow(row, f.Workplace);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void WorkplacesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void WorkplacesForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
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
                    f.Saved += (s, eventArgs) =>
                    {
                        WorkplacesGridViewRenderRow(row, f.Workplace);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private async void workplacesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить рабочее место?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Workplace workplace = e.Row.Tag as Workplace;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteWorkplace(workplace.Id));
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
    }
}
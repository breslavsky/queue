using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class WorkplacesForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        public WorkplacesForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

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
            using (var f = new EditWorkplaceForm())
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

                using (var f = new EditWorkplaceForm(workplace.Id))
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
            row.Cells["typeColumn"].Value = Translater.Enum(workplace.Type);
            row.Cells["numberColumn"].Value = workplace.Number;
            row.Cells["modificatorColumn"].Value = Translater.Enum(workplace.Modificator);
            row.Cells["commentColumn"].Value = workplace.Comment;
            row.Cells["displayDeviceIdColumn"].Value = workplace.DisplayDeviceId;
            row.Cells["qualityPanelDeviceId"].Value = workplace.QualityPanelDeviceId;
            row.Cells["segmentsColumn"].Value = workplace.Segments;
            row.Tag = workplace;
        }
    }
}
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

            typeColumn.DisplayMember = DataListItem.Value;
            typeColumn.ValueMember = DataListItem.Key;
            typeColumn.DataSource = EnumDataListItem<WorkplaceType>.GetList();

            modificatorColumn.DisplayMember = DataListItem.Value;
            modificatorColumn.ValueMember = DataListItem.Key;
            modificatorColumn.DataSource = EnumDataListItem<WorkplaceModificator>.GetList();
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

                        row.Cells["typeColumn"].Value = workplace.Type;
                        row.Cells["numberColumn"].Value = workplace.Number.ToString();
                        row.Cells["modificatorColumn"].Value = workplace.Modificator;
                        row.Cells["commentColumn"].Value = workplace.Comment;
                        row.Cells["displayColumn"].Value = workplace.Display.ToString();
                        row.Cells["segmentsColumn"].Value = workplace.Segments.ToString();
                        row.Tag = workplace;
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

        private async void workplacesGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = workplacesGridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    var workplace = row.Tag as Workplace;
                    workplace.Type = (WorkplaceType)row.Cells["typeColumn"].Value;

                    object column = row.Cells["numberColumn"].Value;
                    if (!string.IsNullOrWhiteSpace((string)column))
                    {
                        try
                        {
                            workplace.Number = Convert.ToInt32(column.ToString());
                        }
                        catch
                        {
                            UIHelper.Warning("Указан не верный номер рабочего места");
                            return;
                        }
                    }

                    workplace.Modificator = (WorkplaceModificator)row.Cells["modificatorColumn"].Value;

                    column = row.Cells["commentColumn"].Value;
                    if (!string.IsNullOrWhiteSpace((string)column))
                    {
                        try
                        {
                            workplace.Comment = column.ToString();
                        }
                        catch
                        {
                            UIHelper.Warning("Указан не верный номер табло");
                            return;
                        }
                    }

                    column = row.Cells["displayColumn"].Value;
                    if (!string.IsNullOrWhiteSpace((string)column))
                    {
                        try
                        {
                            workplace.Display = Convert.ToByte(column.ToString());
                        }
                        catch
                        {
                            UIHelper.Warning("Указан не верный номер табло");
                            return;
                        }
                    }

                    column = row.Cells["segmentsColumn"].Value;
                    if (!string.IsNullOrWhiteSpace((string)column))
                    {
                        try
                        {
                            workplace.Segments = Convert.ToByte(column.ToString());
                        }
                        catch
                        {
                            UIHelper.Warning("Указано не верное кол-во сегментов");
                            return;
                        }
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.EditWorkplace(workplace));
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
        }

        private async void workplacesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = workplacesGridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    Workplace workplace = (Workplace)row.Tag;

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
        }

        private async void addWorkplaceButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addWorkplaceButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    Workplace workplace = await taskPool.AddTask(channel.Service.AddWorkplace());

                    int index = workplacesGridView.Rows.Add();
                    var row = workplacesGridView.Rows[index];

                    row.Cells["typeColumn"].Value = workplace.Type;
                    row.Cells["numberColumn"].Value = workplace.Number.ToString();
                    row.Cells["modificatorColumn"].Value = workplace.Modificator;
                    row.Tag = workplace;
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
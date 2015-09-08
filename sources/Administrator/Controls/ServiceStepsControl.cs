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
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ServiceStepsControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private Service service;

        #endregion fields

        #region properties

        public Service Service
        {
            get
            {
                return service;
            }
            set
            {
                stepsGridView.Rows.Clear();

                service = value;
                if (service != null)
                {
                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                foreach (var s in await taskPool.AddTask(channel.Service.GetServiceSteps(service.Id)))
                                {
                                    RenderStepsGridViewRow(stepsGridView.Rows[stepsGridView.Rows.Add()], s);
                                }

                                Enabled = true;
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
                    }));
                }
            }
        }

        #endregion properties

        public ServiceStepsControl()
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

        private void RenderStepsGridViewRow(DataGridViewRow row, ServiceStep serviceStep)
        {
            row.Cells["nameColumn"].Value = serviceStep.Name;
            row.Tag = serviceStep;
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditServiceStepForm(service.Id))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = stepsGridView.Rows[stepsGridView.Rows.Add()];
                    }
                    RenderStepsGridViewRow(row, f.ServiceStep);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void stepsGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            ServiceStep serviceStep = e.Row.Tag as ServiceStep;

            if (MessageBox.Show("Вы действительно хотите удалить этап услуги?",
                            "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await channel.Service.DeleteServiceStep(serviceStep.Id);
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

        private void stepsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = stepsGridView.Rows[e.RowIndex];
                ServiceStep serviceStep = row.Tag as ServiceStep;

                using (var f = new EditServiceStepForm(null, serviceStep.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        RenderStepsGridViewRow(row, f.ServiceStep);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private async void serviceStepUpButton_Click(object sender, EventArgs e)
        {
            if (stepsGridView.SelectedRows.Count > 0)
            {
                var currentRow = stepsGridView.SelectedRows[0];
                int currentRowIndex = currentRow.Index;
                if (currentRowIndex > 0)
                {
                    var serviceStep = currentRow.Tag as ServiceStep;

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            if (await channel.Service.ServiceStepUp(serviceStep.Id))
                            {
                                int prevRowIndex = currentRowIndex - 1;
                                stepsGridView.ClearSelection();
                                stepsGridView.Rows.RemoveAt(currentRowIndex);
                                stepsGridView.Rows.Insert(prevRowIndex, currentRow);
                                currentRow.Selected = true;
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
            }
        }

        private async void serviceStepDownButton_Click(object sender, EventArgs e)
        {
            if (stepsGridView.SelectedRows.Count > 0)
            {
                var currentRow = stepsGridView.SelectedRows[0];
                int currentRowIndex = currentRow.Index;
                if (currentRowIndex < stepsGridView.Rows.Count - 1)
                {
                    var serviceStep = currentRow.Tag as ServiceStep;

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            if (await channel.Service.ServiceStepDown(serviceStep.Id))
                            {
                                int nextRowIndex = currentRowIndex + 1;
                                stepsGridView.ClearSelection();
                                stepsGridView.Rows.RemoveAt(currentRowIndex);
                                stepsGridView.Rows.Insert(nextRowIndex, currentRow);
                                currentRow.Selected = true;
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
            }
        }
    }
}
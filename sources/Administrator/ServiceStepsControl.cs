using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ServiceStepsControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private Service service;

        #endregion fields

        #region properties

        public Service Service
        {
            set
            {
                service = value;
                if (service != null)
                {
                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                await channel.Service.OpenUserSession(currentUser.SessionId);

                                gridView.Rows.Clear();
                                foreach (var s in await taskPool.AddTask(channel.Service.GetServiceSteps(service.Id)))
                                {
                                    GridViewRenderRow(gridView.Rows[gridView.Rows.Add()], s);
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
                    }));
                }
            }
        }

        #endregion properties

        public ServiceStepsControl()
        {
            InitializeComponent();
        }

        public void Initialize(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void GridViewRenderRow(DataGridViewRow row, ServiceStep serviceStep)
        {
            row.Cells["nameColumn"].Value = serviceStep.Name;
            row.Tag = serviceStep;
        }

        private async void addStepButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addStepButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    var serviceStep = await taskPool.AddTask(channel.Service.AddServiceStep(service.Id));

                    GridViewRenderRow(gridView.Rows[gridView.Rows.Add()], serviceStep);
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
                    addStepButton.Enabled = true;
                }
            }
        }

        private async void gridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = gridView.Rows[rowIndex];
                    var serviceStep = row.Tag as ServiceStep;

                    string name = row.Cells["nameColumn"].Value as string;
                    if (name != null)
                    {
                        serviceStep.Name = name;
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            row.ReadOnly = true;

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.EditServiceStep(serviceStep));
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
                            row.ReadOnly = false;
                        }
                    }
                }
            }
        }

        private async void gridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = gridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    var serviceStep = row.Tag as ServiceStep;

                    switch (cell.OwningColumn.Name)
                    {
                        case "deleteColumn":

                            if (MessageBox.Show("Вы действительно хотите удалить этап услуги?",
                                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var channel = channelManager.CreateChannel())
                                {
                                    try
                                    {
                                        await channel.Service.OpenUserSession(currentUser.SessionId);
                                        await channel.Service.DeleteServiceStep(serviceStep.Id);

                                        gridView.Rows.Remove(row);
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
                            break;
                    }
                }
            }
        }
    }
}
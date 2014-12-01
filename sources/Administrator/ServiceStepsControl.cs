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

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
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

                                serviceStepsGridView.Rows.Clear();
                                foreach (var s in await taskPool.AddTask(channel.Service.GetServiceSteps(service.Id)))
                                {
                                    ServiceStepsGridViewRenderRow(serviceStepsGridView.Rows[serviceStepsGridView.Rows.Add()], s);
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

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void ServiceStepsGridViewRenderRow(DataGridViewRow row, ServiceStep serviceStep)
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

                    var row = serviceStepsGridView.Rows[serviceStepsGridView.Rows.Add()];

                    ServiceStepsGridViewRenderRow(row, serviceStep);

                    using (var f = new EditServiceStepForm(channelBuilder, currentUser, serviceStep.Id))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            ServiceStepsGridViewRenderRow(row, f.ServiceStep);
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
                    addStepButton.Enabled = true;
                }
            }
        }

        private async void serviceStepsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = serviceStepsGridView.Rows[rowIndex];
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

                                    serviceStepsGridView.Rows.Remove(row);
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

        private void serviceStepsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = serviceStepsGridView.Rows[rowIndex];
                ServiceStep serviceStep = row.Tag as ServiceStep;

                using (var f = new EditServiceStepForm(channelBuilder, currentUser, serviceStep.Id))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        ServiceStepsGridViewRenderRow(row, f.ServiceStep);
                    }
                }
            }
        }
    }
}
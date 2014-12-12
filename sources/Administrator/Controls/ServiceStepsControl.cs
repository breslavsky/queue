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

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
        }

        private void ServiceStepsGridViewRenderRow(DataGridViewRow row, ServiceStep serviceStep)
        {
            row.Cells["nameColumn"].Value = serviceStep.Name;
            row.Tag = serviceStep;
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditServiceStepForm(channelBuilder, currentUser, service.Id))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var row = serviceStepsGridView.Rows[serviceStepsGridView.Rows.Add()];
                    ServiceStepsGridViewRenderRow(row, f.ServiceStep);
                }
            }
        }

        private async void serviceStepsGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
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

        private void serviceStepsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = serviceStepsGridView.Rows[e.RowIndex];
                ServiceStep serviceStep = row.Tag as ServiceStep;

                using (var f = new EditServiceStepForm(channelBuilder, currentUser, service.Id, serviceStep.Id))
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
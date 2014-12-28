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
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
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

        private void RenderStepsGridViewRow(DataGridViewRow row, ServiceStep serviceStep)
        {
            row.Cells["nameColumn"].Value = serviceStep.Name;
            row.Tag = serviceStep;
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditServiceStepForm(channelBuilder, currentUser, service.Id))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = stepsGridView.Rows[stepsGridView.Rows.Add()];
                    }
                    RenderStepsGridViewRow(row, f.ServiceStep);
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

                using (var f = new EditServiceStepForm(channelBuilder, currentUser, service.Id, serviceStep.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        RenderStepsGridViewRow(row, f.ServiceStep);
                    };

                    f.ShowDialog();
                }
            }
        }
    }
}
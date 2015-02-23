using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class AdditionalServicesForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        private BindingList<AdditionalService> services;

        public AdditionalServicesForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            InitializeComponent();
        }

        private async void AdditionalServicesForm_Load(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    AdditionalService[] result = await taskPool.AddTask(channel.Service.GetAdditionalServices());
                    services = new BindingList<AdditionalService>(new List<AdditionalService>(result));
                    additionalServiceBindingSource.DataSource = services;
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void addAdditionalServiceButton_Click(object sender, EventArgs e)
        {
            using (EditAdditionalServiceForm f = new EditAdditionalServiceForm(channelBuilder, currentUser))
            {
                f.Saved += (s, eventArgs) =>
                {
                    services.Add(f.Service);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void AdditionalServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void additionalServicesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (additionalServicesGridView.CurrentRow == null)
            {
                return;
            }

            AdditionalService additionalService = services[additionalServicesGridView.CurrentRow.Index];

            using (EditAdditionalServiceForm f = new EditAdditionalServiceForm(channelBuilder, currentUser, additionalService.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    int index = services.IndexOf(additionalService);
                    services.RemoveAt(index);
                    services.Insert(index, f.Service);

                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void additionalServicesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (additionalServicesGridView.CurrentRow == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить дополнительную услугу?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AdditionalService additionalService = services[additionalServicesGridView.CurrentRow.Index];

                using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteAdditionalService(additionalService.Id));
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
            else
            {
                e.Cancel = true;
            }
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
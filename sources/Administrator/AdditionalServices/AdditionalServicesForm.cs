using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
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

        private void addAdditionalServiceButton_Click(object sender, EventArgs e)
        {
            /*using (var f = new EditOfficeForm(channelBuilder, currentUser))
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = additionalServicesGridView.Rows[additionalServicesGridView.Rows.Add()];
                    }

                    AdditionalServicesGridViewRenderRow(row, f.Office);
                    f.Close();
                };

                f.ShowDialog();
            }*/
        }

        private void AdditionalServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void AdditionalServicesForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    foreach (var a in await taskPool.AddTask(channel.Service.GetAdditionalServices()))
                    {
                        var row = additionalServicesGridView.Rows[additionalServicesGridView.Rows.Add()];
                        AdditionalServicesGridViewRenderRow(row, a);
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

        private void additionalServicesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = additionalServicesGridView.Rows[rowIndex];
                AdditionalService additionalService = row.Tag as AdditionalService;

                /*using (var f = new EditOfficeForm(channelBuilder, currentUser, office.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        AdditionalServicesGridViewRenderRow(row, f.Office);
                        f.Close();
                    };

                    f.ShowDialog();
                }*/
            }
        }

        private async void additionalServicesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить дополнительную услугу?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AdditionalService additionalService = e.Row.Tag as AdditionalService;

                using (var channel = channelManager.CreateChannel())
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

        private void AdditionalServicesGridViewRenderRow(DataGridViewRow row, AdditionalService additionalService)
        {
            row.Cells["nameColumn"].Value = additionalService.Name;
            row.Cells["priceColumn"].Value = additionalService.Price;
            row.Cells["measureColumn"].Value = additionalService.Measure;
            row.Tag = additionalService;
        }
    }
}
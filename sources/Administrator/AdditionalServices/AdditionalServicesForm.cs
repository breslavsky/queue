using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class AdditionalServicesForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;

        #endregion fields

        private BindingList<AdditionalService> additionalServices;

        public AdditionalServicesForm()
            : base()
        {
            InitializeComponent();

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private async void AdditionalServicesForm_Load(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    additionalServices = new BindingList<AdditionalService>(new List<AdditionalService>
                        (await taskPool.AddTask(channel.Service.GetAdditionalServices())));
                    additionalServicesBindingSource.DataSource = additionalServices;
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

        private void addButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditAdditionalServiceForm())
            {
                f.Saved += (s, eventArgs) =>
                {
                    additionalServices.Add(f.AdditionalService);
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
            var currentRow = additionalServicesGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            var additionalService = additionalServices[currentRow.Index];

            using (var f = new EditAdditionalServiceForm(additionalService.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    additionalService.Update(f.AdditionalService);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void additionalServicesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var currentRow = additionalServicesGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить дополнительную услугу?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AdditionalService additionalService = additionalServices[currentRow.Index];

                using (Channel<IServerTcpService> channel = ChannelManager.CreateChannel())
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
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
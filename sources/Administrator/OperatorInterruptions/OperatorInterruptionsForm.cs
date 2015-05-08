using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class OperatorInterruptionsForm : RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        private BindingList<OperatorInterruption> operatorInterruptions;

        public OperatorInterruptionsForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
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

        private async void OperatorInterruptionsForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorInterruptions = new BindingList<OperatorInterruption>(new List<OperatorInterruption>
                        (await taskPool.AddTask(channel.Service.GetOperatorInterruptions())));
                    operatorInterruptionsBindingSource.DataSource = operatorInterruptions;
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
            using (var f = new EditOperatorInterruptionForm(channelBuilder, currentUser))
            {
                f.Saved += (s, eventArgs) =>
                {
                    operatorInterruptions.Add(f.OperatorInterruption);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void OperatorInterruptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void operatorInterruptionsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var currentRow = operatorInterruptionsGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            OperatorInterruption operatorInterruption = operatorInterruptions[currentRow.Index];

            using (var f = new EditOperatorInterruptionForm(channelBuilder, currentUser, operatorInterruption.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    operatorInterruption.Update(f.OperatorInterruption);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void operatorInterruptionsGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var currentRow = operatorInterruptionsGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить перерыв оператора?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OperatorInterruption operatorInterruption = operatorInterruptions[currentRow.Index];

                using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteOperatorInterruption(operatorInterruption.Id));
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
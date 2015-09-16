using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Administrator
{
    public partial class OperatorInterruptionsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private BindingList<OperatorInterruption> operatorInterruptions;
        private readonly OperatorInterruptionFilter filter = new OperatorInterruptionFilter();

        #endregion fields

        public OperatorInterruptionsForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
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

        #region form events

        private void OperatorInterruptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void OperatorInterruptionsForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(UserRole.Operator)));

                    RefreshOperatorInterruptionsGridView();
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

        #endregion form events

        private async void RefreshOperatorInterruptionsGridView()
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    operatorInterruptions = new BindingList<OperatorInterruption>(new List<OperatorInterruption>
                        (await taskPool.AddTask(channel.Service.GetOperatorInterruptions(filter))));
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

        private void operatorInterruptionsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var currentRow = operatorInterruptionsGridView.CurrentRow;
            if (currentRow == null)
            {
                return;
            }

            var operatorInterruption = operatorInterruptions[currentRow.Index];

            using (var f = new EditOperatorInterruptionForm(operatorInterruption.Id))
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
                var operatorInterruption = operatorInterruptions[currentRow.Index];

                using (var channel = channelManager.CreateChannel())
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

        private void addButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditOperatorInterruptionForm())
            {
                f.Saved += (s, eventArgs) =>
                {
                    operatorInterruptions.Add(f.OperatorInterruption);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        #region taskpool

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #endregion taskpool

        #region filter bindings

        private void operatorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filter.IsOperator = operatorPanel.Enabled
                = operatorCheckBox.Checked;
        }

        private void operatorControl_Leave(object sender, EventArgs e)
        {
            var selectedOperator = operatorControl.Selected<QueueOperator>();
            if (selectedOperator != null)
            {
                filter.OperatorId = selectedOperator.Id;
            }
        }

        #endregion filter bindings

        private void filterButton_Click(object sender, EventArgs e)
        {
            RefreshOperatorInterruptionsGridView();
        }
    }
}
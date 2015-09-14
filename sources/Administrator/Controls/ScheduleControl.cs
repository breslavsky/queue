using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ScheduleControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private Schedule schedule;

        #endregion fields

        #region properties

        public Schedule Schedule
        {
            get
            {
                return schedule;
            }
            set
            {
                serviceRenderingsGridView.Rows.Clear();

                schedule = value;
                if (schedule != null)
                {
                    Enabled = true;

                    isWorkedCheckBox.Checked = schedule.IsWorked;
                    startTimeTextBox.Text = schedule.StartTime.ToString("hh\\:mm");
                    finishTimeTextBox.Text = schedule.FinishTime.ToString("hh\\:mm");
                    liveClientIntervalUpDown.Value = (decimal)schedule.LiveClientInterval.TotalMinutes;
                    intersectionUpDown.Value = (decimal)schedule.Intersection.TotalMinutes;
                    maxClientRequestsUpDown.Value = schedule.MaxClientRequests;
                    renderingModeControl.Select<ServiceRenderingMode>(schedule.RenderingMode);
                    earlyStartTimeTextBox.Text = schedule.EarlyStartTime.ToString("hh\\:mm");
                    earlyFinishTimeTextBox.Text = schedule.FinishTime.ToString("hh\\:mm");
                    earlyReservationUpDown.Value = schedule.EarlyReservation;
                    earlyClientIntervalUpDown.Value = (decimal)schedule.EarlyClientInterval.TotalMinutes;

                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                foreach (var r in await taskPool.AddTask(channel.Service.GetServiceRenderings(schedule.Id)))
                                {
                                    var row = serviceRenderingsGridView.Rows[serviceRenderingsGridView.Rows.Add()];
                                    ServiceRenderingsGridViewRenderRow(row, r);
                                }

                                serviceStepColumn.Visible = schedule is ServiceSchedule;
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
                else
                {
                    Enabled = false;
                }
            }
        }

        #endregion properties

        public ScheduleControl()
        {
            InitializeComponent();

            if (designtime)
            {
                return;
            }

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            renderingModeControl.Initialize<ServiceRenderingMode>();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void ServiceRenderingsGridViewRenderRow(DataGridViewRow row, ServiceRendering serviceRendering)
        {
            row.Cells["operatorColumn"].Value = serviceRendering.Operator;
            row.Cells["serviceStepColumn"].Value = serviceRendering.ServiceStep;
            row.Cells["modeColumn"].Value = Translater.Enum(serviceRendering.Mode);
            row.Cells["priorityColumn"].Value = serviceRendering.Priority;

            row.Tag = serviceRendering;
        }

        private void addServiceRenderingButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            using (var f = new EditServiceRenderingForm(schedule.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = serviceRenderingsGridView.Rows[serviceRenderingsGridView.Rows.Add()];
                    }
                    ServiceRenderingsGridViewRenderRow(row, f.ServiceRendering);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void serviceRenderingsGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            ServiceRendering serviceRendering = e.Row.Tag as ServiceRendering;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.DeleteServiceRendering(serviceRendering.Id));
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

        private void serviceRenderingsGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = serviceRenderingsGridView.Rows[e.RowIndex];
                ServiceRendering serviceRendering = row.Tag as ServiceRendering;

                using (var f = new EditServiceRenderingForm(schedule.Id, serviceRendering.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        ServiceRenderingsGridViewRenderRow(row, f.ServiceRendering);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        #region bindings

        private void isWorkedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            schedulePanel.Enabled = isWorkedCheckBox.Checked;
        }

        private void isWorkedCheckBox_Leave(object sender, EventArgs e)
        {
            schedule.IsWorked = isWorkedCheckBox.Checked;
        }

        private void startTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.StartTime = TimeSpan.Parse(startTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void finishTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.FinishTime = TimeSpan.Parse(finishTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void liveClientIntervalUpDown_Leave(object sender, EventArgs e)
        {
            schedule.LiveClientInterval = TimeSpan.FromMinutes((double)liveClientIntervalUpDown.Value);
        }

        private void intersectionUpDown_Leave(object sender, EventArgs e)
        {
            schedule.Intersection = TimeSpan.FromMinutes((double)intersectionUpDown.Value);
        }

        private void maxClientRequestsUpDown_Leave(object sender, EventArgs e)
        {
            schedule.MaxClientRequests = (int)maxClientRequestsUpDown.Value;
        }

        private void renderingModeControl_Leave(object sender, EventArgs e)
        {
            schedule.RenderingMode = renderingModeControl.Selected<ServiceRenderingMode>();
        }

        private void renderingModeComboBox_Leave(object sender, EventArgs e)
        {
            schedule.RenderingMode = renderingModeControl.Selected<ServiceRenderingMode>();
        }

        private void earlyStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.EarlyStartTime = TimeSpan.Parse(earlyStartTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void earlyFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.EarlyFinishTime = TimeSpan.Parse(earlyFinishTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void earlyReservationUpDown_Leave(object sender, EventArgs e)
        {
            schedule.EarlyReservation = (int)earlyReservationUpDown.Value;
        }

        private void earlyClientIntervalUpDown_Leave(object sender, EventArgs e)
        {
            schedule.EarlyClientInterval = TimeSpan.FromMinutes((double)earlyClientIntervalUpDown.Value);
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    schedule = await taskPool.AddTask(channel.Service.EditSchedule(schedule));
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
                    saveButton.Enabled = true;
                }
            }
        }
    }
}
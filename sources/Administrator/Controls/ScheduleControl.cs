﻿using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ScheduleControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Schedule schedule;

        #endregion fields

        #region properties

        public Schedule Schedule
        {
            get { return schedule; }
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
                    isInterruptionCheckBox.Checked = schedule.IsInterruption;
                    interruptionStartTimeTextBox.Text = schedule.InterruptionStartTime.ToString("hh\\:mm");
                    interruptionFinishTimeTextBox.Text = schedule.InterruptionFinishTime.ToString("hh\\:mm");
                    clientIntervalUpDown.Value = (decimal)schedule.ClientInterval.TotalMinutes;
                    intersectionUpDown.Value = (decimal)schedule.Intersection.TotalMinutes;
                    maxClientRequestsUpDown.Value = schedule.MaxClientRequests;
                    renderingModeControl.Select<ServiceRenderingMode>(schedule.RenderingMode);
                    earlyStartTimeTextBox.Text = schedule.EarlyStartTime.ToString("hh\\:mm");
                    earlyFinishTimeTextBox.Text = schedule.FinishTime.ToString("hh\\:mm");
                    earlyReservationUpDown.Value = schedule.EarlyReservation;

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

            renderingModeControl.Initialize<ServiceRenderingMode>();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
        }

        private void ServiceRenderingsGridViewRenderRow(DataGridViewRow row, ServiceRendering serviceRendering)
        {
            row.Cells["operatorColumn"].Value = serviceRendering.Operator;
            row.Cells["serviceStepColumn"].Value = serviceRendering.ServiceStep;
            row.Cells["modeColumn"].Value = serviceRendering.Mode.Translate();
            row.Cells["priorityColumn"].Value = serviceRendering.Priority;

            row.Tag = serviceRendering;
        }

        private void addServiceRenderingButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            using (var f = new EditServiceRenderingForm(channelBuilder, currentUser, schedule.Id))
            {
                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = serviceRenderingsGridView.Rows[serviceRenderingsGridView.Rows.Add()];
                    }
                    ServiceRenderingsGridViewRenderRow(row, f.ServiceRendering);
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

                using (var f = new EditServiceRenderingForm(channelBuilder, currentUser, schedule.Id, serviceRendering.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        ServiceRenderingsGridViewRenderRow(row, f.ServiceRendering);
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

        private void isInterruptionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            interruptionPanel.Enabled = isInterruptionCheckBox.Checked;
        }

        private void isInterruptionCheckBox_Leave(object sender, EventArgs e)
        {
            schedule.IsInterruption = isInterruptionCheckBox.Checked;
        }

        private void interruptionStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.InterruptionStartTime = TimeSpan.Parse(interruptionStartTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void interruptionFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                schedule.InterruptionFinishTime = TimeSpan.Parse(interruptionFinishTimeTextBox.Text);
            }
            catch
            {
                throw new FormatException("Ошибочный формат времени");
            }
        }

        private void clientIntervalUpDown_Leave(object sender, EventArgs e)
        {
            schedule.ClientInterval = TimeSpan.FromMinutes((double)clientIntervalUpDown.Value);
        }

        private void intersectionUpDown_Leave(object sender, EventArgs e)
        {
            schedule.Intersection = TimeSpan.FromMinutes((double)intersectionUpDown.Value);
        }

        private void maxClientRequestsUpDown_Leave(object sender, EventArgs e)
        {
            schedule.ClientInterval = TimeSpan.FromMinutes((double)clientIntervalUpDown.Value);
        }

        private void renderingModeControl_SelectedChanged(object sender, EventArgs e)
        {
            var selectedMode = renderingModeControl.Selected<ServiceRenderingMode>();
            earlyGroupBox.Enabled = selectedMode == ServiceRenderingMode.AllRequests;
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
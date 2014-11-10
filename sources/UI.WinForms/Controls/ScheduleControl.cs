using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class ScheduleControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private bool initialized = false;

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
                    renderingModeComboBox.SelectedValue = schedule.RenderingMode;
                    earlyStartTimeTextBox.Text = schedule.EarlyStartTime.ToString("hh\\:mm");
                    earlyFinishTimeTextBox.Text = schedule.FinishTime.ToString("hh\\:mm");
                    earlyReservationUpDown.Value = schedule.EarlyReservation;

                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                await channel.Service.OpenUserSession(currentUser.SessionId);
                                foreach (var r in await taskPool.AddTask(channel.Service.GetServiceRenderings(schedule.Id)))
                                {
                                    int index = serviceRenderingsGridView.Rows.Add();
                                    var row = serviceRenderingsGridView.Rows[index];

                                    row.Cells["operatorColumn"].Value = r.Operator;
                                    row.Cells["modeColumn"].Value = r.Mode;
                                    row.Cells["serviceStepColumn"].Value = r.ServiceStep;
                                    row.Cells["priorityColumn"].Value = r.Priority;

                                    row.Tag = r;
                                }

                                Guid serviceId = GetServiceId();
                                if (serviceId != Guid.Empty)
                                {
                                    var serviceSteps = await taskPool.AddTask(channel.Service.GetServiceSteps(serviceId));
                                    serviceStepsComboBox.DataSource = new BindingSource(serviceSteps, null);

                                    serviceStepColumn.Visible
                                        = serviceStepPanel.Enabled
                                        = serviceSteps.Length > 0;
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

            modeColumn.DisplayMember = DataListItem.Value;
            modeColumn.ValueMember = DataListItem.Key;
            modeColumn.DataSource = EnumDataListItem<ServiceRenderingMode>.GetList();

            renderingModeComboBox.DisplayMember = DataListItem.Value;
            renderingModeComboBox.ValueMember = DataListItem.Key;
            renderingModeComboBox.DataSource = EnumDataListItem<ServiceRenderingMode>.GetList();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            initialized = true;
        }

        private Guid GetServiceId()
        {
            Guid serviceId = Guid.Empty;

            if (schedule is ServiceWeekdaySchedule)
            {
                serviceId = ((ServiceWeekdaySchedule)schedule).Service.Id;
            }
            else

                if (schedule is ServiceExceptionSchedule)
                {
                    serviceId = ((ServiceExceptionSchedule)schedule).Service.Id;
                }

            return serviceId;
        }

        private void RenderServiceRenderingsGridViewRow(DataGridViewRow row, ServiceRendering serviceRendering)
        {
            row.Cells["operatorColumn"].Value = serviceRendering.Operator;
            row.Cells["serviceStepColumn"].Value = serviceRendering.ServiceStep;
            row.Cells["modeColumn"].Value = ServiceRenderingMode.AllRequests;
            row.Cells["priorityColumn"].Value = serviceRendering.Priority;

            row.Tag = serviceRendering;
        }

        private async void ScheduleControl_Load(object sender, EventArgs e)
        {
            if (initialized)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await channel.Service.OpenUserSession(currentUser.SessionId);
                        operatorsComboBox.DataSource = new BindingSource(await taskPool.AddTask(channel.Service.GetOperators()), null);
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

        private async void addServiceRenderingButton_Click(object sender, EventArgs eventArgs)
        {
            var selectedOperator = operatorsComboBox.SelectedValue as Operator;
            var operatorId = selectedOperator.Id;

            var selectedsServiceStep = serviceStepsComboBox.SelectedValue as ServiceStep;
            var serviceStepId = selectedsServiceStep != null ? selectedsServiceStep.Id : Guid.Empty;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addServiceRenderingButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    var serviceRendering = await taskPool.AddTask(channel.Service.AddServiceRendering(schedule.Id, operatorId, serviceStepId));

                    var row = serviceRenderingsGridView.Rows[serviceRenderingsGridView.Rows.Add()];
                    RenderServiceRenderingsGridViewRow(row, serviceRendering);
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
                    addServiceRenderingButton.Enabled = true;
                }
            }
        }

        private async void serviceRenderingsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = serviceRenderingsGridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    switch (cell.OwningColumn.Name)
                    {
                        case "deleteColumn":
                            var serviceRendering = (ServiceRendering)row.Tag;

                            using (var channel = channelManager.CreateChannel())
                            {
                                try
                                {
                                    saveButton.Enabled = false;

                                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                                    await taskPool.AddTask(channel.Service.DeleteServiceRendering(serviceRendering.Id));

                                    serviceRenderingsGridView.Rows.Remove(row);
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

                            break;
                    }
                }
            }
        }

        private async void serviceRenderingsGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = serviceRenderingsGridView.Rows[rowIndex];
                    var serviceRendering = row.Tag as ServiceRendering;

                    object value = row.Cells["modeColumn"].Value;
                    if (value != null)
                    {
                        serviceRendering.Mode = (ServiceRenderingMode)value;
                    }

                    value = row.Cells["priorityColumn"].Value;
                    if (value != null)
                    {
                        try
                        {
                            serviceRendering.Priority = int.Parse(value.ToString());
                        }
                        catch (Exception exception)
                        {
                            UIHelper.Error(exception);
                        }
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            row.ReadOnly = true;

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.EditServiceRendering(serviceRendering));
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
                            row.ReadOnly = false;
                        }
                    }
                }
            }
        }

        private void isWorkedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            schedulePanel.Enabled = isWorkedCheckBox.Checked;
        }

        private void isWorkedCheckBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.IsWorked = isWorkedCheckBox.Checked;
            }
        }

        private void startTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void finishTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void isInterruptionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                interruptionPanel.Enabled = isInterruptionCheckBox.Checked;
            }
        }

        private void isInterruptionCheckBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.IsInterruption = isInterruptionCheckBox.Checked;
            }
        }

        private void interruptionStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void interruptionFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void clientIntervalUpDown_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.ClientInterval = TimeSpan.FromMinutes((double)clientIntervalUpDown.Value);
            }
        }

        private void intersectionUpDown_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.Intersection = TimeSpan.FromMinutes((double)intersectionUpDown.Value);
            }
        }

        private void maxClientRequestsUpDown_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.ClientInterval = TimeSpan.FromMinutes((double)clientIntervalUpDown.Value);
            }
        }

        private void renderingModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var renderingMode = (ServiceRenderingMode)renderingModeComboBox.SelectedValue;
            earlyGroupBox.Enabled = renderingMode == ServiceRenderingMode.AllRequests;
        }

        private void renderingModeComboBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                var renderingMode = (ServiceRenderingMode)renderingModeComboBox.SelectedValue;
                schedule.RenderingMode = renderingMode;
            }
        }

        private void earlyStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void earlyFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
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
        }

        private void earlyReservationUpDown_Leave(object sender, EventArgs e)
        {
            if (schedule != null)
            {
                schedule.EarlyReservation = (int)earlyReservationUpDown.Value;
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    await taskPool.AddTask(channel.Service.EditSchedule(schedule));
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
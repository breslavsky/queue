using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
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
    public partial class EditServiceForm : Queue.UI.WinForms.RichForm
    {
        public event EventHandler<EventArgs> Saved;

        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private Guid serviceGroupId;
        private ServiceGroup serviceGroup;
        private Guid serviceId;
        private TaskPool taskPool;

        #endregion fields

        #region properties

        public Service Service
        {
            get { return service; }
            private set
            {
                service = value;

                codeTextBox.Text = service.Code;
                priorityUpDown.Value = service.Priority;
                nameTextBox.Text = service.Name;
                commentTextBox.Text = service.Comment;
                tagsTextBox.Text = service.Tags;
                descriptionTextBox.Text = service.Description;
                linkTextBox.Text = service.Link;
                maxSubjectsUpDown.Value = service.MaxSubjects;
                maxEarlyDaysUpDown.Value = service.MaxEarlyDays;
                isUseTypeCheckBox.Checked = service.IsUseType;
                isPlanSubjectsCheckBox.Checked = service.IsPlanSubjects;
                clientCallDelayUpDown.Value = (int)service.ClientCallDelay.TotalSeconds;
                clientRequireCheckBox.Checked = service.ClientRequire;
                timeIntervalRoundingUpDown.Value = (int)service.TimeIntervalRounding.TotalMinutes;
                liveRegistratorFlagsControl.Select<ClientRequestRegistrator>(service.LiveRegistrator);
                earlyRegistratorFlagsControl.Select<ClientRequestRegistrator>(service.EarlyRegistrator);

                parametersTabPage.Parent =
                    exceptionScheduleTabPage.Parent =
                    weekdayScheduleTabPage.Parent =
                    stepsTabPage.Parent = service.Empty() ? null : serviceTabControl;

                exceptionScheduleDatePicker.Value = ServerDateTime.Today;
            }
        }

        #endregion properties

        public EditServiceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? serviceGroupId = null, Guid? serviceId = null)

            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceGroupId = serviceGroupId.HasValue
                ? serviceGroupId.Value : Guid.Empty;
            this.serviceId = serviceId.HasValue
                ? serviceId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            liveRegistratorFlagsControl.Initialize<ClientRequestRegistrator>();
            earlyRegistratorFlagsControl.Initialize<ClientRequestRegistrator>();

            serviceStepsControl.Initialize(channelBuilder, currentUser);
            weekdayScheduleControl.Initialize(channelBuilder, currentUser);
            exceptionScheduleControl.Initialize(channelBuilder, currentUser);
            serviceParametersControl.Initialize(channelBuilder, currentUser);
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

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    Service = await taskPool.AddTask(channel.Service.EditService(service));

                    if (Saved != null)
                    {
                        Saved(this, EventArgs.Empty);
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
                finally
                {
                    saveButton.Enabled = true;
                }
            }
        }

        private void EditServiceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void EditServiceForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Enabled = false;

                    if (serviceGroupId != Guid.Empty)
                    {
                        serviceGroup = await taskPool.AddTask(channel.Service.GetServiceGroup(serviceGroupId));
                    }

                    if (serviceId != Guid.Empty)
                    {
                        Service = await taskPool.AddTask(channel.Service.GetService(serviceId));
                    }
                    else
                    {
                        var all = ClientRequestRegistrator.Terminal
                            | ClientRequestRegistrator.Manager
                            | ClientRequestRegistrator.Portal;

                        Service = new Service()
                        {
                            IsActive = true,
                            ServiceGroup = serviceGroup,
                            Code = "0.0",
                            Name = "Новая услуга",
                            LiveRegistrator = all,
                            EarlyRegistrator = all
                        };
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
        }

        #region bindings

        private void clientCallDelayUpDown_Leave(object sender, EventArgs e)
        {
            service.ClientCallDelay = TimeSpan.FromSeconds((double)clientCallDelayUpDown.Value);
        }

        private void clientRequireCheckBox_Leave(object sender, EventArgs e)
        {
            service.ClientRequire = clientRequireCheckBox.Checked;
        }

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            service.Code = codeTextBox.Text;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            service.Comment = commentTextBox.Text;
        }

        private void descriptionTextBox_Click(object sender, EventArgs e)
        {
            using (var form = new HtmlEditorForm())
            {
                form.HTML = descriptionTextBox.Text;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    descriptionTextBox.Text = form.HTML;
                }
            }
        }

        private void descriptionTextBox_Leave(object sender, EventArgs e)
        {
            service.Description = descriptionTextBox.Text;
        }

        private void liveRegistratorFlagsControl_Leave(object sender, EventArgs e)
        {
            service.LiveRegistrator = liveRegistratorFlagsControl.Selected<ClientRequestRegistrator>();
        }

        private void isPlanSubjectsCheckBox_Leave(object sender, EventArgs e)
        {
            service.IsPlanSubjects = isPlanSubjectsCheckBox.Checked;
        }

        private void linkTextBox_Leave(object sender, EventArgs e)
        {
            service.Link = linkTextBox.Text;
        }

        private void earlyRegistratorFlagsControl_Leave(object sender, EventArgs e)
        {
            service.EarlyRegistrator = earlyRegistratorFlagsControl.Selected<ClientRequestRegistrator>();
        }

        private void maxEarlyDaysUpDown_Leave(object sender, EventArgs e)
        {
            service.MaxEarlyDays = (int)maxEarlyDaysUpDown.Value;
        }

        private void maxSubjectsUpDown_Leave(object sender, EventArgs e)
        {
            service.MaxSubjects = (int)maxSubjectsUpDown.Value;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            service.Name = nameTextBox.Text;
        }

        private void priorityUpDown_Leave(object sender, EventArgs e)
        {
            service.Priority = (int)priorityUpDown.Value;
        }

        private void tagsTextBox_Leave(object sender, EventArgs e)
        {
            service.Tags = tagsTextBox.Text;
        }

        private void timeIntervalRoundingUpDown_Leave(object sender, EventArgs e)
        {
            service.TimeIntervalRounding = TimeSpan.FromMinutes((double)timeIntervalRoundingUpDown.Value);
        }

        #endregion bindings

        private void serviceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = serviceTabControl.SelectedTab;
            if (selectedTab.Equals(stepsTabPage))
            {
                serviceStepsControl.Service = Service;
            }
            else if (selectedTab.Equals(weekdayScheduleTabPage))
            {
                LoadWeekdaySchedule();
            }
            else if (selectedTab.Equals(parametersTabPage))
            {
                serviceParametersControl.Service = Service;
            }
        }

        #region weekday schedule

        private async void LoadWeekdaySchedule()
        {
            var dayOfWeek = (DayOfWeek)int.Parse(weekdayTabControl.SelectedTab.Tag.ToString());

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    weekdayScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceWeekdaySchedule(service.Id, dayOfWeek));

                    weekdayScheduleCheckBox.Checked = true;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException<ObjectNotFoundFault>)
                {
                    weekdayScheduleCheckBox.Checked = false;
                    weekdayScheduleControl.Schedule = null;
                }
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

        private void weekdayScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            weekdayScheduleControl.Enabled = weekdayScheduleCheckBox.Checked;
        }

        private async void weekdayScheduleCheckBox_Click(object sender, EventArgs e)
        {
            var dayOfWeek = (DayOfWeek)int.Parse(weekdayTabControl.SelectedTab.Tag.ToString());

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    weekdayScheduleCheckBox.Enabled = false;

                    if (weekdayScheduleCheckBox.Checked)
                    {
                        weekdayScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceWeekdaySchedule(service.Id, dayOfWeek));
                    }
                    else
                    {
                        var schedule = weekdayScheduleControl.Schedule;
                        if (schedule != null)
                        {
                            await taskPool.AddTask(channel.Service.DeleteSchedule(schedule.Id));
                            weekdayScheduleControl.Schedule = null;
                        }
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
                finally
                {
                    weekdayScheduleCheckBox.Enabled = true;
                }
            }
        }

        private void weekdayTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            weekdaySchedulePanel.Parent = weekdayTabControl.SelectedTab;
            LoadWeekdaySchedule();
        }

        #endregion weekday schedule

        #region exception schedule

        private void exceptionScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionScheduleControl.Enabled = exceptionScheduleCheckBox.Checked;
        }

        private async void exceptionScheduleCheckBox_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    exceptionScheduleCheckBox.Enabled = false;

                    var scheduleDate = exceptionScheduleDatePicker.Value;

                    if (exceptionScheduleCheckBox.Checked)
                    {
                        exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceExceptionSchedule(service.Id, scheduleDate));
                    }
                    else
                    {
                        var schedule = exceptionScheduleControl.Schedule;
                        if (schedule != null)
                        {
                            await taskPool.AddTask(channel.Service.DeleteSchedule(schedule.Id));
                            exceptionScheduleControl.Schedule = null;
                        }
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException<ObjectNotFoundFault>)
                {
                    // nothing
                }
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
                    exceptionScheduleCheckBox.Enabled = true;
                }
            }
        }

        private async void exceptionScheduleDatePicker_ValueChanged(object sender, EventArgs e)
        {
            var scheduleDate = exceptionScheduleDatePicker.Value;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceExceptionSchedule(service.Id, scheduleDate));

                    exceptionScheduleCheckBox.Checked = true;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException<ObjectNotFoundFault>)
                {
                    exceptionScheduleCheckBox.Checked = false;
                    exceptionScheduleControl.Schedule = null;
                }
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

        #endregion exception schedule

        private void isUseTypeCheckBox_Leave(object sender, EventArgs e)
        {
            service.IsUseType = isUseTypeCheckBox.Checked;
        }

        private void serviceParametersControl_Load(object sender, EventArgs e)
        {
        }
    }
}
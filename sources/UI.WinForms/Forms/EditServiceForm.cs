using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class EditServiceForm : Queue.UI.WinForms.RichForm
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Guid serviceId;
        private Service service;

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
                isPlanSubjectsCheckBox.Checked = service.IsPlanSubjects;
                clientCallDelayUpDown.Value = (int)service.ClientCallDelay.TotalSeconds;
                clientRequireCheckBox.Checked = service.ClientRequire;
                timeIntervalRoundingUpDown.Value = (int)service.TimeIntervalRounding.TotalMinutes;

                var items = serviceTypeListBox.Items;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i] as EnumItem<ServiceType>;
                    if (service.Type.HasFlag(item.Value))
                    {
                        serviceTypeListBox.SetItemChecked(i, true);
                    }
                }

                items = liveRegistratorListBox.Items;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i] as EnumItem<ClientRequestRegistrator>;
                    if (service.LiveRegistrator.HasFlag(item.Value))
                    {
                        liveRegistratorListBox.SetItemChecked(i, true);
                    }
                }

                items = earlyRegistratorListBox.Items;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i] as EnumItem<ClientRequestRegistrator>;
                    if (service.EarlyRegistrator.HasFlag(item.Value))
                    {
                        earlyRegistratorListBox.SetItemChecked(i, true);
                    }
                }

                exceptionScheduleDatePicker.Value = ServerDateTime.Today;
            }
        }

        #endregion properties

        public EditServiceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid serviceId)

            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceId = serviceId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            var types = EnumItem<ServiceType>.GetItems().ToList();
            types.RemoveAll(t => t.Value == ServiceType.None);

            serviceTypeListBox.Items.AddRange(types.ToArray());

            var registrators1 = EnumItem<ClientRequestRegistrator>.GetItems().ToList();
            registrators1.RemoveAll(t => t.Value == ClientRequestRegistrator.None
                || t.Value == ClientRequestRegistrator.Portal);

            liveRegistratorListBox.Items.AddRange(registrators1.ToArray());

            var registrators2 = EnumItem<ClientRequestRegistrator>.GetItems().ToList();
            registrators2.RemoveAll(t => t.Value == ClientRequestRegistrator.None);

            earlyRegistratorListBox.Items.AddRange(registrators2.ToArray());

            serviceStepsControl.Initialize(channelBuilder, currentUser);
            weekdayScheduleControl.Initialize(channelBuilder, currentUser);
            exceptionScheduleControl.Initialize(channelBuilder, currentUser);
            serviceParametersControl.Initialize(channelBuilder, currentUser);
        }

        private async void ServiceEditForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    Service = await taskPool.AddTask(channel.Service.GetService(serviceId));
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

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            Service.Code = codeTextBox.Text;
        }

        private void priorityUpDown_Leave(object sender, EventArgs e)
        {
            Service.Priority = (int)priorityUpDown.Value;
        }

        private void clientRequireCheckBox_Leave(object sender, EventArgs e)
        {
            Service.ClientRequire = clientRequireCheckBox.Checked;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            Service.Name = nameTextBox.Text;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            Service.Comment = commentTextBox.Text;
        }

        private void linkTextBox_Leave(object sender, EventArgs e)
        {
            Service.Link = linkTextBox.Text;
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
            Service.Description = descriptionTextBox.Text;
        }

        private void tagsTextBox_Leave(object sender, EventArgs e)
        {
            Service.Tags = tagsTextBox.Text;
        }

        private void serviceTypeListBox_Leave(object sender, EventArgs e)
        {
            Service.Type = ServiceType.None;

            foreach (EnumItem<ServiceType> item in serviceTypeListBox.CheckedItems)
            {
                Service.Type |= item.Value;
            }
        }

        private void liveRegistratorListBox_Leave(object sender, EventArgs e)
        {
            Service.LiveRegistrator = ClientRequestRegistrator.None;

            foreach (EnumItem<ClientRequestRegistrator> item in liveRegistratorListBox.CheckedItems)
            {
                Service.LiveRegistrator |= item.Value;
            }
        }

        private void earlyRegistratorListBox_Leave(object sender, EventArgs e)
        {
            Service.EarlyRegistrator = ClientRequestRegistrator.None;

            foreach (EnumItem<ClientRequestRegistrator> item in liveRegistratorListBox.CheckedItems)
            {
                Service.EarlyRegistrator |= item.Value;
            }
        }

        private void timeIntervalRoundingUpDown_Leave(object sender, EventArgs e)
        {
            Service.TimeIntervalRounding = TimeSpan.FromMinutes((double)timeIntervalRoundingUpDown.Value);
        }

        private void clientCallDelayUpDown_Leave(object sender, EventArgs e)
        {
            Service.ClientCallDelay = TimeSpan.FromSeconds((double)clientCallDelayUpDown.Value);
        }

        private void maxSubjectsUpDown_Leave(object sender, EventArgs e)
        {
            Service.MaxSubjects = (int)maxSubjectsUpDown.Value;
        }

        private void maxEarlyDaysUpDown_Leave(object sender, EventArgs e)
        {
            Service.MaxEarlyDays = (int)maxEarlyDaysUpDown.Value;
        }

        private void isPlanSubjectsCheckBox_Leave(object sender, EventArgs e)
        {
            Service.IsPlanSubjects = isPlanSubjectsCheckBox.Checked;
        }

        #endregion bindings

        private async void saveServiceButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    Service = await taskPool.AddTask(channel.Service.EditService(Service));

                    DialogResult = DialogResult.OK;
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

        private void serviceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = serviceTabControl.SelectedTab;
            if (selectedTab.Tag == null)
            {
                switch (serviceTabControl.SelectedIndex)
                {
                    case 1:
                        serviceStepsControl.Service = Service;
                        break;

                    case 2:
                        LoadWeekdaySchedule();
                        break;

                    case 3:

                        break;

                    case 4:

                        serviceParametersControl.Service = Service;
                        break;
                }

                selectedTab.Tag = true;
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
                    await channel.Service.OpenUserSession(currentUser.SessionId);
                    weekdayScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceWeekdaySchedule(Service.Id, dayOfWeek));

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

        private void weekdayTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            weekdaySchedulePanel.Parent = weekdayTabControl.SelectedTab;
            LoadWeekdaySchedule();
        }

        private async void weekdayScheduleCheckBox_Click(object sender, EventArgs e)
        {
            var dayOfWeek = (DayOfWeek)int.Parse(weekdayTabControl.SelectedTab.Tag.ToString());

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    weekdayScheduleCheckBox.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    if (weekdayScheduleCheckBox.Checked)
                    {
                        weekdayScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceWeekdaySchedule(Service.Id, dayOfWeek));
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

        private void weekdayScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            weekdayScheduleControl.Enabled = weekdayScheduleCheckBox.Checked;
        }

        #endregion weekday schedule

        #region exception schedule

        private async void exceptionScheduleDatePicker_ValueChanged(object sender, EventArgs e)
        {
            var scheduleDate = exceptionScheduleDatePicker.Value;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceExceptionSchedule(Service.Id, scheduleDate));

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

        private async void exceptionScheduleCheckBox_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    exceptionScheduleCheckBox.Enabled = false;

                    var scheduleDate = exceptionScheduleDatePicker.Value;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    if (exceptionScheduleCheckBox.Checked)
                    {
                        exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceExceptionSchedule(Service.Id, scheduleDate));
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

        private void exceptionScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionScheduleControl.Enabled = exceptionScheduleCheckBox.Checked;
        }

        #endregion exception schedule

        private void ServiceEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
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
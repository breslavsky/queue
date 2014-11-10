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
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ServiceEditForm : Queue.UI.WinForms.RichForm
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Service service;

        #endregion fields

        #region properties

        public Service Service
        {
            get { return service; }
        }

        #endregion properties

        public ServiceEditForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Service service)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.service = service;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            var types = EnumDataListItem<ServiceType>.GetList();
            types.RemoveAll(t => t.Key == ServiceType.None);

            serviceTypeListBox.DataSource = types;
            serviceTypeListBox.DisplayMember = DataListItem.Value;
            serviceTypeListBox.ValueMember = DataListItem.Key;

            var registrators1 = EnumDataListItem<ClientRequestRegistrator>.GetList();
            registrators1.RemoveAll(t => t.Key == ClientRequestRegistrator.None
                || t.Key == ClientRequestRegistrator.Portal);

            liveRegistratorListBox.DataSource = registrators1;
            liveRegistratorListBox.DisplayMember = DataListItem.Value;
            liveRegistratorListBox.ValueMember = DataListItem.Key;

            var registrators2 = EnumDataListItem<ClientRequestRegistrator>.GetList();
            registrators2.RemoveAll(t => t.Key == ClientRequestRegistrator.None);

            earlyRegistratorListBox.DataSource = registrators2;
            earlyRegistratorListBox.DisplayMember = DataListItem.Value;
            earlyRegistratorListBox.ValueMember = DataListItem.Key;

            serviceStepsControl.Initialize(channelBuilder, currentUser);
            weekdayScheduleControl.Initialize(channelBuilder, currentUser);
            exceptionScheduleControl.Initialize(channelBuilder, currentUser);
            serviceParametersControl.Initialize(channelBuilder, currentUser);
        }

        private void ServiceEditForm_Load(object sender, EventArgs e)
        {
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
                var item = (KeyValuePair<ServiceType, string>)items[i];
                if (service.Type.HasFlag(item.Key))
                {
                    serviceTypeListBox.SetItemChecked(i, true);
                }
            }

            items = liveRegistratorListBox.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var item = (KeyValuePair<ClientRequestRegistrator, string>)items[i];
                if (service.LiveRegistrator.HasFlag(item.Key))
                {
                    liveRegistratorListBox.SetItemChecked(i, true);
                }
            }

            items = earlyRegistratorListBox.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var item = (KeyValuePair<ClientRequestRegistrator, string>)items[i];
                if (service.EarlyRegistrator.HasFlag(item.Key))
                {
                    earlyRegistratorListBox.SetItemChecked(i, true);
                }
            }

            exceptionScheduleDatePicker.Value = ServerDateTime.Today;
        }

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            service.Code = codeTextBox.Text;
        }

        private void priorityUpDown_Leave(object sender, EventArgs e)
        {
            service.Priority = (int)priorityUpDown.Value;
        }

        private void clientRequireCheckBox_Leave(object sender, EventArgs e)
        {
            service.ClientRequire = clientRequireCheckBox.Checked;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            service.Name = nameTextBox.Text;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            service.Comment = commentTextBox.Text;
        }

        private void linkTextBox_Leave(object sender, EventArgs e)
        {
            service.Link = linkTextBox.Text;
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

        private void tagsTextBox_Leave(object sender, EventArgs e)
        {
            service.Tags = tagsTextBox.Text;
        }

        private void serviceTypeListBox_Leave(object sender, EventArgs e)
        {
            service.Type = ServiceType.None;

            foreach (KeyValuePair<ServiceType, string> item in serviceTypeListBox.CheckedItems)
            {
                service.Type |= item.Key;
            }
        }

        private void liveRegistratorListBox_Leave(object sender, EventArgs e)
        {
            service.LiveRegistrator = ClientRequestRegistrator.None;

            foreach (KeyValuePair<ClientRequestRegistrator, string> item in liveRegistratorListBox.CheckedItems)
            {
                service.LiveRegistrator |= item.Key;
            }
        }

        private void earlyRegistratorListBox_Leave(object sender, EventArgs e)
        {
            service.EarlyRegistrator = ClientRequestRegistrator.None;

            foreach (KeyValuePair<ClientRequestRegistrator, string> item in earlyRegistratorListBox.CheckedItems)
            {
                service.EarlyRegistrator |= item.Key;
            }
        }

        private void timeIntervalRoundingUpDown_Leave(object sender, EventArgs e)
        {
            service.TimeIntervalRounding = TimeSpan.FromMinutes((double)timeIntervalRoundingUpDown.Value);
        }

        private void clientCallDelayUpDown_Leave(object sender, EventArgs e)
        {
            service.ClientCallDelay = TimeSpan.FromSeconds((double)clientCallDelayUpDown.Value);
        }

        private void maxSubjectsUpDown_Leave(object sender, EventArgs e)
        {
            service.MaxSubjects = (int)maxSubjectsUpDown.Value;
        }

        private void maxEarlyDaysUpDown_Leave(object sender, EventArgs e)
        {
            service.MaxEarlyDays = (int)maxEarlyDaysUpDown.Value;
        }

        private void isPlanSubjectsCheckBox_Leave(object sender, EventArgs e)
        {
            service.IsPlanSubjects = isPlanSubjectsCheckBox.Checked;
        }

        private async void saveServiceButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    service = await taskPool.AddTask(channel.Service.EditService(service));
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
                        serviceStepsControl.Service = service;
                        break;

                    case 2:
                        LoadWeekdaySchedule();
                        break;

                    case 3:

                        break;

                    case 4:

                        serviceParametersControl.Service = service;
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
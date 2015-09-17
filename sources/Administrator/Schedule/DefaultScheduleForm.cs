using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class DefaultScheduleForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region fields

        private DuplexChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        #endregion fields

        public DefaultScheduleForm()
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

        private void DefaultScheduleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void DefaultScheduleForm_Load(object sender, EventArgs e)
        {
            exceptionScheduleDatePicker.Value = ServerDateTime.Now;
            LoadWeekdaySchedule();
        }

        private void exceptionScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionScheduleControl.Enabled = exceptionScheduleCheckBox.Checked;
        }

        private async void exceptionScheduleCheckBox_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                if (exceptionScheduleCheckBox.Checked)
                {
                    var scheduleDate = exceptionScheduleDatePicker.Value;

                    try
                    {
                        exceptionScheduleCheckBox.Enabled = false;
                        exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddDefaultExceptionSchedule(scheduleDate));
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
                        exceptionScheduleCheckBox.Enabled = true;
                    }
                }
                else
                {
                    var schedule = exceptionScheduleControl.Schedule;
                    if (schedule != null)
                    {
                        try
                        {
                            exceptionScheduleCheckBox.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteSchedule(schedule.Id));
                            exceptionScheduleControl.Schedule = null;
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
            }
        }

        private async void exceptionScheduleDatePicker_ValueChanged(object sender, EventArgs e)
        {
            var scheduleDate = exceptionScheduleDatePicker.Value;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    exceptionScheduleDatePicker.Enabled = false;

                    exceptionScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetDefaultExceptionSchedule(scheduleDate));

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
                finally
                {
                    exceptionScheduleDatePicker.Enabled = true;
                }
            }
        }

        private async void LoadWeekdaySchedule()
        {
            var dayOfWeek = (DayOfWeek)int.Parse(weekdayTabControl.SelectedTab.Tag.ToString());

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    weekdayScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetDefaultWeekdaySchedule(dayOfWeek));
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

        private void weekdayTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            weekdayScheduleControl.Parent = weekdayTabControl.SelectedTab;
            LoadWeekdaySchedule();
        }
    }
}
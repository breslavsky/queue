using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class CurrentScheduleForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService ServerService { get; set; }

        #endregion dependency

        #region filelds

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private Service selectedService;

        #endregion filelds

        public CurrentScheduleForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void currentScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentScheduleControl.Enabled = currentScheduleCheckBox.Checked;
        }

        private async void currentScheduleCheckBox_Click(object sender, EventArgs e)
        {
            if (selectedService != null)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        currentScheduleCheckBox.Enabled = false;

                        var scheduleDate = ServerDateTime.Today;

                        if (currentScheduleCheckBox.Checked)
                        {
                            currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceExceptionSchedule(selectedService.Id, scheduleDate));
                        }
                        else
                        {
                            var schedule = currentScheduleControl.Schedule;
                            if (schedule != null)
                            {
                                await taskPool.AddTask(channel.Service.DeleteSchedule(schedule.Id));
                                currentScheduleControl.Schedule = null;
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
                        currentScheduleCheckBox.Enabled = true;
                    }
                }
            }
        }

        private async void selectServiceControl_ServiceSelected(object sender, EventArgs e)
        {
            selectedService = selectServiceControl.SelectedService;
            if (selectedService != null)
            {
                currentSchedulePanel.Enabled = true;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceExceptionSchedule(selectedService.Id, ServerDateTime.Today));

                        currentScheduleCheckBox.Checked = true;
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException<ObjectNotFoundFault>)
                    {
                        currentScheduleCheckBox.Checked = false;
                        currentScheduleControl.Schedule = null;
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
            else
            {
                currentSchedulePanel.Enabled = false;
            }
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}
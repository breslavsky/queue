using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class CurrentScheduleForm : RichForm
    {
        #region filelds

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        #endregion filelds

        #region properties

        private Service SelectedService { get { return selectServiceControl.Service; } }

        #endregion properties

        public CurrentScheduleForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private async void selectServiceControl_ServiceSelected(object sender, EventArgs e)
        {
            if (SelectedService != null)
            {
                currentSchedulePanel.Enabled = true;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceExceptionSchedule(SelectedService.Id, ServerDateTime.Today));

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

        private async void currentScheduleCheckBox_Click(object sender, EventArgs e)
        {
            if (SelectedService != null)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        currentScheduleCheckBox.Enabled = false;

                        var scheduleDate = ServerDateTime.Today;

                        if (currentScheduleCheckBox.Checked)
                        {
                            currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceExceptionSchedule(SelectedService.Id, scheduleDate));
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

        private void currentScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentScheduleControl.Enabled = currentScheduleCheckBox.Checked;
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }
    }
}